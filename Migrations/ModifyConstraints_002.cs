using FluentMigrator;


namespace dotnetserver
{
    [Migration(002)]
    public class ModifyConstraints_002 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.002.ModifyConstraints.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.002.ModifyConstraints.sql");
        }
        
    }
}