using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;


namespace dotnetserver
{
    public class RandomAlphaNumericString
    {
        private static Random random = new Random();
        public RandomAlphaNumericString() { }
        public static string Generate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}