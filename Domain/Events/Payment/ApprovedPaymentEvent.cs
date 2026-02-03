using DDDPratical.Domain.Events.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Events.Payment;


//as event represent historical fact, use record is a good pratice to guarantee immutability
public record ApprovedPaymentEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    DateTime Date,
    string? TransactionId) : DomainEventBase;
