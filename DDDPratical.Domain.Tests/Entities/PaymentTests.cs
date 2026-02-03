using DDDPratical.Domain.Commom.Enums;
using DDDPratical.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Tests.Entities;

public class PaymentTests
{
    [Fact(DisplayName = "Should create a valid payment when data is valid")]
    public void Should_Create_Valid_Payment_When_Data_Is_Valid()
    {
        var orderId = Guid.NewGuid();
        var method = PaymentMethod.CreditCard;
        var amount = 100.00m;

        var payment = new Payment(orderId, method, amount);

        payment.OrderId.Should().Be(orderId);
        payment.Method.Should().Be(method);
        payment.Amount.Should().Be(amount);
        payment.Status.Should().Be(PaymentStatus.Pending);
    }

}
