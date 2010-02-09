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
using System.Text;
using ComLib;
using ComLib.Locale;



namespace ComLib.Errors
{
    /// <summary>
    /// Interface for an exception manager.
    /// </summary>
    public interface IErrorManager
    {
        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Handle(object error, Exception exception);
        
        
        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        void Handle(object error, Exception exception, object[] arguments);
        

        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errorResults">The error results.</param>
        void Handle(object error, Exception exception, IStatusResults errorResults);


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errorResults">The error results.</param>
        /// <param name="arguments">The arguments.</param>
        void Handle(object error, Exception exception, IStatusResults errorResults, object[] arguments);
    }



    /// <summary>
    /// Localization based
    /// </summary>
    public interface ILocalizedExceptionManager : IErrorManager
    {
        /// <summary>
        /// Handles the exception by getting the error description from the <paramref name="resources"/> using
        /// the key specified by <paramref name="errorDescriptorKey"/>. Converts all the <paramref name="args"/>
        /// to a string to put into the error.
        /// </summary>
        /// <param name="errorDescriptorKey">The name of key to use to get the localized errors from resources. </param>
        /// <param name="resources">The localized resources that contains the error string.</param>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="args">Array of strings to report in the error.</param>
        void Handle(string errorDescriptorKey, ILocalizationResourceProvider resources, Exception ex, string[] args);


        /// <summary>
        /// Handles the exception by getting the error description from the <paramref name="resources"/> using
        /// the key specified by <paramref name="errorDescriptorKey"/>. Adds the error to <paramref name="errors"/>.
        /// </summary>
        /// <param name="errorDescriptor">The name of key to use to get the localized errors from resources. </param>
        /// <param name="resources">The localized resources that contains the error string.</param>
        /// <param name="errors">The list of errors to add to the error string to.</param>
        /// <param name="ex">The exception to handle.</param>
        void Handle(string errorDescriptorKey, ILocalizationResourceProvider resources, IStatusResults errors, Exception ex);
        

        /// <summary>
        /// Handles the exception by getting the error description from the <paramref name="resources"/> using
        /// the key specified by <paramref name="errorDescriptorKey"/>. Adds the error to <paramref name="errors"/>.
        /// </summary>
        /// <param name="errorDescriptor">The name of key to use to get the localized errors from resources. </param>
        /// <param name="resources">The localized resources that contains the error string.</param>
        /// <param name="errors">The list of errors to add to the error string to.</param>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="args">Array of strings to report in the error.</param>
        void Handle(string errorDescriptorKey, ILocalizationResourceProvider resources, IStatusResults errors, Exception ex, string[] args);
    }
}
