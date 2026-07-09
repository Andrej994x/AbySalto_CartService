namespace AbySalto.CartService.DTOs;

public class CartResponse
{
    public Guid CartId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public List<CartItemResponse> Items { get; set; } = new();

    public decimal TotalAmount { get; set; }
}