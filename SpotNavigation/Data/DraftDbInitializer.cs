using System;

namespace SpotNavigation.Data
{
    public class DraftDbInitializer
    {
        
        public static void Initialize(DraftDbContext context)
        {
            // Delete the database before we initialize it.
            // This is common to do during development.
            context.Database.EnsureDeleted();

            // Make sure the database and tables exist
            context.Database.EnsureCreated();
            // Test data is typically added here

            // Finally save changes to the database
            Console.WriteLine("DB Initialized");
            context.SaveChanges();
        }
    }
}