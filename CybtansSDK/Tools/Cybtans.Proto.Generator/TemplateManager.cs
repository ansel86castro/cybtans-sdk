using Cybtans.Proto.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cybtans.Proto.Generator
{
    public class TemplateManager
    {
        public static string GetRawTemplate(string template)
        {
            using var stream = typeof(Program).Assembly.GetManifestResourceStream($"Cybtans.Proto.Generator.Templates.{template}");
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            return content;
        }

        public static string GetTemplate(string template, object args = null)
        {
            using var stream = typeof(Program).Assembly.GetManifestResourceStream($"Cybtans.Proto.Generator.Templates.{template}");
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            if (args == null)
                return content;

            return TemplateProcessor.Process(content, args);
        }
    }
}
