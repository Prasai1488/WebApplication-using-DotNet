namespace WebApplication1.Models
{
    public class Product
    {
        public int Id { get; set; }  
        public string Name { get; set; }  
        public string SKU { get; set; }  
        public string Description { get; set; }  

        public int QuantityInStock { get; set; }  
        public int ReorderLevel { get; set; }  
        public int ReorderQuantity { get; set; }  

        public decimal CostPrice { get; set; }  
        public decimal SellingPrice { get; set; }  

        // Foreign Keys
        public int CategoryId { get; set; }  
        public Category Category { get; set; } 

        public int SupplierId { get; set; } 
        public Supplier Supplier { get; set; }  

        public DateTime DateAdded { get; set; } = DateTime.UtcNow; 
        public DateTime? LastUpdated { get; set; }  
        public bool IsDiscontinued { get; set; } 
    }

}
