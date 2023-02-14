using FluentMigrator;


namespace dotnetserver
{
    [Migration(000)]
    public class InitialTables_000 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.000.InitialTables.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.000.InitialTables.sql");
        }
        
    }
}