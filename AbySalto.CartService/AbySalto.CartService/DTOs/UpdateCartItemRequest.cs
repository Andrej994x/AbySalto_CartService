using System.ComponentModel.DataAnnotations;

namespace AbySalto.CartService.DTOs;

public class UpdateCartItemRequest
{
    [Range(1, 999)]
    public int Quantity { get; set; }
}