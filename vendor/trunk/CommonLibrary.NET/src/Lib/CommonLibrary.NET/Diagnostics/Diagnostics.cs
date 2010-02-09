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
    /// Obtains diagnostic information related to the Machine, Currently executing 
    /// process, among other areas.
    /// </summary>
    public class Diagnostics
    {
        private static Func<IDiagnosticsService> _serviceCreator;

        
        /// <summary>
        /// Default initialization.
        /// </summary>
        static Diagnostics()
        {
            _serviceCreator = () => new DiagnosticsService();
        }


        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="service"></param>
        public static void Init(Func<IDiagnosticsService> serviceCreator)
        {
            _serviceCreator = serviceCreator;
        }


        /// <summary>
        /// Get all the diagnostic information.
        /// </summary>
        public static string GetAllInfo()
        {
            return GetInfo(string.Empty);
        }


        /// <summary>
        /// Get all the information associated with the specified groups.
        /// </summary>
        /// <param name="commaDelimitedGroups"></param>
        /// <returns></returns>
        public static string GetInfo(string commaDelimitedGroups)
        {
            IDiagnosticsService svc = _serviceCreator();
            if (string.IsNullOrEmpty(commaDelimitedGroups))
            {
                svc.FilterOn(commaDelimitedGroups, true);
            }
            string data = svc.GetDataTextual();
            return data;
        }


        /// <summary>
        /// Write all diagnostic information to the file specified.
        /// </summary>
        /// <param name="path"></param>
        public static void WriteAllInfo(string path)
        {
            WriteInfo(string.Empty, path, string.Empty);
        }


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="commaDelimitedGroups">"Machine,AppDomain"</param>
        /// <param name="path">Path of file to write information to.</param>
        public static void WriteInfo(string commaDelimitedGroups, string path)
        {
            WriteInfo(commaDelimitedGroups, path, string.Empty);
        }


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="commaDelimitedGroups">"Machine,AppDomain"</param>
        /// <param name="path">Path of file to write information to.</param>
        public static void WriteInfo(string commaDelimitedGroups, string path, string referenceMessage)
        {
            IDiagnosticsService svc = _serviceCreator();
            svc.WriteInfo(commaDelimitedGroups, path, referenceMessage);
        }
    }
}
