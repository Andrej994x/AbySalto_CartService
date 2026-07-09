using AbySalto.CartService.DTOs;

namespace AbySalto.CartService.Services;

public interface ICartService
{
    Task<CartResponse?> GetCartAsync(string userId);

    Task<CartResponse> AddItemAsync(string userId, AddCartItemRequest request);

    Task<bool> UpdateItemQuantityAsync(string userId, Guid itemId, UpdateCartItemRequest request);

    Task<bool> RemoveItemAsync(string userId, Guid itemId);

    Task<bool> ClearCartAsync(string userId);
}