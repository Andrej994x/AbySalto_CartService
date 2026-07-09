using AbySalto.CartService.Data;
using AbySalto.CartService.DTOs;
using AbySalto.CartService.Models;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.CartService.Services;

public class CartService : ICartService
{
    private readonly CartDbContext _context;

    public CartService(CartDbContext context)
    {
        _context = context;
    }

    public async Task<CartResponse?> GetCartAsync(string userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return null;

        return MapToResponse(cart);
    }

    public async Task<CartResponse> AddItemAsync(string userId, AddCartItemRequest request)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
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
        }

        await _context.SaveChangesAsync();

        return MapToResponse(cart);
    }

    public async Task<bool> UpdateItemQuantityAsync(
        string userId,
        Guid itemId,
        UpdateCartItemRequest request)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return false;

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
            return false;

        item.Quantity = request.Quantity;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveItemAsync(string userId, Guid itemId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return false;

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
            return false;

        _context.CartItems.Remove(item);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return false;

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