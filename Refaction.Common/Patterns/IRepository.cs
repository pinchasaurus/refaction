using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Common.Patterns
{
    /// <summary>
    /// Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects.
    /// </summary>
    /// <remarks>Per https://martinfowler.com/eaaCatalog/repository.html</remarks>
    /// <typeparam name="TModel">The type of the model class for the repository</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the model class</typeparam>
    public interface IRepository<TModel, TKey>
    {
        /// <summary>
        /// Creates a single model in the underlying datasource
        /// </summary>
        void Create(TModel model);


        /// <summary>
        /// Retrieves all models from the underlying datasource
        /// </summary>
        IEnumerable<TModel> Retrieve();

        /// <summary>
        /// Retrieves matching models from the underlying datasource
        /// </summary>
        TModel Retrieve(TKey id);


        /// <summary>
        /// Updates a single model in the underlying datasource
        /// </summary>
        void Update(TModel model);


        /// <summary>
        /// Deletes the specified model in the underlying datasource
        /// </summary>
        void Delete(TModel model);

        /// <summary>
        /// Deletes the matching model in the underlying datasource
        /// Throws an exception if the predicate matches more than one model
        /// </summary>
        void Delete(TKey id);


        /// <summary>
        /// Returns true if a matching model exists in the underlying datasource; otherwise, returns false
        /// </summary>
        bool Exists(TKey id);
    }
}
