using System;
using System.Diagnostics;
using DapperMicrosoftDataSqlite;
using MicrosoftDataSqlite;
using MicrosoftEntitiyFrameworkCoreSqlite;
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
            //executer = new DapperMicrosoftDataSqliteExecuter();
            executer = new MicrosoftEntityFrameworkCoreSqliteExecuter();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            executer.Initilize();
            Console.WriteLine($"Initialize took {stopWatch.Elapsed}");
            stopWatch.Restart();

            executer.CreateTables();
            Console.WriteLine($"CreateTables took {stopWatch.Elapsed}");
            stopWatch.Restart();

            executer.InsertEntries(users);
            Console.WriteLine($"InsertEntries took {stopWatch.Elapsed}");
            stopWatch.Restart(); 

            var readUsers = executer.ReadEntries();
            Console.WriteLine($"ReadEntries took {stopWatch.Elapsed}");
            stopWatch.Restart(); 
            
            executer.Dispose();
            stopWatch.Stop();
            Console.WriteLine($"Dispose took {stopWatch.Elapsed}");

            foreach (var readUser in readUsers)
            {
                Console.WriteLine($"{readUser.Name} | {readUser.Address.City} ({readUser.Address.Id})");
            }
        }
    }
}
