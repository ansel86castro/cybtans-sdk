using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public class ParserFunctionAttribute : Attribute
    {
        public ParserFunctionAttribute(string name)
        {
            this.Name = name;
        }

        public ParserFunctionAttribute()
        {

        }

        public string Name { get; set; }
    }

    public static class Functions
    {
        [ParserFunction]
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            if (s.Length == 1)
                return s.ToUpper();

            var firstLetter = s[0];            

            return char.ToUpperInvariant(firstLetter) + s.Substring(1);
        }

        [ParserFunction]
        public static string Uncapitalize(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            if (s.Length == 1)
                return s.ToLowerInvariant();

            var firstLetter = s[0];

            return char.ToLowerInvariant(firstLetter) + s.Substring(1);
        }

        [ParserFunction]
        public static string Upper(this string s)
        {
            return s.ToUpperInvariant();
        }

        [ParserFunction]
        public static string Lower(this string s)
        {
            return s.ToLowerInvariant();
        }

        [ParserFunction]
        public static string Pascal(this string s)
        {
            var sections = s.Split('_');
            StringBuilder sb = new StringBuilder();
            foreach (var part in sections)
            {

                for (int i = 0; i < part.Length; i++)
                {
                    var c = part[i];
                    if (i == 0)
                    {
                        sb.Append(char.ToUpperInvariant(c));
                    }
                    else if (i < part.Length - 1 && char.IsLower(part[i - 1]) && char.IsUpper(c))
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(char.ToLowerInvariant(c));
                    }

                }             
            }

            var result = sb.ToString();
            if(result.All(x => char.IsDigit(x)))
            {
                result = "_" + result;
            }

            return result;
        }

        [ParserFunction]
        public static string Camel(this string s)
        {
            StringBuilder sb = new StringBuilder();
            var sections = s.Split('_');

            if (sections.Length > 0)
            {
                foreach (var part in sections)
                {
                    for (int i = 0; i < part.Length; i++)
                    {
                        var c = part[i];
                        if (i == 0)
                        {
                            sb.Append(char.ToLowerInvariant(c));
                        }
                        else if (i < part.Length - 1 && char.IsLower(part[i - 1]) && char.IsUpper(c))
                        {
                            sb.Append(c);
                        }
                        else
                        {
                            sb.Append(char.ToLowerInvariant(c));
                        }

                    }
                }
            }

            var result = sb.ToString();
            if (result.All(x => char.IsDigit(x)))
            {
                result = "_" + result;
            }

            return result;
        }
    }


}
