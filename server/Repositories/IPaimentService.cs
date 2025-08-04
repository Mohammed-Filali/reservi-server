namespace server.Repositories
{
    public interface IPaimentService
    {
        Task<bool> defaultPaiment(string userId, int reservationID);
    }
}
