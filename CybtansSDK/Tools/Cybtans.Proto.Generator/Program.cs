using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cybtans.Proto.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generators = new IGenerator[]
            {
                new ProjectsGenerator(),
                new ProtoGenerator(),
                new MessageGenerator()
            };

            List<CybtansConfig> configs = null;
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Generate code from config files: cybtans-cli [base path]");
                Console.WriteLine("Other options:");
                foreach (var item in generators)
                {
                    Console.WriteLine();
                    item.PrintHelp();
                }
                return;
            }
            else if (args.Length == 1)
            {
                configs = CybtansConfig.SearchConfigs(args[0]).ToList();
                if (configs.Count == 0)
                {
                    Console.WriteLine("No \"cybtans.json\" config files were found");
                    return;
                }
            }

            try
            {
                if (configs != null && configs.Any())
                {
                    foreach (var config in configs)
                    {
                        foreach (var step in config.Steps)
                        {
                            foreach (var item in generators)
                            {
                                if (item.Generate(config, step))
                                    break;
                            }
                        }
                    }

                    return;
                }


                foreach (var item in generators)
                {
                    if (item.CanGenerate(args[0]) && item.Generate(args))
                    {
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                var ex = e;
                while (ex != null)
                {
                    Console.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }
                Console.ResetColor();
            }

        }
        
       
    }
}
