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
using System.Collections.ObjectModel;
using System.Text;
using System.Management;
using System.IO;
using System.Collections;
using System.Reflection;
using System.ServiceProcess;
using System.Web;
using System.Diagnostics;


namespace ComLib.Diagnostics
{

    /// <summary>
    /// Get diagnostic information about the machine and current process.
    /// This includes information for the following set of data:
    /// 1. MachineInfo
    /// 2. Env_System
    /// 3. Env_User
    /// 4. Drives
    /// 5. AppDomain
    /// 6. Services
    /// 7. Processes
    /// 8. Modules
    /// </summary>
    public class DiagnosticsService : IDiagnosticsService
    {
        private IList<string> sections = new List<string>();
        private IDictionary<string, Func<IDictionary>> _diagnosticsRetrievers = new SortedDictionary<string, Func<IDictionary>>();
        private ReadOnlyCollection<string> _diagnostricGroups;
        private ReadOnlyCollection<string> _diagnostricGroupsAll;
        private IDictionary<string, string> _diagnosticGroupsAllMap = new Dictionary<string, string>();


        /// <summary>
        /// Initalizes a list representing a set of computer/application related data that can be diagnosed.
        /// This includes:
        /// 1. Machine Information, 2. Environment variables. 3. Drives, 4. AppDomain ( dlls loaded )., etc.
        /// </summary>
        public DiagnosticsService()
        {
            Init();
        }


        /// <summary>
        /// Initalizes a list representing a set of computer/application related data that can be diagnosed.
        /// This includes:
        /// 1. Machine Information, 2. Environment variables. 3. Drives, 4. AppDomain ( dlls loaded )., etc.
        /// </summary>
        public DiagnosticsService(List<string> diagnosticGroups, bool include)
            : this()
        {

        }


        /// <summary>
        /// Filter the diagnostics on the supplied comma delimited list of groups
        /// representing the areas that can be diagnosed.
        /// </summary>
        /// <param name="groupNamesDelimited">"MachineInfo,AppDomain,Drives"</param>
        /// <param name="include">Whether or the the groups supplied should be
        /// included, false value representing exclusion.</param>
        public void FilterOn(string groupNamesDelimited, bool include)
        {
            string[] groups = groupNamesDelimited.Split(',');
            List<string> groupNames = new List<string>(groups);
            FilterOn(groupNames, include);
        }


        /// <summary>
        /// Filter the diagnostics on the list of groups
        /// representing the areas that can be diagnosed.
        /// </summary>
        /// <param name="groupNamesDelimited">"MachineInfo,AppDomain,Drives"</param>
        /// <param name="include">Whether or the the groups supplied should be
        /// included, false value representing exclusion.</param>
        public void FilterOn(List<string> groupNames, bool include)
        {
            Init();
            List<string> excluded = groupNames;
            if (include)
            {
                // Create lookup of included group names.
                Dictionary<string, string> included = new Dictionary<string, string>();
                foreach (string group in groupNames)
                    included[group] = group;

                excluded = new List<string>();

                // Now get the names of the items to exclude.
                foreach (string group in _diagnostricGroupsAll)
                {
                    if (!included.ContainsKey(group))
                        excluded.Add(group);
                }
            }
            // Remove all excluded items.
            foreach (string excludedGroup in excluded)
            {
                if (_diagnosticsRetrievers.ContainsKey(excludedGroup))
                    _diagnosticsRetrievers.Remove(excludedGroup);
            }

            // Reset the names stored of all items being handled.
            StoreGroupsBeingProcessed();
        }


        /// <summary>
        /// The names of the groups representing what can be diagnosed.
        /// </summary>
        public ReadOnlyCollection<string> GroupNames
        {
            get { return _diagnostricGroups; }
        }


        /// <summary>
        /// The names of the groups representing what can be diagnosed.
        /// </summary>
        public ReadOnlyCollection<string> GroupNamesAll
        {
            get { return _diagnostricGroupsAll; }
        }


        /// <summary>
        /// Get all diagnostic information about currently running process
        /// and machine information.
        /// </summary>
        /// <returns></returns>
        public string GetDataTextual()
        {
            IDictionary diagnostics = GetData();
            StringBuilder buffer = new StringBuilder();
            try
            {
                BuildDiagnostics(buffer, diagnostics);
            }
            catch (Exception ex)
            {
                buffer.Append("Error ocurred attempting to get diagnostic information. " + ex.Message);
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Write all the diagnostic info to file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public void WriteInfo(string filePath)
        {
            try
            {
                string data = GetDataTextual();
                File.WriteAllText(filePath, data);
            }
            catch { Console.WriteLine("Unable to write diagnostic information to file : " + filePath); }
        }


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="commaDelimitedGroups">"Machine,AppDomain"</param>
        /// <param name="path">Path of file to write information to.</param>
        public void WriteInfo(string commaDelimitedGroups, string path, string referenceMessage)
        {            
            if (!string.IsNullOrEmpty(commaDelimitedGroups))
            {
                FilterOn(commaDelimitedGroups, true);
            }
            string data = GetDataTextual();
            string message = "[Message]" + Environment.NewLine 
                            + referenceMessage
                            + Environment.NewLine + Environment.NewLine + Environment.NewLine
                            + data;
            try
            {
                File.WriteAllText(path, message);
            }
            catch { Console.WriteLine("Unable to write diagnostic information to file : " + path); }
        }


        /// <summary>
        /// Get all diagnostic information about currently running process
        /// and machine information.
        /// </summary>
        /// <returns></returns>
        public IDictionary GetData()
        {
            IDictionary diagnostics = new SortedDictionary<string, object>();
            StringBuilder buffer = new StringBuilder();
            try
            {
                // Get all the diagnostic fetchers and execute them
                // to get the diagnostic data.
                foreach (string key in _diagnosticsRetrievers.Keys)
                {
                    Func<IDictionary> fetcher = _diagnosticsRetrievers[key];
                    diagnostics[key] = fetcher();
                }
            }
            catch (Exception ex)
            {
                diagnostics["Error"] = "Error ocurred attempting to get diagnostic information. " + ex.Message;
            }
            return diagnostics;
        }


        #region Private methods
        protected void Init()
        {
            // This is the full list of all diagnostic information grouped by data.
            _diagnosticsRetrievers["MachineInfo"] = new Func<IDictionary>(DiagnosticsHelper.GetMachineInfo);
            _diagnosticsRetrievers["Env_System"] = new Func<IDictionary>(DiagnosticsHelper.GetSystemEnvVariables);
            _diagnosticsRetrievers["Env_User"] = new Func<IDictionary>(DiagnosticsHelper.GetUserEnvVariables);
            _diagnosticsRetrievers["Drives"] = new Func<IDictionary>(DiagnosticsHelper.GetDrivesInfo);
            _diagnosticsRetrievers["AppDomain"] = new Func<IDictionary>(DiagnosticsHelper.GetAppDomainInfo);
            _diagnosticsRetrievers["Services"] = new Func<IDictionary>(DiagnosticsHelper.GetServices);
            _diagnosticsRetrievers["Processes"] = new Func<IDictionary>(DiagnosticsHelper.GetProcesses);
            _diagnosticsRetrievers["Modules"] = new Func<IDictionary>(DiagnosticsHelper.GetModules);
            StoreGroupsBeingProcessed();
        }


        /// <summary>
        /// Build a textual representation of all the diagnostics information.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="diagnostics"></param>
        /// <returns></returns>
        protected string BuildDiagnostics(StringBuilder buffer, IDictionary diagnostics)
        {
            foreach (string group in diagnostics.Keys)
            {
                BuildSection(buffer, diagnostics, group);
            }

            if (diagnostics.Contains("Drives"))
            {
                IDictionary drives = diagnostics["Drives"] as IDictionary;
                foreach (object drive in drives.Keys)
                {
                    BuildSection(buffer, drives, drive);
                }
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Builds a "INI" formatted represention of the diagnostic information
        /// for the group specified.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="diagnostics"></param>
        /// <param name="sectionName"></param>
        protected static void BuildSection(StringBuilder buffer, IDictionary diagnostics, object sectionName)
        {
            IDictionary section = diagnostics[sectionName] as IDictionary;
            buffer.Append("[" + sectionName + "]" + Environment.NewLine);
            BuildProperties(buffer, section);
            buffer.Append(Environment.NewLine);
            buffer.Append(Environment.NewLine);
        }


        /// <summary>
        /// Recursively builds an ini formatted representation of all the diagnostic 
        /// information.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="diagnostics"></param>
        /// <returns></returns>
        protected static void BuildProperties(StringBuilder buffer, IDictionary diagnostics)
        {
            foreach (object key in diagnostics.Keys)
            {
                object val = diagnostics[key];
                buffer.Append(key + " : " + val + Environment.NewLine);
            }
        }


        private void StoreGroupsBeingProcessed()
        {
            List<string> groups = new List<string>();

            // Store the group names of diagnostic information.
            foreach (string key in _diagnosticsRetrievers.Keys)
            {
                groups.Add(key);
                _diagnosticGroupsAllMap[key] = key;
            }

            _diagnostricGroups = new ReadOnlyCollection<string>(groups);
            _diagnostricGroupsAll = new ReadOnlyCollection<string>(groups);
        }
        #endregion
    }
}
