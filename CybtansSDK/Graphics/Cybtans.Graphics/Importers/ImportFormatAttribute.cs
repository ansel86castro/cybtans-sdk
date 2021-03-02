using System;

namespace Cybtans.Graphics.Importers
{
    public class ImportFormatAttribute : Attribute
    {
        public ImportFormatAttribute(string fileExtension)
        {
            FileExtension = fileExtension;
        }
        public string FileExtension { get; set; }
    }
}
