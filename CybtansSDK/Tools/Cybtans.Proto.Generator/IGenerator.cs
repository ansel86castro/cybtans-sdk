namespace Cybtans.Proto.Generator
{
    public interface IGenerator
    {
        bool Generate(string[] args);

        bool Generate(CybtansConfig config, GenerationStep step);

        void PrintHelp();

        bool CanGenerate(string value);
    }
}
