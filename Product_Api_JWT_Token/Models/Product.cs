using System.ComponentModel.DataAnnotations;

namespace Product_Api_JWT_Token.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Images { get; set; }

    }
}
