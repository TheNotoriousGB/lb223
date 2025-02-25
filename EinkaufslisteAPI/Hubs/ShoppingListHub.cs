using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EinkaufslisteAPI.Hubs
{
    public class ShoppingListHub : Hub
    {
        // Diese Methode wird aufgerufen, wenn eine Verbindung erfolgreich hergestellt wird
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client verbunden: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        // Diese Methode wird aufgerufen, wenn ein Client die Verbindung trennt
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Client getrennt: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        // Beispiel für das Senden einer Nachricht an alle verbundenen Clients
        public async Task UpdateShoppingList()
        {
            // Alle Clients über das Update informieren
            await Clients.All.SendAsync("ReceiveUpdate");
        }
    }
}