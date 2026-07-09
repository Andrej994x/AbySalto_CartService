using System.ComponentModel.DataAnnotations;

namespace AbySalto.CartService.DTOs;

public class UpdateCartItemRequest
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}