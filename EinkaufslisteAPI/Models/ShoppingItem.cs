namespace EinkaufslisteAPI.Models
{
    public class ShoppingItem
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool Purchased { get; set; }
        public string UserId { get; set; } 
    }

}

