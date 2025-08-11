namespace server.Repositories
{
    public interface IAdminServices
    {
        Task<int> ProfetionnalsCount();
        Task<int> ClientsCount();

        Task<int> ReservationsCountThisMonth();

        Task<int> CancelingPayment();

        Task<int> PendingPayment();

        Task<decimal> TotalRevenue();
    }
}
