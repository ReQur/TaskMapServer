using FluentMigrator;


namespace dotnetserver
{
    [Migration(003)]
    public class AddIndex_003 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.003.AddIndex.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.003.AddIndex.sql");
        }
        
    }
}