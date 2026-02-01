using DDDPratical.Domain.Commom.Base;
using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Commom.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Entities;

public sealed class OrderItem : Entity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal AppliedDiscount { get; private set; }
    public decimal TotalAmount { get; private set; }


    internal OrderItem(Guid productId, string productName, decimal unitPrice, int quantity, decimal appliedDiscount)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.AgainstNullOrWhiteSpace(productName, nameof(productName));
        Guard.Against<DomainException>(unitPrice <= 0, "Unit price must be greater than zero.");
        Guard.Against<DomainException>(quantity <= 0, "Quantity must be greater than zero.");

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        AppliedDiscount = appliedDiscount;
        CalculateTotalAmount();
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = (UnitPrice * Quantity) - AppliedDiscount;
    }

    public void ApplyDicount(decimal discount)
    {
        Guard.Against<DomainException>(discount < 0, "Discount cannot be negative.");
        Guard.Against<DomainException>(discount > (UnitPrice * Quantity), "Discount cannot exceed total item amount.");

        AppliedDiscount = discount;

        SetUpdatedAt();
        CalculateTotalAmount();
    }

    public void AddQuantity(int quantity)
    {
        Guard.Against<DomainException>(quantity <= 0, "Quantity to add must be greater than zero.");
       
        Quantity += quantity;
        SetUpdatedAt();
        CalculateTotalAmount();
    }

    public void RemoveQuantity(int quantity) {
        Guard.Against<DomainException>(quantity <= 0, "Quantity to remove must be greater than zero.");
        Guard.Against<DomainException>(quantity > Quantity, "Quantity to remove cannot exceed current quantity.");
        Quantity -= quantity;

        Guard.Against<DomainException>(Quantity == 0, "Order item quantity cannot be zero. Consider removing the item from the order.");

        SetUpdatedAt();
        CalculateTotalAmount();
    }

    public void UpdateUnitPrice(decimal newUnitPrice)
    {
        Guard.Against<DomainException>(newUnitPrice <= 0, "New unit price must be greater than zero.");
        UnitPrice = newUnitPrice;
        SetUpdatedAt();
        CalculateTotalAmount();
    }

    public static OrderItem Create(Guid productId, string productName, decimal unitPrice, int quantity, decimal appliedDiscount)
    {
        return new OrderItem(productId, productName, unitPrice, quantity, appliedDiscount);
    }

}
