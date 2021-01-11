using System;
using System.Linq;
using DapperMicrosoftDataSqlite;
using MicrosoftDataSqlite;
using Shared;
using SystemDataSqlite;

namespace Sqlite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var users = DataGenerator.GenerateUsers(20);

            ISqliteExecuter executer;

            //executer = new MicrosoftDataSqliteExecuter();
            //executer = new SystemDataSqliteExecuter();
            executer = new DapperMicrosoftDataSqliteExecuter();

            executer.Initilize();
            executer.CreateTables();
            executer.InsertEntries(users);
            var readUsers = executer.ReadEntries();

            foreach (var readUser in readUsers)
            {
                Console.WriteLine($"{readUser.Name} | {readUser.Address.City} ({readUser.Address.Id})");
            }
        }
    }
}
