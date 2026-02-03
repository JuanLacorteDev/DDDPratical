using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Entities;
using FluentAssertions;

namespace DDDPratical.Domain.Tests.Entities;

public class OrderItemTests
{
    #region Auxiliary Methods
    private static OrderItem CreateOrderItemValid(decimal unitPrice, int quantity)
    {
        var productId = Guid.NewGuid();
        var productName = "Product A";
        var appliedDiscount = 0;
        return new OrderItem(productId, productName, unitPrice, quantity, appliedDiscount);
    }

    #endregion

    #region Create Tests
    [Fact(DisplayName = "Create and return a valid OrderItem when given data is valid")]
    public void Create_ShouldReturnValidOrderItem_WhenDataIsValid()
    {
        //Arrange
        var productId = Guid.NewGuid();
        var productName = "Product A";
        var unitPrice = 100m;
        var quantity = 2;
        var appliedDiscount = 10m;

        //Arrange and Act
        var orderItem = new OrderItem(productId, productName, unitPrice, quantity, appliedDiscount);

        //Assert
        orderItem.Should().NotBeNull();
        orderItem.ProductId.Should().Be(productId);
        orderItem.ProductName.Should().Be(productName);
        orderItem.UnitPrice.Should().Be(unitPrice);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.AppliedDiscount.Should().Be(appliedDiscount);
        orderItem.TotalAmount.Should().Be((unitPrice * quantity) - appliedDiscount);
    }


    [Theory(DisplayName = "Should throw a DomainException when given data is invalid")]
    [InlineData("00000000-0000-0000-0000-000000000000", "Product A", 100, 2, 10, "can not be Guid.Empty.")]
    [InlineData("d290f1ee-6c54-4b01-90e6-d701748f0851", "", 100, 2, 10, "can not be null or white space.")]
    [InlineData("d290f1ee-6c54-4b01-90e6-d701748f0851", "Product A", null, 2, 10, "Unit price must be greater than zero.")]
    [InlineData("d290f1ee-6c54-4b01-90e6-d701748f0851", "Product A", 100, 0, 10, "Quantity must be greater than zero.")]
    public void Create_ShouldThrowDomainException_WhenDataIsInvalid(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity,
        decimal appliedDiscount,
        string expectedMessage)
    {
        //Arrange & Act
        Action act = () => new OrderItem(productId, productName, unitPrice, quantity, appliedDiscount);
        //Assert
        act.Should().Throw<DomainException>()
            .Where(exp => exp.Message.Contains(expectedMessage));
    }
    
    #endregion

    #region Apply Discount Tests
    [Fact(DisplayName = "Apply discount with success when given discount value is valid.")]
    public void ApplyDiscount_ShouldApplyDiscount_WhenDiscountIsValid()
    {
        //Arrange
        var orderItem = CreateOrderItemValid(unitPrice: 200, quantity: 2);
        var discountToApply = 50m;
        var expectedTotalAmount = (orderItem.UnitPrice * orderItem.Quantity) - discountToApply;
        //Act
        orderItem.ApplyDicount(discountToApply);

        //Assert
        orderItem.AppliedDiscount.Should().Be(discountToApply);
        orderItem.TotalAmount.Should().Be(expectedTotalAmount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }

    [Theory(DisplayName = "Apply discount with failure when given discount value is not valid.")]
    [InlineData(401, "Discount cannot exceed total item amount.")]
    [InlineData(-1, "Discount cannot be negative.")]
    public void ApplyDiscount_ShouldNotApplyDiscount_WhenDiscountIsNotValid(decimal discountToApply, string messageExpected)
    {
        //Arrange
        var orderItem = CreateOrderItemValid(unitPrice: 200, quantity: 2);
        //Act
        var act = () => orderItem.ApplyDicount(discountToApply);

        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage(messageExpected);
    }

    #endregion

    #region Add Quantity Tests
    [Fact(DisplayName = "Should add quantity with success when given quantity is valid.")]
    public void AddQuantity_ShouldAddQuantity_WhenQuantityIsValid()
    {
        //Arrange
        var initialQuantity = 2;
        var quantityToAdd = 3;
        var orderItem = CreateOrderItemValid(unitPrice: 150, quantity: initialQuantity);
        var expectedQuantity = initialQuantity + quantityToAdd;
        var expectedTotalAmount = orderItem.UnitPrice * expectedQuantity;

        //Act
        orderItem.AddQuantity(quantityToAdd);

        //Assert
        orderItem.Quantity.Should().Be(expectedQuantity);
        orderItem.TotalAmount.Should().Be(expectedTotalAmount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }

    [Fact(DisplayName ="Should throw a DomainException when add an invalid quantity")]
    public void AddQuantity_ShouldThrowDomainException_WhenQuantityIsInvalid()
    {
        //Arrange
        var orderItem = CreateOrderItemValid(unitPrice: 150, quantity: 2);
        var quantityToAdd = 0;
        //Act
        Action act = () => orderItem.AddQuantity(quantityToAdd);
        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Quantity to add must be greater than zero.");
    }
    #endregion

    #region Remove Quantity Tests

    [Fact(DisplayName ="Should remove item with sucess when data is valid")]
    public void RemoveQuantity_ShouldRemoveQuantity_WhenQuantityIsValid()
    {
        //Arrange
        var initialQuantity = 5;
        var quantityToRemove = 2;
        var orderItem = CreateOrderItemValid(unitPrice: 100, quantity: initialQuantity);
        var expectedQuantity = initialQuantity - quantityToRemove;
        var expectedTotalAmount = orderItem.UnitPrice * expectedQuantity;
        //Act
        orderItem.RemoveQuantity(quantityToRemove);
        //Assert
        orderItem.Quantity.Should().Be(expectedQuantity);
        orderItem.TotalAmount.Should().Be(expectedTotalAmount);
        orderItem.UpdatedAt.Should().NotBeNull();
    }

    [Theory(DisplayName ="Shoud throw a DomainException when data is not valid")]
    [InlineData(4, "Quantity to remove cannot exceed current quantity.")]
    [InlineData(3, "Order item quantity cannot be zero. Consider removing the item from the order.")]
    [InlineData(0, "Quantity to remove must be greater than zero.")]
    public void RemoveQuantity_ShouldThrowDomainException_WhenQuantityIsInvalid(int quantityToRemove, string messageExpected)
    {
        //Arrange
        var orderItem = CreateOrderItemValid(unitPrice: 100, quantity: 3);
        //Act
        Action act = () => orderItem.RemoveQuantity(quantityToRemove);
        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage(messageExpected);
    }

    #endregion


}
