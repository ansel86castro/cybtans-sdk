using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Proto.Generators
{    
    public class TemplateProcessor
    {

        IDictionary<string, object> symbols = new Dictionary<string, object>();

        public string Template { get; set; }

        public TemplateProcessor()
        {

        }

        public TemplateProcessor(string template, IDictionary<string, object> symbols)
        {
            this.symbols = symbols;
            this.Template = template;
        }

        public void SetSymbol(string symbol, string value)
        {
            symbols[symbol] = value;
        }

        public string Process()
        {
            return ProcessDictionary(Template, symbols);
        }


        public static string Process(string template, object symbols)
        {
            if (template == string.Empty)
                return string.Empty;

            var properties = symbols.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            return ProcessDictionary(template, properties.ToDictionary(p => p.Name, p => p.GetValue(symbols)));
        }

        public static string ProcessDictionary(string template, IDictionary<string, object> symbols)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (symbols == null)
                throw new ArgumentNullException(nameof(symbols));

            if (template == string.Empty)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            int spaces = 0;
            int tabs = 0;

            for (int i = 0; i < template.Length; i++)
            {
                var c = template[i];

                if (c == ' ')
                    spaces++;
                else if (c == '\t')
                    tabs++;

                while (template[i] == '@' && i < template.Length)
                {
                    var key = ReadSymbol(template, ref i);
                    if (key != null)
                    {
                        if (symbols.TryGetValue(key, out object value))
                        {
                            var valueStr = FormatString(value.ToString(), spaces, tabs);
                            sb.Append(valueStr);
                        }
                    }
                    else
                    {
                        sb.Append("@");
                        i++;
                    }
                }

                if (c != '\t' && c != ' ')
                {
                    spaces = 0;
                    tabs = 0;
                }

                if (i < template.Length)
                    sb.Append(template[i]);
            }

            return sb.ToString();
        }

        static string ReadSymbol(string template, ref int i)
        {
            if (IsSymbol(template, i, out bool matchCloseBracket))
            {
                StringBuilder sb = new StringBuilder();
                i += 2;

                if (matchCloseBracket)
                {
                    SkipSpaces(template, ref i);
                }

                while (i < template.Length && char.IsLetterOrDigit(template[i]) || template[i] == '_')
                {
                    sb.Append(template[i]);
                    i++;
                }

                if (matchCloseBracket)
                {
                    SkipSpaces(template, ref i);

                    if (template[i] != '}')
                        throw new InvalidOperationException($"Invalid Macro definition {sb.ToString()}{template[i]}");
                    i++;
                }

                return sb.ToString();
            }

            return null;
        }

        private static void SkipSpaces(string template, ref int i)
        {
            while (i < template.Length && char.IsWhiteSpace(template[i]))
                i++;
        }

        public static string FormatString(string value, int spaces, int tabs)
        {
            StringBuilder sb = new StringBuilder();

            int cursor = 0;
            string line = ReadLine(value, ref cursor);
            sb.Append(line);

            line = ReadLine(value, ref cursor);
            while (line != null)
            {
                sb.Append('\t', tabs);
                sb.Append(' ', spaces);
                sb.Append(line);

                line = ReadLine(value, ref cursor);
            }

            return sb.ToString();
        }

        private static string ReadLine(string value, ref int cursor)
        {
            StringBuilder sb = new StringBuilder();
            while (cursor < value.Length)
            {
                var c = value[cursor];
                if (c == '\n')
                {
                    cursor++;
                    sb.Append(c);
                    return sb.ToString();
                }

                sb.Append(c);
                cursor++;
            }

            if (sb.Length == 0)
                return null;

            return sb.ToString();
        }

        static bool IsSymbol(string template, int i, out bool matchCloseBracket)
        {
            var c = template[i];
            matchCloseBracket = false;

            if (c == '@')
            {
                var next = (i + 1) < template.Length ? template[i + 1] : -1;

                if (next == '@')
                    return true;
                else if (next == '{')
                {
                    matchCloseBracket = true;
                    return true;
                }
            }

            return false;
        }

        public void Save(string filename)
        {
            File.WriteAllText(filename, Process());
        }
    }

    public class SymbolCollection : Dictionary<string, object>
    {
    }
}
