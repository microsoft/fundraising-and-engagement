using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FundraisingandEngagement.Data
{
    public static class DbContextExtensions
    {
        public static async Task SaveChangesAsync(this IPaymentContext context, TimeSpan timeout, CancellationToken token = default)
        {
            if (context is DbContext dbContext)
            {
                var tempTimeout = dbContext.Database.GetCommandTimeout();
                try
                {
                    dbContext.Database.SetCommandTimeout(timeout);
                    await dbContext.SaveChangesAsync(token);
                }
                finally
                {
                    dbContext.Database.SetCommandTimeout(tempTimeout);
                }
            }
            else
            {
                await context.SaveChangesAsync(token);
            }
        }

        public static void DisableAutoDetectChanges(this IPaymentContext context, bool disable = true)
        {
            // See https://blog.oneunicorn.com/2012/03/12/secrets-of-detectchanges-part-3-switching-off-automatic-detectchanges/
            if (context is DbContext dbContext)
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = !disable;
            }
        }
    }
}