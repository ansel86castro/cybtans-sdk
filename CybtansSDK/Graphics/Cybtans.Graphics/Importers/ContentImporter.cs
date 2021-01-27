using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Cybtans.Graphics.Importers
{
    public abstract class ContentImporter
    {
        static Dictionary<string, Type> _importerTypes = new Dictionary<string, Type>();

        private bool _disposed;

        public string FileName { get; set; }

        public void LoadFile(Scene scene, string srcfilename)
        {
            FileName = srcfilename;
            ImportFile(scene, srcfilename);
        }

        protected abstract void ImportFile(Scene scene, string filename);


        public static void RegisterImporter(string extension, Type importerType)
        {
            _importerTypes.Add(extension, importerType);
        }

        public static void RemoveImporter(string extension)
        {
            _importerTypes.Remove(extension);
        }

        public static void InitializeDefaultLoaders()
        {
            Module mod = typeof(ContentImporter).Module;
            foreach (var type in mod.GetTypes())
            {
                var attr = (ImportFormatAttribute[])type.GetCustomAttributes(typeof(ImportFormatAttribute), true);
                if (attr.Length > 0)
                {
                    _importerTypes.Add(attr[0].FileExtension, type);
                }
            }
        }

        public static ContentImporter GetImporter(string extension)
        {
            if (_importerTypes.Count == 0)
                InitializeDefaultLoaders();

            var importerType = _importerTypes[extension.ToLower()];
            return (ContentImporter)Activator.CreateInstance(importerType);
        }

        public static IAnimationImporter GetAnimationImporter(string extension)
        {
            if (_importerTypes.Count == 0)
                InitializeDefaultLoaders();

            var importerType = _importerTypes[extension.ToLower()];
            return (IAnimationImporter)Activator.CreateInstance(importerType);
        }

        public static void Import(Scene scene, string filename)
        {
            var importer = GetImporter(Path.GetExtension(filename));
            importer.LoadFile(scene, filename);
        }

        public static void ImportAnimation(Scene scene, string filename)
        {
            var importer = GetAnimationImporter(Path.GetExtension(filename));
            if (importer == null)
                return;

            importer.ImportAnimation(scene, filename);
        }

        public static void ImportAnimation(Scene scene, string filename, Frame root, string fileRootName = null)
        {
            var importer = GetImporter(Path.GetExtension(filename)) as IAnimationImporter;
            if (importer == null)
                return;
            importer.ImportAnimation(scene, filename, root, fileRootName);
        }

    }

}
