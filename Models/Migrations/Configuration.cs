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
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true; // ������������ ���������, ���� ���� ��� ������� � ������ ������
        }

        protected override void Seed(Models.LrdrContext context)
        {
            Seeder.Seed(context);
        }
    }
}
