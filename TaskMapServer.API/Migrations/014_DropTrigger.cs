using FluentMigrator;


namespace dotnetserver
{
    [Migration(014)]
    public class DropTrigger_014 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.014.DropTrigger.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.014.DropTrigger.sql");
        }
        
    }
}