using AbySalto.CartService.Data;
using AbySalto.CartService.DTOs;
using AbySalto.CartService.Models;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.CartService.Services;

public class CartService : ICartService
{
    private readonly CartDbContext _context;
    private readonly ILogger<CartService> _logger;

    public CartService(
        CartDbContext context,
        ILogger<CartService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CartResponse?> GetCartAsync(string userId)
    {
        _logger.LogInformation("Getting cart for user {UserId}", userId);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            _logger.LogWarning("Cart not found for user {UserId}", userId);
            return null;
        }

        return MapToResponse(cart);
    }

    public async Task<CartResponse> AddItemAsync(string userId, AddCartItemRequest request)
    {
        _logger.LogInformation(
            "Adding product {ProductId} to cart for user {UserId}",
            request.ProductId,
            userId);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            _logger.LogInformation("Creating new cart for user {UserId}", userId);

            cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };

            _context.Carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;

            _logger.LogInformation(
                "Updated quantity for existing product {ProductId} in cart for user {UserId}",
                request.ProductId,
                userId);
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            });

            _logger.LogInformation(
                "Added new product {ProductId} to cart for user {UserId}",
                request.ProductId,
                userId);
        }

        await _context.SaveChangesAsync();

        return MapToResponse(cart);
    }

    public async Task<bool> UpdateItemQuantityAsync(
        string userId,
        Guid itemId,
        UpdateCartItemRequest request)
    {
        _logger.LogInformation(
            "Updating item {ItemId} quantity for user {UserId}",
            itemId,
            userId);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            _logger.LogWarning("Cart not found for user {UserId}", userId);
            return false;
        }

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
        {
            _logger.LogWarning(
                "Item {ItemId} not found in cart for user {UserId}",
                itemId,
                userId);

            return false;
        }

        item.Quantity = request.Quantity;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveItemAsync(string userId, Guid itemId)
    {
        _logger.LogInformation(
            "Removing item {ItemId} from cart for user {UserId}",
            itemId,
            userId);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            _logger.LogWarning("Cart not found for user {UserId}", userId);
            return false;
        }

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
        {
            _logger.LogWarning(
                "Item {ItemId} not found in cart for user {UserId}",
                itemId,
                userId);

            return false;
        }

        _context.CartItems.Remove(item);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        _logger.LogInformation("Clearing cart for user {UserId}", userId);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            _logger.LogWarning("Cart not found for user {UserId}", userId);
            return false;
        }

        _context.CartItems.RemoveRange(cart.Items);

        await _context.SaveChangesAsync();

        return true;
    }

    private static CartResponse MapToResponse(Cart cart)
    {
        return new CartResponse
        {
            CartId = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(i => new CartItemResponse
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.Quantity * i.UnitPrice
            }).ToList(),
            TotalAmount = cart.Items.Sum(i => i.Quantity * i.UnitPrice)
        };
    }
}