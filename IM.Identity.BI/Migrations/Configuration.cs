namespace IM.Identity.BI.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Edm.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Edm.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
