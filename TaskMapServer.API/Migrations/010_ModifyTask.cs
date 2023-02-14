using FluentMigrator;


namespace dotnetserver
{
    [Migration(010)]
    public class ModifyTask_010 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.010.ModifyTask.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.010.ModifyTask.sql");
        }
        
    }
}