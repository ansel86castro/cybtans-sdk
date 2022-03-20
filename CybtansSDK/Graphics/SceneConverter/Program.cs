using Cybtans.Graphics;
using Cybtans.Graphics.Importers;
using Cybtans.Graphics.Shading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;

namespace SceneConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args == null || args.Length == 0)
            {
                Console.WriteLine("arguments : path");
                return;
            }

            string path = null;
            string destination = null;

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var value = arg;
                if (arg.StartsWith("-"))
                {
                    i++;
                    if (i >= args.Length)
                    {
                        Console.WriteLine("Invalid options");
                        PrintHelp();

                        return;
                    }

                    value = args[i];
                }

                switch (arg)
                {
                    case "-path":
                    case "-p":
                        path = value;
                        break;
                    case "-destination":
                    case "-d":
                        destination = value;
                        break;
                }
            }

            if(path !=null)
            {
                var di = new DirectoryInfo(path);
                ConvertScenesFromDirectory(di, destination);

                ConvertEffectsFromDirectory(path, destination);
            }

        }

        private static void ConvertEffectsFromDirectory(string path, string destination)
        {
            EffectsManager effectManager = new EffectsManager();
            effectManager.LoadEffectsFromDirectory(path).Wait();

            var dto = effectManager.ToDto();
            var json = JsonConvert.SerializeObject(dto);
            var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            });

            var filename = Path.Combine(destination?? path, "effects.json");
            File.WriteAllBytes(filename, bytes);
        }

        public static void ConvertScenesFromDirectory(DirectoryInfo di, string destination)
        {
            foreach (var file in di.EnumerateFiles("*.dae"))
            {                
                if (file.Extension == ".dae")
                {
                    ConvertDae(file, destination);
                }
            }

            foreach (var dir in di.EnumerateDirectories())
            {
                ConvertScenesFromDirectory(dir, destination);
            }
        }

        private static void ConvertDae(FileInfo fi, string destination)
        {
            var name = Path.GetFileNameWithoutExtension(fi.Name);
            Scene scene = new Scene(name);
            ContentImporter.Import(scene, fi.FullName);
            var dto = scene.ToDto();            
            var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(dto, new JsonSerializerOptions
            {
                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                  IgnoreNullValues = true                   
            });

            var filename = Path.Combine(destination??fi.DirectoryName, name + ".scene.json");
            File.WriteAllBytes(filename, bytes);
        }

        private static void PrintHelp()
        {

        }
    }
}
