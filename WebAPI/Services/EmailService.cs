using StackExchange.Redis;
using System.Text.Json;
using WebAPI.Models.Redis;

namespace WebAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IDatabase database;

        public EmailService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis");
            var redis = ConnectionMultiplexer.Connect(connectionString);
            database = redis.GetDatabase();
        }

        public async Task SendEmail(EmailMessage emailMessage)
        {
            var serializedEmailMessage = JsonSerializer.Serialize(emailMessage);

            await database.ListRightPushAsync("message", serializedEmailMessage);
        }
    }
}
