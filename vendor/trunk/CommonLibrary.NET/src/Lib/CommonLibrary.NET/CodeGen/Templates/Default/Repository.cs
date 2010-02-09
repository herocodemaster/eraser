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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using ComLib.Entities;
using ComLib.Database;
using ComLib.LocationSupport;


namespace <%= model.NameSpace %>
{
    /// <summary>
    /// Generic repository for persisting <%= model.Name %>.
    /// </summary>
    public partial class <%= model.Name %>Repository : Repository<<%= model.Name %>> //EntityRepositoryInMemory<<%= model.Name %>>  //
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public <%= model.Name %>Repository() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public <%= model.Name %>Repository(ConnectionInfo connectionInfo, IDBHelper helper)
            : base(connectionInfo, helper)
        {
        }
    }


    
    /// <summary>
    /// RowMapper for <%= model.Name %>.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class <%= model.Name %>RowMapper : EntityRowMapper<<%= model.Name %>>, IEntityRowMapper<<%= model.Name %>>
    {
        public override <%= model.Name %> MapRow(IDataReader reader, int rowNumber)
        {
            <%= model.Name %> entity = <%= model.Name %>s.New();
            <%= model.RowMappingCode %>
            return entity;
        }
    }
}