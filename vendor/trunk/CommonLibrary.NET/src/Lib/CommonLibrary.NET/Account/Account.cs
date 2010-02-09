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
using System.Linq;
using System.Text;

using ComLib.Entities;


namespace ComLib.Membership
{
    /// <summary>
    /// Account entity.
    /// </summary>
    public partial class Account : DomainObject<Account>
    {
		/// <summary>
		/// Get/Set SecurityQuestion
		/// </summary>
		public string SecurityQuestion { get; set; }


		/// <summary>
		/// Get/Set SecurityAnswer
		/// </summary>
		public string SecurityAnswer { get; set; }


		/// <summary>
		/// Get/Set IsApproved
		/// </summary>
		public bool IsApproved { get; set; }


		/// <summary>
		/// Get/Set IsLockedOut
		/// </summary>
		public bool IsLockedOut { get; set; }


		/// <summary>
		/// Get/Set Comment
		/// </summary>
		public string Comment { get; set; }


		/// <summary>
		/// Get/Set LockOutReason
		/// </summary>
		public string LockOutReason { get; set; }


		/// <summary>
		/// Get/Set LastLoginDate
		/// </summary>
		public DateTime LastLoginDate { get; set; }


		/// <summary>
		/// Get/Set LastPasswordChangedDate
		/// </summary>
		public DateTime LastPasswordChangedDate { get; set; }


		/// <summary>
		/// Get/Set LastPasswordResetDate
		/// </summary>
		public DateTime LastPasswordResetDate { get; set; }


		/// <summary>
		/// Get/Set LastLockOutDate
		/// </summary>
		public DateTime LastLockOutDate { get; set; }



    }
}
