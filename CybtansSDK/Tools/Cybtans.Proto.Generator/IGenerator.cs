namespace Cybtans.Proto.Generator
{
    public interface IGenerator
    {
        bool Generate(string[] args);

        void PrintHelp();

        bool CanGenerate(string value);
    }
}
