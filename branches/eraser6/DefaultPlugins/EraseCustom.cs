/* 
 * $Id$
 * Copyright 2008 The Eraser Project
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

using Eraser.Manager;

namespace Eraser.DefaultPlugins
{
	[Serializable]
	class EraseCustom : PassBasedErasureMethod
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="method">The erasure method definition for the custom method.</param>
		public EraseCustom(CustomErasureMethod method)
		{
			this.method = method;
		}

		/// <summary>
		/// Registers all defined custom methods with the method manager.
		/// </summary>
		internal static void RegisterAll()
		{
			if (DefaultPlugin.Settings.EraseCustom == null)
				return;

			Dictionary<Guid, CustomErasureMethod> methods =
				DefaultPlugin.Settings.EraseCustom;
			foreach (Guid guid in methods.Keys)
			{
				CustomErasureMethod method = methods[guid];
				ErasureMethodManager.Register(new EraseCustom(method), new object[] { method });
			}
		}

		public override string Name
		{
			get { return method.Name; }
		}

		public override Guid Guid
		{
			get { return method.GUID; }
		}

		protected override bool RandomizePasses
		{
			get { return method.RandomizePasses; }
		}

		protected override Pass[] PassesSet
		{
			get { return method.Passes; }
		}

		CustomErasureMethod method;
	}

	/// <summary>
	/// Contains information necessary to create user-defined erasure methods.
	/// </summary>
	[Serializable]
	public class CustomErasureMethod : ISerializable
	{
		public CustomErasureMethod()
		{
			Name = string.Empty;
			GUID = Guid.Empty;
			RandomizePasses = true;
			Passes = null;
		}

		public CustomErasureMethod(SerializationInfo info, StreamingContext context)
		{
			Name = info.GetString("Name");
			GUID = (Guid)info.GetValue("GUID", GUID.GetType());
			RandomizePasses = info.GetBoolean("RandomizePasses");
			List<PassData> passes = (List<PassData>)
				info.GetValue("Passes", typeof(List<PassData>));

			Passes = new ErasureMethod.Pass[passes.Count];
			for (int i = 0; i != passes.Count; ++i)
				Passes[i] = passes[i];
		}

		public string Name;
		public Guid GUID;
		public bool RandomizePasses;
		public ErasureMethod.Pass[] Passes;

		#region ISerializable Members
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", Name);
			info.AddValue("GUID", GUID);
			info.AddValue("RandomizePasses", RandomizePasses);

			List<PassData> passes = new List<PassData>(Passes.Length);
			foreach (ErasureMethod.Pass pass in Passes)
				passes.Add(new PassData(pass));
			info.AddValue("Passes", passes);
		}

		[Serializable]
		private class PassData
		{
			public PassData(ErasureMethod.Pass pass)
			{
				if (pass.Function == ErasureMethod.Pass.WriteConstant)
				{
					Random = false;
					OpaqueValue = pass.OpaqueValue;
				}
				else if (pass.Function == ErasureMethod.Pass.WriteRandom)
				{
					Random = true;
					OpaqueValue = null;
				}
				else
					throw new ArgumentException("The custom erasure method can only comprise " +
						"passes containining constant or random passes");
			}

			public static implicit operator ErasureMethod.Pass(PassData pass)
			{
				ErasureMethod.Pass result = new ErasureMethod.Pass(pass.Random ?
					ErasureMethod.Pass.WriteRandom :
					ErasureMethod.Pass.WriteConstant, pass.OpaqueValue);
				return result;
			}

			object OpaqueValue;
			bool Random;
		}
		#endregion
	}
}
