namespace ApplicationCore.Services
{
    public interface IPasswordAlgorithm
    {
        void CreatePassword(string password, out byte[] hash, out byte[] salt);
        bool ComparePasswordHash(string password, byte[] hash, byte[] salt);
    }
}
