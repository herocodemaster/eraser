using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.IO;

namespace Eraser.Manager
{
	/// <summary>
	/// A class managing all plugins dealing with languages.
	/// </summary>
	public class LanguageManager
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public LanguageManager()
		{
		}

		/// <summary>
		/// Retrieves all present language plugins
		/// </summary>
		/// <returns>A list, with an instance of each Language class</returns>
		public static List<CultureInfo> GetAll()
		{
			List<CultureInfo> result = new List<CultureInfo>();
			foreach (CultureInfo info in CultureInfo.GetCultures(CultureTypes.AllCultures))
			{
				if (info.Name == string.Empty)
					continue;
				else if (new DirectoryInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
					Path.DirectorySeparatorChar + info.Name).Exists)
					result.Add(info);
			}

			//Last resort
			if (result.Count == 0)
				result.Add(CultureInfo.GetCultureInfo("EN"));
			return result;
		}
	}
}
