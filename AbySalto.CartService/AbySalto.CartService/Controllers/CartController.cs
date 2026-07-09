using AbySalto.CartService.DTOs;
using AbySalto.CartService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.CartService.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var cart = await _cartService.GetCartAsync(userId);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("{userId}/items")]
    public async Task<IActionResult> AddItem(string userId, AddCartItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var cart = await _cartService.AddItemAsync(userId, request);

        return Ok(cart);
    }

    [HttpPut("{userId}/items/{itemId:guid}")]
    public async Task<IActionResult> UpdateItemQuantity(
        string userId,
        Guid itemId,
        UpdateCartItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var updated = await _cartService.UpdateItemQuantityAsync(userId, itemId, request);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{userId}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(string userId, Guid itemId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var removed = await _cartService.RemoveItemAsync(userId, itemId);

        if (!removed)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{userId}/clear")]
    public async Task<IActionResult> ClearCart(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var cleared = await _cartService.ClearCartAsync(userId);

        if (!cleared)
            return NotFound();

        return NoContent();
    }
}