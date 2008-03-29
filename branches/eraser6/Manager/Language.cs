using System;
using System.Collections.Generic;
using System.Text;

namespace Eraser.Manager
{
	public abstract class Language
	{
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// The name of the language which this plugin implements. This should be
		/// the name of the language in the language itself, ie 简体华文 or Français.
		/// </summary>
		public abstract string Name
		{
			get;
		}

		/// <summary>
		/// Gets the GUID for the current language (to distinguish different 
		/// language DLLs)
		/// </summary>
		public abstract Guid GUID
		{
			get;
		}
	}

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
			LanguageEnglishUK lang = new LanguageEnglishUK();
			langs.Add(lang.GUID, lang);
		}

		/// <summary>
		/// Retrieves all currently registered Language plugins
		/// </summary>
		/// <returns>A list, with an instance of each Language class</returns>
		public static Dictionary<Guid, Language> GetAll()
		{
			lock (ManagerLibrary.Instance.LanguageManager.langs)
				return ManagerLibrary.Instance.LanguageManager.langs;
		}

		/// <summary>
		/// Retrieves the instance of the Language with the given GUID.
		/// </summary>
		/// <param name="guid">The GUID of the Language.</param>
		/// <returns>The Language instance.</returns>
		public static Language GetInstance(Guid guid)
		{
			try
			{
				lock (ManagerLibrary.Instance.LanguageManager.langs)
					return ManagerLibrary.Instance.LanguageManager.langs[guid];
			}
			catch (KeyNotFoundException)
			{
				throw new FatalException("Language not found: " + guid.ToString());
			}
		}

		/// <summary>
		/// Allows plug-ins to register Language with the main program.
		/// </summary>
		/// <param name="method"></param>
		public static void Register(Language lang)
		{
			lock (ManagerLibrary.Instance.LanguageManager.langs)
				ManagerLibrary.Instance.LanguageManager.langs.Add(lang.GUID, lang);
		}

		/// <summary>
		/// The list of current Languages
		/// </summary>
		private Dictionary<Guid, Language> langs = new Dictionary<Guid, Language>();
	}

	/// <summary>
	/// The default English (United Kingdom) language for the UI. This is the default.
	/// </summary>
	internal class LanguageEnglishUK : Language
	{
		public override string Name
		{
			get { return "English (United Kingdom)"; }
		}

		public override Guid GUID
		{
			get { return new Guid("{00000000-0000-0000-0000-000000000000}"); }
		}
	}
}
