#nullable enable


namespace Cybtans.Entities
{
    public interface IDatabaseConnectionFactory
    {
        IDatabaseConnection GetConnection();
    }
}