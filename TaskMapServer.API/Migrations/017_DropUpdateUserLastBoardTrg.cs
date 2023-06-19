using FluentMigrator;


namespace dotnetserver
{
    [Migration(017)]
    public class DropUpdateUserLastBoardTrg_017 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.017.DropUpdateUserLastBoardTrg.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.017.DropUpdateUserLastBoardTrg.sql");
        }
        
    }
}