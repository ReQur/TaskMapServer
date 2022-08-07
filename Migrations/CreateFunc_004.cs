using FluentMigrator;


namespace dotnetserver
{
    [Migration(004)]
    public class CreateFunc_004 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.004.CreateFunc.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.004.CreateFunc.sql");
        }
        
    }
}