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
using System.Data;
using System.Globalization;
using System.Text;

using System.Xml;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;



namespace ComLib.Database
{    
    /// <summary>
    /// TO_DO:
    /// Class containing various methods for discovering various objects in a database.
    /// eg.
    /// 1. Find tables
    /// 2. Find schemas
    /// 3. Find stored procedures
    /// 4. Find Views
    /// </summary>
    public class DBSchema : DBHelper
    {
        /// <summary>
        /// Default construction
        /// </summary>
        public DBSchema() { }


        /// <summary>
        /// Initialize using connection.
        /// </summary>
        /// <param name="connection"></param>
        public DBSchema(ConnectionInfo connection)
            : base(connection)
        {
        }


        /// <summary>
        /// Drop the table from the database.
        /// </summary>
        /// <param name="tableName"></param>
        public void DropTable(string tableName)
        {
            // IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
            // DROP TABLE [dbo].[Categories]
            string checkDelete = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND type in (N'U'))";
            checkDelete += Environment.NewLine + "DROP TABLE [dbo].[" + tableName + "]";
            this.ExecuteNonQuery(checkDelete, CommandType.Text, null);
        }
       

        public void CreateTable(DataTable table)
        {
        }


        public void GetTables()
        {
            WriteSchema("all", "", null);
            WriteSchema("tables", "Tables", null);
            WriteSchema("columns", "Columns", null);
            WriteSchema("columns_cat", "Columns", new string[] { "WorkshopsProd2", null, "wk_Categories", null });
            WriteSchema("restrictions", "Restrictions",null);
        }



        private void WriteSchema(string fileName, string schemaName, string[] restrictionValues)
        {
            DataTable table = null;
            using (DbConnection con = base.GetConnection())
            {
                con.Open();
                if (!string.IsNullOrEmpty(schemaName))
                    table = con.GetSchema(schemaName, restrictionValues);
                else
                    table = con.GetSchema();
            }
            string schemaFileName = string.IsNullOrEmpty(schemaName) ? "schema" : schemaName;
            table.WriteXml("f:/tests/" + fileName + ".xml");
                        
            StringBuilder buffer = new StringBuilder();
            foreach (DataColumn col in table.Columns)
            {
                buffer.Append("Column: name = " + col.ColumnName + " type = " + col.DataType.ToString() + Environment.NewLine);
            }
            for (int ndx = 0; ndx < table.Rows.Count; ndx++)
            {
                buffer.Append("Record: " + ndx + " ");
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    buffer.Append(table.Rows[ndx][col].ToString() + ", ");
                }
                buffer.Append(Environment.NewLine);
            }
            string info = buffer.ToString();
            System.IO.File.WriteAllText("f:/tests/" + fileName + "_data.xml", info);
                      
        }
    }
}
