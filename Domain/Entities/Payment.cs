using DDDPratical.Domain.Commom.Base;
using DDDPratical.Domain.Commom.Enums;
using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.Commom.Validations;
using DDDPratical.Domain.Events.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Entities;

public sealed class Payment : Entity
{
    public Guid OrderId { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public string? TransactionId { get; private set; }


    internal Payment(Guid orderId, PaymentMethod method, decimal amount)
    {
        Guard.AgainstEmptyGuid(orderId, nameof(orderId));
        Guard.Against<DomainException>(amount <= 0, "Amount must be grater than zero.");
        Guard.Against<DomainException>(
            !Enum.IsDefined(typeof(PaymentMethod), method),
            "Invalid payment method."
        );

        OrderId = orderId;
        Method = method;
        Amount = amount;

        Status = PaymentStatus.Pending;
        PaidAt = null;
        TransactionId = null;
    }

    public void SetTransactionId(string transactionId)
    {
        Guard.AgainstNullOrWhiteSpace(transactionId, nameof(transactionId));
        Guard.Against<DomainException>(
            TransactionId is not null,
            "Transaction ID has already been set."
        );
        Guard.Against<DomainException>(
            Status != PaymentStatus.Pending,
            "Cannot set transaction ID for a payment that is not pending."
        );

        TransactionId = transactionId;
        SetUpdatedAt();
    }

    public void GenerateTransactionMock()
    {
        if (TransactionId is not null)
            return;

        var transactionGuid = $"MOCK-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        SetTransactionId(transactionGuid);
    }

    public void MarkAsPaid()
    {
        Guard.Against<DomainException>(
            Status != PaymentStatus.Pending,
            "Only pending payment could be marked as paid"
            );
        Guard.Against<DomainException>(
            string.IsNullOrWhiteSpace(TransactionId),
            "The payment could no be marked as paid without a transaction ID"
            );

        Status = PaymentStatus.Approved;
        PaidAt = DateTime.UtcNow;
        SetUpdatedAt();

        AddDomainEvent(new ApprovedPaymentEvent(Id, OrderId, Amount, PaidAt.Value, TransactionId));
    }

    public void MarkAsRefused()
    {
        Guard.Against<DomainException>(
         Status != PaymentStatus.Pending,
         "Only pending payment could be marked as refused"
         );
         
        Status = PaymentStatus.Refused;
        SetUpdatedAt();

        AddDomainEvent(new RefusedPaymentEvent(Id, OrderId, Amount, DateTime.UtcNow, TransactionId));

    }

}
