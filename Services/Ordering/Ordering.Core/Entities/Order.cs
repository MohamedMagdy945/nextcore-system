using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Core.Entities
{
    public class Order : EntityBase
    {
        public string UserName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        // Customer Info
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;

        // Address
        public string AddressLine { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        // Payment
        public string CardName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiration { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;

        public int PaymentMethod { get; set; }
    }
}