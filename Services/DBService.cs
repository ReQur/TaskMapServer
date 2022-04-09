using System;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;


namespace dotnetserver
{
   public class DBService
   {
       private static MySqlConnection conn;
       public static void Init()
       {
           string connStr = "server=localhost;user=root;database=TaskMap;password=rootPassword;";
           conn = new MySqlConnection(connStr);
           conn.Open();
       }

       public static async Task<string> Query(string sql)
       {
           MySqlCommand command = new MySqlCommand(sql, conn);
           var res = command.ExecuteScalarAsync();
           //var res = await command.ExecuteReaderAsync();
           //await Task.Run(() =>
           //{
           //    while (res.Read())
           //    {
           //        Console.WriteLine(res.GetTextReader(0).ReadToEnd());
           //        //Console.WriteLine(res[0].ToString() + " " + res[1].ToString());
           //    }
           //});
           var resJson =  JsonConvert.SerializeObject(res.Result);
           //await res.CloseAsync();
           return resJson;
       }
       
    }
}