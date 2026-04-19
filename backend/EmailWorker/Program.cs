using EmailWorker.Models;
using MailKit.Net.Smtp;
using MimeKit;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static async Task Main(string[] args)
    {
        var settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var redis = await ConnectionMultiplexer.ConnectAsync(settings["Redis:Url"] ?? "");
        var database = redis.GetDatabase();
        Console.WriteLine("Email worked started");

        while (true)
        {
            var message = await database.ListLeftPopAsync("message");

            if (!message.IsNullOrEmpty)
            {
                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);
                if(emailMessage != null)
                {
                    SendEmail(emailMessage,
                        senderEmail: settings["Mail:Sender"] ?? "",
                        host: settings["Mail:Host"] ?? "",
                        port: int.Parse(settings["Mail:Port"] ?? ""),
                        password: settings["Mail:Password"] ?? "");

                }

            }
        }
    }

    // API -> Redis (Docker) -> Client
    public static void SendEmail(EmailMessage emailMessage, string senderEmail, string host, int port, string password)
    {
        var message = new MimeMessage();
        message.Subject = emailMessage.Subject;
        message.From.Add(new MailboxAddress("PcShop", senderEmail));
        message.To.Add(new MailboxAddress(emailMessage.ToName, emailMessage.ToEmail));

        var body = new BodyBuilder
        {
            HtmlBody = emailMessage.Content,
            TextBody = emailMessage.Content    
        };

        message.Body = body.ToMessageBody();

        using var client = new SmtpClient();
        client.Connect(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(senderEmail, password);
        client.Send(message);
        client.Disconnect(true);
        Console.WriteLine("Message sent!");
        Console.WriteLine($"{emailMessage.ToEmail}\n{emailMessage.Content}");


    }
}