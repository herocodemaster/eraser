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
using System.Web;
using System.Security.Principal;
using System.Web.Security;




namespace ComLib.Authentication
{
    /// <summary>
    /// Class to represnt a simple security service for 
    /// DESKTOP based windows applications where there is just 1 user.
    /// </summary>
    public class AuthWin : IAuth
    {
        private string _adminRoleName = "Administrators";
        private IPrincipal _user = UserPrincipal.Empty;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public AuthWin() 
        {
            UserPrincipal user = new UserPrincipal(-1, Environment.UserName, string.Empty, 
                System.Security.Principal.WindowsIdentity.GetCurrent());
            _user = user;
        }


        /// <summary>
        /// Initialize with the admin role name.
        /// </summary>
        /// <param name="adminRoleName"></param>
        public AuthWin(string adminRoleName, IPrincipal user)
        {
            _adminRoleName = adminRoleName;
            _user = user;
        }


        /// <summary>
        /// Get the current user.
        /// </summary>
        public IPrincipal User
        {
            get { return _user; }
        }


        /// <summary>
        /// The name of the current user.
        /// </summary>
        public string UserName
        {
            get { return _user.Identity.Name; }
        }


        /// <summary>
        /// Provides just the username if the username contains
        /// the domain.
        /// e.g. returns "john" if username is "mydomain\john"
        /// </summary>
        public string UserShortName
        {
            get
            {
                string fullName = _user.Identity.Name;
                int ndxSlash = fullName.LastIndexOf(@"\");
                if (ndxSlash == -1)
                    ndxSlash = fullName.LastIndexOf("/");

                if (ndxSlash == -1)
                    return fullName;

                return fullName.Substring(ndxSlash + 1);
            }
        }


        /// <summary>
        /// Get the user data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userName"></param>
        /// <returns></returns>
        public T GetUser<T>(string userName) where T : class, IPrincipal
        {
            return _user as T;
        }


        /// <summary>
        /// Determine if the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _user.Identity.IsAuthenticated;
        }


        /// <summary>
        /// Return whether or not the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public bool IsGuest()
        {
            return !IsAuthenticated();
        }        


        /// <summary>
        /// Determine if currently logged in user is an administrator.
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            if (IsGuest()) return false;

            return IsUserInRoles("Administrators");
        }      


        /// <summary>
        /// Is User in the selected roles.
        /// </summary>
        /// <param name="rolesDelimited"></param>
        /// <returns></returns>
        public bool IsUserInRoles(string rolesDelimited)
        {
            return RoleHelper.IsInRoles(rolesDelimited, _user);
        }


        /// <summary>
        /// Sign the user in.
        /// </summary>
        /// <param name="user"></param>
        public void SignIn(IPrincipal user)
        {
            _user = user;
        }


        /// <summary>
        /// Sign the user in via username.
        /// </summary>
        /// <param name="user"></param>
        public void SignIn(string userName)
        {
            UserPrincipal user = new UserPrincipal(-1, userName, string.Empty,
                System.Security.Principal.WindowsIdentity.GetCurrent());
            _user = user;
        }


        /// <summary>
        /// Sign the user in via username and remember user.
        /// </summary>
        /// <param name="user">username.</param>
        /// <param name="rememberUser"></param>
        public void SignIn(string user, bool rememberUser)
        {
            // Not applicable for windows implementation.
        }


        /// <summary>
        /// Signout the user.
        /// </summary>
        public void SignOut()
        {
            _user = UserPrincipal.Empty;
        }
    }
}
