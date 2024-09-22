using BookCatalog.API.Data;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API
{
    public class MigrationService
    {
        public static void InitializeMigration(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            serviceScope.ServiceProvider.GetService<BookContext>()!.Database.Migrate();
        }
    }
}
