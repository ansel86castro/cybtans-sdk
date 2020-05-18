namespace Cybtans.Proto.Generators.CSharp
{
    public class GenerationOptions
    {        
        public TypeGeneratorOption ModelOptions { get; set; }

        public TypeGeneratorOption ServiceOptions { get; set; }

        public TypeGeneratorOption ControllerOptions { get; set; }

        public TypeGeneratorOption ClientOptions { get; set; }
    }
}
