using System;

#nullable enable

namespace Cybtans.Entities.MongoDb
{
    public class MongoCollectionAttribute: Attribute
    {
        public string Name { get; }

        public MongoCollectionAttribute(string name)
        {
            Name = name;
        }
    }
}
