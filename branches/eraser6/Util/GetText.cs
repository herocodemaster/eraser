using System;
using System.Collections.Generic;
using System.Text;

using GNU.Gettext;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;

namespace Eraser.Util
{
	/// <summary>
	/// GetText shortcut class. Instead of calling GetString on all strings, just
	/// call S._(string) or S._(string, object) for plurals
	/// </summary>
	public static class S
	{
		/// <summary>
		/// Translates the localizable string to the set localized string.
		/// </summary>
		/// <param name="str">The string to localize.</param>
		/// <returns>A localized string, or str if no localization exists.</returns>
		public static string _(string str)
		{
			return TranslateText(str, Assembly.GetCallingAssembly());
		}

		/// <summary>
		/// Translates the localizable string to the set localized string.
		/// </summary>
		/// <param name="str">The string to localize.</param>
		/// <param name="assembly">The assembly from which localized resource satellite
		/// assemblies should be loaded from.</param>
		/// <returns>A localized string, or str if no localization exists.</returns>
		public static string TranslateText(string str, Assembly assembly)
		{
			GettextResourceManager res = null;
			if (!managers.ContainsKey(assembly))
			{
				res = new GettextResourceManager(
					Path.GetFileNameWithoutExtension(assembly.Location), assembly);
				managers[assembly] = res;
			}
			else
				res = managers[assembly];

			return res.GetString(str, Language);
		}

		/// <summary>
		/// Translates a form control 
		/// </summary>
		/// <param name="c">The control to translate. Certain classes will be dealt with
		/// individually.</param>
		public static void TranslateControl(Control c)
		{
			TranslateControl(c, Assembly.GetCallingAssembly());
		}

		private static void TranslateControl(Control c, Assembly assembly)
		{
			c.Text = TranslateText(c.Text, assembly);

			if (c is TranslatableControl)
				((TranslatableControl)c).Translate();
			else if (c is ListView)
			{
				ListView lv = (ListView)c;
				foreach (ListViewGroup group in lv.Groups)
					group.Header = TranslateText(group.Header, assembly);
				foreach (ColumnHeader header in lv.Columns)
					header.Text = TranslateText(header.Text, assembly);
			}

			foreach (Control child in c.Controls)
				TranslateControl(child, assembly);
		}

		/// <summary>
		/// The current culture to use when looking up for localizations.
		/// </summary>
		public static CultureInfo Language = CultureInfo.CurrentUICulture;
		private static Dictionary<Assembly, GettextResourceManager> managers =
			new Dictionary<Assembly, GettextResourceManager>();

		/// <summary>
		/// Translatable control interface. Implement this interface to allow the
		/// control to be translated by gettext at runtime
		/// </summary>
		public interface TranslatableControl
		{
			/// <summary>
			/// Translates all strings in the control
			/// </summary>
			void Translate();
		}
	}
}
