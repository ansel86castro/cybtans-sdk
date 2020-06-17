using System;
using System.Collections.Generic;

namespace Cybtans.Entities
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Enum)]
    public class GenerateMessageAttribute : Attribute
    {
        public GenerateMessageAttribute(string name = null)
        {
            this.Name = name;            
        }

        public GenerateMessageAttribute(string name, params string[] excluded)
        {
            this.Name = name;
            this.Exclude = excluded;
        }


        public string Name { get; }

        public bool GenerateCrudService { get; set; } = false;     

        public string[] Exclude { get; set; }

        public HashSet<string> GetExcluded()
        {
            HashSet<string> excluded = new HashSet<string>();
            if(Exclude != null)
            {
                foreach (var item in Exclude)
                {
                    excluded.Add(item);
                }
            }
            return excluded;
        }
    }

}
