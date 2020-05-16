using System;
using System.Collections.Generic;
using System.Text;

namespace CybtansSdk.Proto.Generators
{
    public class CodeWriter
    {
        private StringBuilder sb = new StringBuilder();
        Dictionary<string, object> blocks;

        public CodeWriter Block(string blockName)
        {
            if (blocks == null)
            {
                blocks = new Dictionary<string, object>();
            }

            if (!blocks.TryGetValue(blockName, out var writer))
            {
                sb.Append("@{" + blockName + "}");
                writer = new CodeWriter();
                blocks.Add(blockName, writer);
            }
            return (CodeWriter)writer;
        }

        public CodeWriter Append(string value)
        {
            sb.Append(value);
            return this;
        }

        public CodeWriter AppendLine()
        {
            sb.AppendLine();
            return this;
        }

        public CodeWriter Append(char c, int times)
        {
            sb.Append(c, times);
            return this;
        }

        public CodeWriter Append(string value, int times)
        {
            for (int i = 0; i < times; i++)
            {
                sb.Append(value);
            }
            return this;
        }

        public CodeWriter Append(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0)
                    sb.AppendLine();

                sb.Append(values[i]);
            }
            return this;
        }

        public override string ToString()
        {
            if(blocks != null)
            {
                return TemplateProcessor.ProcessDictionary(sb.ToString(), blocks);               
            }

            return sb.ToString();
        }
    }

    public class ClassCodeWriter
    {
        StringBuilder header = new StringBuilder();

        StringBuilder content = new StringBuilder();

        public ClassCodeWriter AppendHeader(string value)
        {
            header.Append(value);
            return this;
        }

        public ClassCodeWriter AppendHeaderLine()
        {
            header.AppendLine();
            return this;
        }

        public ClassCodeWriter AppendHeader(char c, int times)
        {
            header.Append(c, times);
            return this;
        }

        public ClassCodeWriter AppendHeader(string value, int times)
        {
            for (int i = 0; i < times; i++)
            {
                header.Append(value);
            }
            return this;
        }

        public ClassCodeWriter AppendHeaders(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0)
                    header.AppendLine();

                header.Append(values[i]);
            }
            return this;
        }

        public ClassCodeWriter AppendBody(string value)
        {
            content.Append(value);
            return this;
        }

        public ClassCodeWriter AppendBody(string pattern, params object[] value)
        {
            content.AppendFormat(pattern, value);
            return this;
        }

        public ClassCodeWriter AppendBodyLine()
        {
            content.AppendLine();
            return this;
        }

        public ClassCodeWriter AppendBody(char c, int times)
        {
            content.Append(c, times);
            return this;
        }

        public ClassCodeWriter AppendBodyTimes(string value, int times)
        {
            for (int i = 0; i < times; i++)
            {
                content.Append(value);
            }
            return this;
        }

        public virtual string GetCode()
        {
            return header.ToString() + content.ToString();
        }

        public override string ToString()
        {
            return GetCode();
        }
    }
}
