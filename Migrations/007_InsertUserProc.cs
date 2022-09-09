using FluentMigrator;


namespace dotnetserver
{
    [Migration(007)]
    public class InsertUserTrigger_007 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.007.InsertUserProc.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.007.InsertUserProc.sql");
        }
        
    }
}