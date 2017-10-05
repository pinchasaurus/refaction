using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

namespace Refaction.Common.Patterns
{
    /// <summary>
    /// Abstract base class for all repositories
    /// Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects.
    /// </summary>
    /// <remarks>Per https://martinfowler.com/eaaCatalog/repository.html</remarks>
    /// <typeparam name="TModel">The type of the model class for the repository</typeparam>
    /// <typeparam name="TKey">The type of the primary key for the model class</typeparam>
    public abstract class RepositoryBase<TModel, TKey> : IRepository<TModel, TKey>, IDisposable
    {
        /// <summary>
        /// Creates a single model in the underlying datasource
        /// </summary>
        public abstract void Create(TModel model);



        /// <summary>
        /// Retrieves all models from the underlying datasource
        /// </summary>
        public abstract IEnumerable<TModel> Retrieve();

        /// <summary>
        /// Retrieves matching models from the underlying datasource
        /// </summary>
        public abstract TModel Retrieve(TKey id);



        /// <summary>
        /// Updates a single model in the underlying datasource
        /// </summary>
        public abstract void Update(TModel model);



        /// <summary>
        /// Deletes the specified model in the underlying datasource
        /// </summary>
        public abstract void Delete(TModel model);

        /// <summary>
        /// Deletes the matching model in the underlying datasource
        /// Throws an exception if the predicate matches more than one model
        /// </summary>
        public abstract void Delete(TKey id);



        /// <summary>
        /// Returns true if a matching model exists in the underlying datasource, otherwise returns false
        /// </summary>
        public abstract bool Exists(TKey id);



        /// <summary>
        /// Allows inherited classes to override Dispose method
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // do nothing, allow derived classes to override dispose method
        }

        ~RepositoryBase()
        {
            Dispose(false);
        }
    }
}
