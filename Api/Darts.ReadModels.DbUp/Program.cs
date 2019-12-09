using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace Darts.ReadModels.DbUp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString =
            args.FirstOrDefault()
        ?? "Server=.;Database=darts; Integrated Security=true";

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .JournalToSqlTable("dbo", "Journal")
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
