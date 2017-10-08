using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Refaction.Data
{
    public interface IDbContext
    {
        //////////////////////////////////////////////////////////////////////////
        // Copied SaveChanges() method signatures from DbContext class metadata //
        //////////////////////////////////////////////////////////////////////////
        
        //
        // Summary:
        //     Saves all changes made in this context to the underlying database.
        //
        // Returns:
        //     The number of state entries written to the underlying database. This can include
        //     state entries for entities and/or relationships. Relationship state entries are
        //     created for many-to-many relationships and relationships where there is no foreign
        //     key property included in the entity class (often referred to as independent associations).
        //
        // Exceptions:
        //   T:System.Data.Entity.Infrastructure.DbUpdateException:
        //     An error occurred sending updates to the database.
        //
        //   T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException:
        //     A database command did not affect the expected number of rows. This usually indicates
        //     an optimistic concurrency violation; that is, a row has been changed in the database
        //     since it was queried.
        //
        //   T:System.Data.Entity.Validation.DbEntityValidationException:
        //     The save was aborted because validation of entity property values failed.
        //
        //   T:System.NotSupportedException:
        //     An attempt was made to use unsupported behavior such as executing multiple asynchronous
        //     commands concurrently on the same context instance.
        //
        //   T:System.ObjectDisposedException:
        //     The context or connection have been disposed.
        //
        //   T:System.InvalidOperationException:
        //     Some error occurred attempting to process entities in the context either before
        //     or after sending commands to the database.

        int SaveChanges();
        //
        // Summary:
        //     Asynchronously saves all changes made in this context to the underlying database.
        //
        // Returns:
        //     A task that represents the asynchronous save operation. The task result contains
        //     the number of state entries written to the underlying database. This can include
        //     state entries for entities and/or relationships. Relationship state entries are
        //     created for many-to-many relationships and relationships where there is no foreign
        //     key property included in the entity class (often referred to as independent associations).
        //
        // Exceptions:
        //   T:System.Data.Entity.Infrastructure.DbUpdateException:
        //     An error occurred sending updates to the database.
        //
        //   T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException:
        //     A database command did not affect the expected number of rows. This usually indicates
        //     an optimistic concurrency violation; that is, a row has been changed in the database
        //     since it was queried.
        //
        //   T:System.Data.Entity.Validation.DbEntityValidationException:
        //     The save was aborted because validation of entity property values failed.
        //
        //   T:System.NotSupportedException:
        //     An attempt was made to use unsupported behavior such as executing multiple asynchronous
        //     commands concurrently on the same context instance.
        //
        //   T:System.ObjectDisposedException:
        //     The context or connection have been disposed.
        //
        //   T:System.InvalidOperationException:
        //     Some error occurred attempting to process entities in the context either before
        //     or after sending commands to the database.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.

        Task<int> SaveChangesAsync();
        //
        // Summary:
        //     Asynchronously saves all changes made in this context to the underlying database.
        //
        // Parameters:
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // Returns:
        //     A task that represents the asynchronous save operation. The task result contains
        //     the number of state entries written to the underlying database. This can include
        //     state entries for entities and/or relationships. Relationship state entries are
        //     created for many-to-many relationships and relationships where there is no foreign
        //     key property included in the entity class (often referred to as independent associations).
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     Thrown if the context has been disposed.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
