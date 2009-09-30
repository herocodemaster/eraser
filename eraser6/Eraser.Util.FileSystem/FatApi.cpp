/* 
 * $Id$
 * Copyright 2009 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By: 
 * 
 * This file is part of Eraser.
 * 
 * Eraser is free software: you can redistribute it and/or modify it under the
 * terms of the GNU General Public License as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option) any later
 * version.
 * 
 * Eraser is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
 * A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * 
 * A copy of the GNU General Public License can be found at
 * <http://www.gnu.org/licenses/>.
 */

#include <stdafx.h>
#include <windows.h>

#include "FatApi.h"

using namespace System::Collections::Generic;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace Eraser {
namespace Util {
	FatApi::FatApi(VolumeInfo^ info)
	{
		SectorSize = info->SectorSize;
		ClusterSize = info->ClusterSize;

		BootSector = new FatBootSector();
		memset(BootSector, 0, sizeof(*BootSector));
		Fat = NULL;

		//Open the handle to the drive
		VolumeStream = info->Open(FileAccess::Read);

		//Then read the boot sector for information
		array<Byte>^ bootSector = gcnew array<Byte>(sizeof(*BootSector));
		VolumeStream->Seek(0, SeekOrigin::Begin);
		VolumeStream->Read(bootSector, 0, sizeof(*BootSector));
		Marshal::Copy(bootSector, 0, static_cast<IntPtr>(BootSector), bootSector->Length);

		//Then load the FAT
		LoadFat();
	}

	FatApi::FatApi(VolumeInfo^ info, Stream^ stream)
	{
		SectorSize = info->SectorSize;
		ClusterSize = info->ClusterSize;

		BootSector = new FatBootSector();
		memset(BootSector, 0, sizeof(*BootSector));
		Fat = NULL;

		//Open the handle to the drive
		VolumeStream = stream;

		//Then read the boot sector for information
		array<Byte>^ bootSector = gcnew array<Byte>(sizeof(*BootSector));
		VolumeStream->Seek(0, SeekOrigin::Begin);
		VolumeStream->Read(bootSector, 0, sizeof(*BootSector));
		Marshal::Copy(bootSector, 0, static_cast<IntPtr>(BootSector), bootSector->Length);

		//Then load the FAT
		LoadFat();
	}

	FatDirectoryBase^ FatApi::LoadDirectory(String^ directory)
	{
		//Return the root directory if nothing is specified
		if (String::IsNullOrEmpty(directory))
			return LoadDirectory(DirectoryToCluster(directory), String::Empty, nullptr);

		array<wchar_t>^ pathSeparators = { Path::DirectorySeparatorChar, Path::AltDirectorySeparatorChar };
		int lastIndex = directory->LastIndexOfAny(pathSeparators);
		return LoadDirectory(DirectoryToCluster(directory), directory->Substring(lastIndex + 1),
			LoadDirectory(directory->Substring(0, lastIndex)));
	}

	unsigned long long FatApi::SectorToOffset(unsigned long long sector)
	{
		return sector * SectorSize;
	}

	unsigned FatApi::SectorSizeToSize(unsigned size)
	{
		return size * SectorSize;
	}

	unsigned FatApi::ClusterSizeToSize(unsigned size)
	{
		return size * ClusterSize;
	}

	std::vector<char> FatApi::GetFileContents(unsigned cluster)
	{
		if (!IsClusterAllocated(cluster))
			throw gcnew ArgumentException(L"The specified cluster is not used.");

		std::vector<char> result;
		result.reserve(FileSize(cluster));
		array<Byte>^ buffer = gcnew array<Byte>(ClusterSize);

		do
		{
			VolumeStream->Seek(ClusterToOffset(cluster), SeekOrigin::Begin);
			VolumeStream->Read(buffer, 0, ClusterSize);

			result.insert(result.end(), ClusterSize, 0);
			Marshal::Copy(buffer, 0, static_cast<IntPtr>(&result.back() - ClusterSize + 1),
				ClusterSize);
		}
		while ((cluster = GetNextCluster(cluster)) != 0xFFFFFFFF);

		return result;
	}

	void FatApi::SetFileContents(const void* data, size_t length, unsigned cluster)
	{
		if (!IsClusterAllocated(cluster))
			throw gcnew ArgumentException(L"The specified cluster is not used.");
		if (length != FileSize(cluster))
			throw gcnew ArgumentException(L"The provided file contents will not fit in the " +
				gcnew String(L"allocated file."));

		array<Byte>^ buffer = gcnew array<Byte>(ClusterSize);
		for (size_t i = 0; i < length; i += ClusterSize)
		{
			Marshal::Copy(static_cast<IntPtr>(reinterpret_cast<intptr_t>(static_cast<const char*>(data) + i)),
				buffer, 0, ClusterSize);
			VolumeStream->Seek(ClusterToOffset(cluster), SeekOrigin::Begin);
			VolumeStream->Write(buffer, 0, ClusterSize);
			cluster = GetNextCluster(cluster);
		}
	}

	FatDirectoryEntry::FatDirectoryEntry(String^ name, FatDirectoryBase^ parent,
		FatDirectoryEntryType type, unsigned cluster)
	{
		Name = name;
		Parent = parent;
		EntryType = type;
		Cluster = cluster;
	}

	String^ FatDirectoryEntry::FullName::get()
	{
		String^ result = Name;
		FatDirectoryEntry^ currentEntry = this;

		while (currentEntry->Parent != nullptr)
		{
			currentEntry = currentEntry->Parent;
			result = currentEntry->Name + Path::DirectorySeparatorChar + result;
		}

		return result;
	}

	FatDirectoryBase::FatDirectoryBase(String^ name, FatDirectoryBase^ parent, unsigned cluster)
		: FatDirectoryEntry(name, parent, FatDirectoryEntryType::Directory, cluster)
	{
		Entries = gcnew Dictionary<String^, FatDirectoryEntry^>();
		ReadDirectory();
	}

	void FatDirectoryBase::ClearDeletedEntries()
	{
		std::vector<::FatDirectoryEntry> validEntries;

		//Parse the directory structures
		for (::FatDirectoryEntry* i = Directory; i != Directory + DirectorySize; ++i)
		{
			//Check if we have checked the last valid entry
			if (i->Short.Name[0] == 0x00)
				break;

			//Skip deleted entries.
			if (static_cast<unsigned char>(i->Short.Name[0]) == 0xE5)
				continue;

			if (i->Short.Attributes == 0x0F)
			{
				//This is a long file name.
				::FatDirectoryEntry* longFileNameBegin = i;
				for (unsigned char sequence = 0; i->Short.Attributes == 0x0F; ++i)
				{
					if (static_cast<unsigned char>(i->Short.Name[0]) == 0xE5)
						continue;
					else if (!(i->LongFileName.Sequence & 0x40)) //Second entry onwards
					{
						//Check that the checksum of the file name is the same as the previous
						//long file name entry, to ensure no corruption has taken place
						if ((i - 1)->LongFileName.Checksum != i->LongFileName.Checksum)
							continue;

						//Check that the sequence is one less than the previous one.
						if (sequence != i->LongFileName.Sequence + 1)
							throw gcnew ArgumentException(L"Invalid directory entry.");
					}
					
					sequence = i->LongFileName.Sequence & ~0x40;
				}

				//Checksum the string
				unsigned char sum = 0;
				char* shortFileName = i->Short.Name;
				for (int j = 11; j; --j)
					sum = ((sum & 1) << 7) + (sum >> 1) + *shortFileName++;

				if (sum == (i - 1)->LongFileName.Checksum)
				{
					//The previous few entries contained the correct file name. Save these entries
					validEntries.insert(validEntries.end(), longFileNameBegin, i);
				}
				else
				{
					--i;
					continue;
				}
			}

			validEntries.push_back(*i);
		}

		//validEntries now contains the compacted list of directory entries. Zero
		//the memory used.
		memset(Directory, 0, DirectorySize * sizeof(::FatDirectoryEntry));

		//Copy the memory back if we have any valid entries. The root directory can
		//be empty (no . and .. entries)
		if (!validEntries.empty())
			memcpy(Directory, &validEntries.front(), validEntries.size() * sizeof(::FatDirectoryEntry));

		//Write the entries to disk
		WriteDirectory();
	}

	void FatDirectoryBase::ParseDirectory()
	{
		//Clear the list of entries
		Entries->Clear();

		//Parse the directory structures
		for (::FatDirectoryEntry* i = Directory; i != Directory + DirectorySize; ++i)
		{
			//Check if we have checked the last valid entry
			if (i->Short.Name[0] == 0x00)
				break;

			//Skip deleted entries.
			if (static_cast<unsigned char>(i->Short.Name[0]) == 0xE5)
				continue;

			if (i->Short.Attributes == 0x0F)
			{
				//This is a long file name.
				std::wstring longFileName;
				for (unsigned char sequence = 0; i->Short.Attributes == 0x0F; ++i)
				{
					if (static_cast<unsigned char>(i->Short.Name[0]) == 0xE5)
						continue;
					else if (!(i->LongFileName.Sequence & 0x40)) //Second entry onwards
					{
						//Check that the checksum of the file name is the same as the previous
						//long file name entry, to ensure no corruption has taken place
						if ((i - 1)->LongFileName.Checksum != i->LongFileName.Checksum)
							continue;

						//Check that the sequence is one less than the previous one.
						if (sequence != i->LongFileName.Sequence + 1)
							throw gcnew ArgumentException(L"Invalid directory entry.");
					}
					else
						longFileName.clear();
					
					sequence = i->LongFileName.Sequence & ~0x40;
					std::wstring namePart(i->LongFileName.Name1, sizeof(i->LongFileName.Name1) / sizeof(i->LongFileName.Name1[0]));
					namePart += std::wstring(i->LongFileName.Name2, sizeof(i->LongFileName.Name2) / sizeof(i->LongFileName.Name2[0]));
					namePart += std::wstring(i->LongFileName.Name3, sizeof(i->LongFileName.Name3) / sizeof(i->LongFileName.Name3[0]));
					namePart += longFileName;
					longFileName = namePart;
				}

				//Checksum the string
				unsigned char sum = 0;
				char* shortFileName = i->Short.Name;
				for (int j = 11; j; --j)
					sum = ((sum & 1) << 7) + (sum >> 1) + *shortFileName++;

				if (sum == (i - 1)->LongFileName.Checksum)
				{
					//fileName contains the correct full long file name, strip the file name of the
					//invalid characters.
					String^ fileName = gcnew String(longFileName.c_str());
					Entries->Add(fileName, gcnew FatDirectoryEntry(fileName, this,
						(i->Short.Attributes & FILE_ATTRIBUTE_DIRECTORY) ?
							FatDirectoryEntryType::Directory : FatDirectoryEntryType::File,
						GetStartCluster(*i)));
				}
				else
				{
					--i;
					continue;
				}
			}

			//Skip the dot directories.
			if (i->Short.Name[0] == '.')
				continue;

			//Substitute 0x05 with 0xE5
			if (i->Short.Name[0] == 0x05)
				i->Short.Name[0] = static_cast<unsigned char>(0xE5);
			
			//Then read the 8.3 entry for the file details
			wchar_t shortFileName[8 + 3 + 2];
			if (mbstowcs(shortFileName, i->Short.Name, sizeof(i->Short.Name)) != sizeof(i->Short.Name))
				continue;

			//If the extension is blank, don't care about it
			if (strncmp(i->Short.Extension, "   ", 3) == 0)
			{
				shortFileName[8] = '\0';
			}
			else
			{
				mbstowcs(shortFileName + 9, i->Short.Extension, sizeof(i->Short.Extension) + 1);
				shortFileName[8] = L'.';
				shortFileName[8 + 3 + 1] = '\0';
			}

			String^ fileName = gcnew String(shortFileName);
			Entries->Add(fileName, gcnew FatDirectoryEntry(fileName, this,
				(i->Short.Attributes & FILE_ATTRIBUTE_DIRECTORY) ?
					FatDirectoryEntryType::Directory : FatDirectoryEntryType::File,
				GetStartCluster(*i)));
		}
	}

	FatDirectory::FatDirectory(String^ name, FatDirectoryBase^ parent, unsigned cluster, FatApi^ api)
		: Api(api),
		  FatDirectoryBase(name, parent, cluster)
	{
	}

	void FatDirectory::ReadDirectory()
	{
		std::vector<char> dir = Api->GetFileContents(Cluster);
		DirectorySize = dir.size() / sizeof(::FatDirectoryEntry);
		Directory = new ::FatDirectoryEntry[DirectorySize];
		memcpy(Directory, &dir.front(), dir.size());

		ParseDirectory();
	}

	void FatDirectory::WriteDirectory()
	{
		Api->SetFileContents(Directory, Api->FileSize(Cluster), Cluster);
	}
}
}
