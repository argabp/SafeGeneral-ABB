namespace ABB.Application.Common.Interfaces
{
    public interface IDbContextFactory
    {
        IDbContext CreateDbContext(string databaseName);
    }
}