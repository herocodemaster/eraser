/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;


namespace ComLib.Configuration
{
    /// <summary>
    /// Configuration source interface for loading/viewing/saving settings.
    /// </summary>
    public interface IConfigSource : IConfigSection
    {
        /// <summary>
        /// Event handler when the configuration store changes.
        /// </summary>
        event EventHandler OnConfigSourceChanged;


        /// <summary>
        /// Name of the source.
        /// This cane be the file path.
        /// </summary>
        string SourcePath { get; }


        /// <summary>
        /// Load the config settings from the underlying datasource.
        /// </summary>
        void Load();


        /// <summary>
        /// Save the config settings to the underlying datasource.
        /// </summary>
        void Save();
    }



    /// <summary> 
    /// Base class for config settings. 
    /// This stores settings in 
    /// 1. At the root level ( similiar to AppSettings ). 
    /// 2. At a section level ( similar to GetSection("SectionName") ); 
    /// </summary> 
    /// <remarks> 
    /// The following properties are associated with 
    /// storing settings at the root level. 
    /// 1. Count 
    /// </remarks> 
    /// <typeparam name="T"></typeparam> 
    public interface IConfigSection : IDictionary
    {
        /// <summary>
        /// The name of the this config section.
        /// </summary>
        string Name { get; set; }
        

        /// <summary>
        /// Get typed value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);


        /// <summary>
        /// Get key value if preset, default value otherwise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetDefault<T>(string key, T defaultValue);

        
        /// <summary>
        /// Get the section's key value.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string section, string key);


        /// <summary>
        /// Get the section's key's specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string section, string key);

        
        /// <summary>
        /// Get section/key value if preset, default value otherwise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetDefault<T>(string section, string key, T defaultValue);
        

        /// <summary>
        /// Get the section with the name specified.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        IConfigSection GetSection(string sectionName);


        /// <summary>
        /// Get sectionlist with the specified name.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="ndx"></param>
        /// <returns></returns>
        IConfigSection GetSection(string sectionName, int ndx);


        /// <summary>
        /// Get the section key value using the indexer.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string sectionName, string key] { get; set; }


        /// <summary>
        /// Checks whether or not the key exists in the section.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string section, string key);


        /// <summary>
        /// Get the names of the sections.
        /// </summary>
        List<string> Sections { get; }


        /// <summary>
        /// Add to value to the section/key combination.
        /// </summary>
        /// <param name="sectionName">"ApplicationSettings"</param>
        /// <param name="key">PageSize</param>
        /// <param name="val">15</param>
        void Add(string sectionName, string key, object val, bool overWrite);


        /// <summary>
        /// Add section key/value item.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void Add(string sectionName, string key, object val);


        /// <summary>
        /// Add to value to the section/key combination.
        /// </summary>
        /// <param name="sectionName">"ApplicationSettings"</param>
        /// <param name="key">PageSize</param>
        /// <param name="val">15</param>
        void AddMulti(string key, object val, bool overWrite);
    }
}
