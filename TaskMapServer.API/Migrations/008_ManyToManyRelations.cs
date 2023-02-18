using FluentMigrator;


namespace dotnetserver
{
    [Migration(008)]
    public class ManyToManyRelations_008 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.008.ManyToManyRelations.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.008.ManyToManyRelations.sql");
        }
        
    }
}