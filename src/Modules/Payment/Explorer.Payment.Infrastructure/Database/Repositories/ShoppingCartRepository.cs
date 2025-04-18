﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Infrastructure.Database.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{

    private readonly PaymentContext _dbContext;

    public ShoppingCartRepository(PaymentContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ShoppingCart Get(long id)
    {
        var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.Id == id);
        if (cart == null) throw new KeyNotFoundException("Shopping cart not found.");
        return cart;
    }

    public ShoppingCart GetByUserId(long userId)
    {
        return _dbContext.ShoppingCarts
                         .Include(cart => cart.Items)
                         .FirstOrDefault(cart => cart.TouristId == userId);
    }


    public ShoppingCart Create(ShoppingCart shoppingCart)
    {
        _dbContext.ShoppingCarts.Add(shoppingCart);
        _dbContext.SaveChanges();
        return shoppingCart;
    }

    public ShoppingCart Update(ShoppingCart shoppingCart)
    {
        var existingCart = Get(shoppingCart.Id);
        if (existingCart == null)
        {
            throw new KeyNotFoundException("Shopping cart not found.");
        }

        //  existingCart.UpdateItems(shoppingCart.Items);
        _dbContext.ShoppingCarts.Update(existingCart);
        _dbContext.SaveChanges();
        return existingCart;
    }


    public void Delete(long id)
    {
        var cart = Get(id);
        if (cart != null)
        {
            _dbContext.ShoppingCarts.Remove(cart);
            _dbContext.SaveChanges();
        }
    }

    public PagedResult<ShoppingCart> GetPaged(int page, int pageSize)
    {
        var totalItems = _dbContext.ShoppingCarts.Count();
        var items = _dbContext.ShoppingCarts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<ShoppingCart>(items, totalItems);
    }
}
