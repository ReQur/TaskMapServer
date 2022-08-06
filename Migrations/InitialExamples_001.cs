using FluentMigrator;


namespace dotnetserver
{
    [Migration(001)]
    public class InitialTables_001 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Dowm.001.InitialExample.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.001.InitialExample.sql");
        }
        
    }
}