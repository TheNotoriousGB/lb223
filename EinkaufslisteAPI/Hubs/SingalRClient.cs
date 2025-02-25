using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace EinkaufslisteAPI.Hubs
{
    public class SignalRClient
    {
        private readonly HubConnection _connection;

        public SignalRClient()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7081/shoppinglisthub", options =>
                {
                    options.HttpMessageHandlerFactory = handler =>
                    {
                        if (handler is HttpClientHandler clientHandler)
                        {
                            // Falls du mit unsignierten Zertifikaten arbeitest
                            clientHandler.ServerCertificateCustomValidationCallback =
                                (httpRequestMessage, cert, cetChain, policyErrors) => true;
                        }
                        return handler;
                    };
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug); // Setze ein niedriges Log-Level
                })
                .Build();

            // Abonniere die Nachricht, die vom Hub gesendet wird
            _connection.On("ReceiveUpdate", () =>
            {
                // Nachricht empfangen und im UI-Thread verarbeiten
                Console.WriteLine("Update erhalten!");
            });
        }

        // Verbindung zum SignalR-Hub herstellen
        public async Task ConnectAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Verbindung zum Hub erfolgreich!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Verbindung: {ex.Message}");
            }
        }

        // Nachricht an den Hub senden (für Update)
        public async Task SendMessageAsync(string message)
        {
            try
            {
                await _connection.SendAsync("UpdateShoppingList", message);
                Console.WriteLine("Nachricht gesendet!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der Nachricht: {ex.Message}");
            }
        }
    }
}
