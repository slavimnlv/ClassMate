using ClassMate.Data;

namespace ClassMate.API.ExtensionMethods
{
    public static class ServiceProviderExtensions
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ClassMateDbContext>();
                dbContext.Database.EnsureCreated(); // Or dbContext.Database.Migrate() for more complex migrations
            }
        }
    }
}
