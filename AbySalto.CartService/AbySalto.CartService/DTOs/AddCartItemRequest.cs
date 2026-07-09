using System.ComponentModel.DataAnnotations;

namespace AbySalto.CartService.DTOs;

public class AddCartItemRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal UnitPrice { get; set; }
}