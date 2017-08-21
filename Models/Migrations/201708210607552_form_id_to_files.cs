namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class form_id_to_files : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "FormId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "FormId");
        }
    }
}
