namespace server.Repositories
{
    public interface IAdminServices
    {
        Task<int> ProfetionnalsCount();
        Task<int> ClientsCount();
    }
}
