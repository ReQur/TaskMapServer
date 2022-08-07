using FluentMigrator;


namespace dotnetserver
{
    [Migration(004)]
    public class CreateUpdateUserLastBoardTrg_004 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.004.CreateUpdateUserLastBoardTrg.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.004.CreateUpdateUserLastBoardTrg.sql");
        }
        
    }
}