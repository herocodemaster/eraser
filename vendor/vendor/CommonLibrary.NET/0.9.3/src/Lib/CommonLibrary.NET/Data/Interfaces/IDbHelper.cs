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
using System.Collections.Generic;


namespace ComLib.Database
{
    /// <summary>
    /// Interface for a DatabaseHelper
    /// </summary>
    public interface IDBHelper
    {
        /// <summary>
        /// The underlying connection to the datastore.
        /// </summary>
        ConnectionInfo Connection { get; set; }


        /// <summary>
        /// Core method which all the 
        /// 1. ExecuteScalar
        /// 2. ExecuteNonQuery
        /// 3. ExecuteReader
        /// 4. ExecuteDataTable methods call.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        /// <param name="useTransaction"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        TResult Execute<TResult>(string commandText, CommandType commandType, DbParameter[] dbParameters, bool useTransaction, Func<DbCommand, TResult> executor);


        /// <summary>
        /// Execute non-query sql.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute non-query sql.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        int ExecuteNonQueryText(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute non-query sql.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        int ExecuteNonQueryProc(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return datareader.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string commandText, CommandType commandType, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return datareader.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        IDataReader ExecuteReaderText(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return datareader.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        IDataReader ExecuteReaderProc(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return single scalar value.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        object ExecuteScalar(string commandText, CommandType commandType, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return single scalar value.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        object ExecuteScalarText(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return single scalar value.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        object ExecuteScalarProc(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return dataset.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string commandText, CommandType commandType, params DbParameter[] dbParameters);

        
        /// <summary>
        /// Execute sql and return dataset.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataSet ExecuteDataSetText(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return dataset.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataSet ExecuteDataSetProc(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return datatable
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataTable ExecuteDataTable(string commandText, CommandType commandType, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return datatable
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataTable ExecuteDataTableText(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Execute sql and return datatable
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DataTable ExecuteDataTableProc(string commandText, params DbParameter[] dbParameters);


        /// <summary>
        /// Build an input parameter.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        DbParameter BuildInParam(string paramName, DbType dbType, object val);


        /// <summary>
        /// Build an input parameter.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        DbParameter BuildOutParam(string paramName, DbType dbType);
        

        /// <summary>
        /// Get a connection to the appropriate database.
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <returns></returns>
        DbConnection GetConnection();


        /// <summary>
        /// Create a new dbcommand using the connection.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commmandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        DbCommand GetCommand(DbConnection con, string commmandText, CommandType commandType);
    }
}
