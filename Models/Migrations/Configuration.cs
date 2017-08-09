namespace Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.LrdrContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true; // автомиграции примен¤тс¤, даже если это приведЄт к потер¤м данных
        }

        protected override void Seed(Models.LrdrContext context)
        {
            Seeder.Seed(context);
        }
    }
}
