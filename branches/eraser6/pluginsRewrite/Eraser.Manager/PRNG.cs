/* 
 * $Id$
 * Copyright 2008-2010 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By: Kasra Nassiri <cjax@users.sourceforge.net>
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

using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Microsoft.Win32.SafeHandles;

using Eraser.Util;
using Eraser.Plugins;
using Eraser.Plugins.ExtensionPoints;

namespace Eraser.Manager
{
	/// <summary>
	/// Class managing all the PRNG algorithms.
	/// </summary>
	public class PrngRegistrar : Registrar<Prng>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		internal PrngRegistrar()
		{
		}

		/// <summary>
		/// Allows the EntropyThread to get entropy to the PRNG functions as seeds.
		/// </summary>
		/// <param name="entropy">An array of bytes, being entropy for the PRNG.</param>
		internal void AddEntropy(byte[] entropy)
		{
			lock (ManagerLibrary.Instance.PrngRegistrar)
				foreach (Prng prng in ManagerLibrary.Instance.PrngRegistrar)
					prng.Reseed(entropy);
		}

		/// <summary>
		/// Gets entropy from the EntropyThread.
		/// </summary>
		/// <returns>A buffer of arbitrary length containing random information.</returns>
		internal static byte[] GetEntropy()
		{
			return ManagerLibrary.Instance.EntropySourceRegistrar.Poller.GetPool();
		}
	}
}
