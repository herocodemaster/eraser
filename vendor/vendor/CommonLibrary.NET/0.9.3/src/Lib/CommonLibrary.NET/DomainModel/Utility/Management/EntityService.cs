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
using System.Collections;
using System.Reflection;

using ComLib;
using ComLib.Entities;
using ComLib.Reflection;

namespace ComLib.Entities.Management
{
    /// <summary>
    /// Context information for performing CRUD operations at the Service <see cref="ComLib.Entities.Service"/>
    /// level on any Domain Entity.
    /// </summary>
    public class EntityServiceContext : ActionContext
    {
        /// <summary>
        /// The fully-qualified name of the entity on which the action is being performed on.
        /// </summary>
        public string EntityName;


        /// <summary>
        /// List of key/value pairs corresponding the properties on the Entity.
        /// </summary>
        public IList<KeyValuePair<string, string>> PropValues;


        /// <summary>
        /// Store the id and/or entityName.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        public EntityServiceContext(string entityName, int id)
            : base(true, id)
        {
            EntityName = entityName;
        }


        /// <summary>
        /// Store the id and/or entityName.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        public EntityServiceContext(Type entityType, int id, object entity)
            : base(true, id)
        {
            EntityName = entityType.FullName;
            Item = entity;
        }
    }



    /// <summary>
    /// Class to help manage DomainEntities.
    /// by supplying CRUD operations on any DomainEntity.
    /// </summary>

    /// <summary>
    /// Generic Reference data service to provide 
    /// 1. Creation
    /// 2. Retrieve
    /// 3. Update
    /// 4. Deletion
    /// 
    /// operations to Reference data entities, such as 
    /// 1. category
    /// 2. state
    /// 3. country
    /// 4. location short names
    /// 5. cities.
    /// 
    /// </summary>
    public class EntityService
    {
        
        /// <summary>
        /// Initialize the mappings to the entities.
        /// </summary>
        public EntityService()
        {
            Settings = new EntityServiceSettings();
        }


        /// <summary>
        /// Settings for the entity service.
        /// </summary>
        public EntityServiceSettings Settings { get; set; }


        #region Public CRUD operations on the services related to the domain models.
        /// <summary>
        /// Update the specific entity with property values specified.
        /// </summary>
        /// <param name="entityName">"Category"</param>
        /// <param name="propValues">List of key value pairs.
        /// Where the key is the property name, and the value is a string 
        /// representation of the property value.</param>
        public BoolMessage Create(EntityServiceContext ctx)
        {
            object result = InvokeAction(ctx, Settings.CreateMethod);
            return result as BoolMessage;
        }


        /// <summary>
        /// Retrieves all the reference data records associated with the
        /// specified entity
        /// </summary>
        /// <param name="entity">e.g. "Category"</param>
        /// <returns>DataTable containing all entity records and all properties of the entity</returns>
        public BoolMessageItem GetAll(EntityServiceContext ctx)
        {
            object result = InvokeAction(ctx, Settings.RetrieveAllMethod);
            return result as BoolMessageItem;
        }


        /// <summary>
        /// Retrieve the entity with the specified id, given the entityType
        /// </summary>
        /// <param name="entityName">"Category"</param>
        /// <param name="id">"1" Id of a specific entity.</param>
        /// <returns></returns>
        public BoolMessageItem Get(EntityServiceContext ctx)
        {
            object result = InvokeAction(ctx, Settings.RetrieveMethod);
            return result as BoolMessageItem;
        }


        /// <summary>
        /// Update the specific entity with property values specified.
        /// </summary>
        /// <param name="entityName">"Category"</param>
        /// <param name="propValues">List of key value pairs.
        /// Where the key is the property name, and the value is a string 
        /// representation of the property value.</param>
        /// <param name="id">"2"</param>
        public BoolMessage Update(EntityServiceContext ctx)
        {
            object result = InvokeAction(ctx, Settings.UpdateMethod);
            return result as BoolMessage;
        }


        /// <summary>
        /// Delete the specific entity with the id.
        /// </summary>
        /// <param name="entityName">"Category"</param>
        /// <param name="id">"2"</param>
        public BoolMessage Delete(EntityServiceContext ctx)
        {
            object result = InvokeAction(ctx, Settings.DeleteMethod);
            return result as BoolMessage;
        }
        #endregion


        private object InvokeAction(EntityServiceContext ctx, string methodName)
        {
            // Create the appropriate type of actioncontext for the specific entity being managed.
            // And then set it's errors and messages to the calling context so the errors / messages
            // can be available to the caller.
            IActionContext actionContext = ActiveRecordRegistration.GetContext(ctx.EntityName);
            actionContext.CombineMessageErrors = true;
            actionContext.Item = ctx.Item;
            actionContext.Id = ctx.Id;
            actionContext.Errors = ctx.Errors;
            actionContext.Messages = ctx.Messages;

            object service = EntityRegistration.GetService(ctx.EntityName);            
            object result = ReflectionUtils.InvokeMethod(service, methodName, new object[] { actionContext });
            return result;
        }
    }
}
