namespace StockAlarmSystem.Models
{
    public class Product//ürünler tanlosu 
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Stock { get; set; }

        public int MinStock { get; set; }

        public decimal Price { get; set; }
        public bool IsCritical { get; set; }//kritik
    }
}
