using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LDTSolutions.Common.EntityFrameworkCore.Transaction
{
   public class ResilientTransaction
   {
      private DbContext _context;

      private ResilientTransaction(DbContext context) =>
          _context = context ?? throw new ArgumentNullException(nameof(context));

      public static ResilientTransaction New(DbContext context) =>
          new ResilientTransaction(context);

      #region Execution Methods

      // Use of an EF Core resiliency strategy when using single/multiple DbContexts
      // within an explicit BeginTransaction():
      // https://docs.microsoft.com/ef/core/miscellaneous/connection-resiliency

      /// <summary>
      /// Execute transaction
      /// </summary>
      /// <param name="prerequisiteAction">action run before start transaction</param>
      /// <param name="beforeSaveAction">action run before DbContext execute SaveChanges method</param>
      /// <param name="afterSaveAction">action run after DbContext execute SaveChanges method</param>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      public async Task ExecuteAsync(Func<Task> prerequisiteAction = null, Func<Task> beforeSaveAction = null, Func<Task> afterSaveAction = null, CancellationToken cancellationToken = default)
      {
         if (prerequisiteAction != null)
            await prerequisiteAction();

         var strategy = _context.Database.CreateExecutionStrategy();

         await strategy.ExecuteAsync(operation: async (ctoken) =>
         {
            using (var transaction = _context.Database.BeginTransaction())
            {
               if (beforeSaveAction != null)
                  await beforeSaveAction();

               await _context.SaveChangesAsync(cancellationToken: cancellationToken);

               if (afterSaveAction != null)
                  await afterSaveAction();

               transaction.Commit();
            }
         }, cancellationToken: cancellationToken);
      }

      /// <summary>
      /// Execute transaction and retry until succeeded ("verifySucceeded" function return "true")
      /// </summary>
      /// <param name="verifySucceeded">validate function check every single time a transaction is complete</param>
      /// <param name="prerequisiteAction">action run before start transaction</param>
      /// <param name="beforeSaveAction">action run before DbContext execute SaveChanges method</param>
      /// <param name="afterSaveAction">action run after DbContext execute SaveChanges method</param>
      /// <param name="cancellationToken"></param>
      /// <returns></returns>
      public async Task ExecuteAsync(Func<Task<bool>> verifySucceeded, Func<Task> prerequisiteAction = null, Func<Task> beforeSaveAction = null, Func<Task> afterSaveAction = null, CancellationToken cancellationToken = default)
      {
         if (prerequisiteAction != null)
            await prerequisiteAction();

         var strategy = _context.Database.CreateExecutionStrategy();

         await strategy.ExecuteInTransactionAsync(_context,
            operation: async (context, ctoken) =>
            {
               if (beforeSaveAction != null)
                  await beforeSaveAction();

               await _context.SaveChangesAsync(acceptAllChangesOnSuccess: false, cancellationToken: cancellationToken);

               if (afterSaveAction != null)
                  await afterSaveAction();
            },
            verifySucceeded: async (context, ctoken) => await verifySucceeded(),
            cancellationToken: cancellationToken);

         _context.ChangeTracker.AcceptAllChanges();
      }

      #endregion
   }
}
