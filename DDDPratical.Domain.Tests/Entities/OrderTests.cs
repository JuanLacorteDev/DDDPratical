using DDDPratical.Domain.Commom.Enums;
using DDDPratical.Domain.Entities;
using DDDPratical.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DDDPratical.Domain.Tests.Entities;

public class OrderTests
{
    private static readonly Guid _validClientId = Guid.NewGuid();
    private static readonly Guid _validProductId = Guid.NewGuid();
    
    private static DeliveryAddress CreateValidDeliveryAddres()
    {
        return DeliveryAddress.Create(
            postalCode: "12345-678",
            street: "Main St",
            number: "100",
            complement: "Apt 101",
            neighborhood: "Downtown",
            state: "CA",
            city: "Los Angeles",
            country: "USA"
        );
    }

    private static void SetOrderStatus(Order order, OrderStatus status)
    {
        typeof(Order).GetProperty(nameof(Order.Status), BindingFlags.Public | BindingFlags.Instance)!
                     .SetValue(order, status);
    }

    [Fact(DisplayName = "Should return a valid order when data is valid")]
    public void Create_ShouldReturnValidOrder_WhenDataIsValid()
    {
        // Arrange
        var deliveryAddress = CreateValidDeliveryAddres();

        // Act
        var order = Order.CreateNewOrder(_validClientId, deliveryAddress);

        // Assert
        order.Should().NotBeNull();
        order.ClientId.Should().Be(_validClientId);
        order.DeliveryAddress.Should().Be(deliveryAddress);
        order.Status.Should().Be(OrderStatus.Pending);
        order.TotalAmount.Should().Be(0);
        order.Items.Should().BeEmpty();
        order.Payments.Should().BeEmpty();
        order.Id.Should().NotBeEmpty();
        order.OrderNumber.Should().NotBeNullOrWhiteSpace();

    }

}
