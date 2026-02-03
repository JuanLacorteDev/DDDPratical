using DDDPratical.Domain.Commom.Enums;
using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Tests.Entities;

public class PaymentTests
{
    [Fact(DisplayName = "Should create a valid payment when data is valid")]
    public void Create_ShouldReturnValidPayment_WhenDataIsValid()
    {
        var orderId = Guid.NewGuid();
        var method = PaymentMethod.CreditCard;
        var amount = 100.00m;

        var payment = new Payment(orderId, method, amount);

        payment.OrderId.Should().Be(orderId);
        payment.Method.Should().Be(method);
        payment.Amount.Should().Be(amount);


        //Validate default values
        payment.Status.Should().Be(PaymentStatus.Pending);
        payment.PaidAt.Should().BeNull();
        payment.TransactionId.Should().BeNull();
    }

    [Fact(DisplayName = "Should not define transaction id null or whitespace")]
    public void SetTransactionId_ShouldThrowDomainException_WhenTransactionIdIsNullOrWhitespace()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100);

        Action actNull = () => payment.SetTransactionId(null!);
        actNull.Should().Throw<DomainException>()
            .Where(exp => exp.Message.Contains("can not be null or white space."));

        Action actWhitespace = () => payment.SetTransactionId("   ");
        actWhitespace.Should().Throw<DomainException>()
            .Where(exp => exp.Message.Contains("can not be null or white space."));
    }

    [Fact(DisplayName = "Should define transaction valid and update updateAt date")]
    public void SetTransactionId_ShouldSetTransactionIdAndUpdateUpdatedAt_WhenTransactionIdIsValid()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100);
        var transactionId = "TX1-23456";
        payment.SetTransactionId(transactionId);

        payment.TransactionId.Should().Be(transactionId);
        payment.UpdatedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should not redefine transaction once is all ready define")]
    public void SetTransactionId_ShouldNotChangeTransactionId_WhenTransactionIdIsAllReadyDefined()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100);
        payment.SetTransactionId("TX1-23456");

        Action act = () => payment.SetTransactionId("TX2-65432");

        act.Should().Throw<DomainException>()
            .Where(exp => exp.Message.Contains("Transaction ID has already been set."));
    }


    [Theory(DisplayName = "Should throw domain exception when data is not valid")]
    [InlineData("00000000-0000-0000-0000-000000000000", PaymentMethod.CreditCard, 100, "can not be Guid.Empty.")]
    [InlineData("d290f1ee-6c54-4b01-90e6-d701748f0851", PaymentMethod.CreditCard, 0, "Amount must be grater than zero.")]
    [InlineData("d290f1ee-6c54-4b01-90e6-d701748f0851", null, 100, "Invalid payment method.")]
    public void Create_ShouldThrowDomainException_WhenDataIsInvalid(Guid orderId, PaymentMethod method, decimal amount, string message)
    {
        Action act = () => new Payment(orderId, method, amount);

        act.Should().Throw<DomainException>()
            .Where(exp => exp.Message.Contains(message));
    }

}
