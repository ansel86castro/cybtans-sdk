using System;

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
            

            if(args == null || args.Length == 0)
            {
                foreach (var item in generators)
                {
                    Console.WriteLine();
                    item.PrintHelp();
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
       
    }
}
