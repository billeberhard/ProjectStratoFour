
namespace StratoFour.Infrastructure
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task<List<T>> LoadData<T, U>(string sql, U paramaters);
        Task SaveData<T>(string sql, T paramaters);
    }
}