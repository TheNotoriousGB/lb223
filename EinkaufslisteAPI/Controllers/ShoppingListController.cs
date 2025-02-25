using EinkaufslisteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using EinkaufslisteAPI.Hubs;

namespace EinkaufslisteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingDbContext _context;
        private readonly IHubContext<ShoppingListHub> _hubContext;

        public ShoppingListController(ShoppingDbContext context, IHubContext<ShoppingListHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // 📌 GET: Alle Einkaufsitems abrufen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingItem>>> GetShoppingItems()
        {
            return await _context.ShoppingItems.ToListAsync();
        }

        // 📌 GET: Ein einzelnes Einkaufsitem abrufen
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingItem>> GetShoppingItem(int id)
        {
            var item = await _context.ShoppingItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // 📌 POST: Neues Item hinzufügen
        [HttpPost]
        public async Task<ActionResult<ShoppingItem>> AddShoppingItem([FromBody] ShoppingItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
                return BadRequest("Ungültige Eingabe.");

            // Validierung der Quantity
            if (item.Quantity <= 0)
            {
                return BadRequest("Die Quantity muss größer als 0 sein.");
            }

            _context.ShoppingItems.Add(item);
            await _context.SaveChangesAsync();

            // Echtzeit-Update an alle Clients senden
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");

            return CreatedAtAction(nameof(GetShoppingItem), new { id = item.Id }, item);
        }

        // 📌 DELETE: Ein Item löschen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingItem(int id)
        {
            var item = await _context.ShoppingItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.ShoppingItems.Remove(item);
            await _context.SaveChangesAsync();

            // Echtzeit-Update an alle Clients senden
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");

            return NoContent();
        }

        // 📌 PUT: Item als gekauft markieren
        [HttpPut("{id}/purchase")]
        public async Task<IActionResult> MarkAsPurchased(int id)
        {
            var item = await _context.ShoppingItems.FindAsync(id);
            if (item == null)
                return NotFound();

            // Optional: Wenn das Item gekauft wurde, Quantity auf 0 setzen
            item.Purchased = true;
            item.Quantity = 0; // Falls du möchtest, dass die Menge beim Kauf auf 0 gesetzt wird
            _context.ShoppingItems.Update(item);
            await _context.SaveChangesAsync();

            // Echtzeit-Update an alle Clients senden
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");

            return NoContent();
        }

        // 📌 PUT: Quantity aktualisieren
        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int quantity)
        {
            var item = await _context.ShoppingItems.FindAsync(id);
            if (item == null)
                return NotFound();

            if (quantity <= 0)
            {
                return BadRequest("Die Quantity muss größer als 0 sein.");
            }

            item.Quantity = quantity;
            _context.ShoppingItems.Update(item);
            await _context.SaveChangesAsync();

            // Echtzeit-Update an alle Clients senden
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate");

            return NoContent();
        }
    }
}
