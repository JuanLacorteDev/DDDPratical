using DDDPratical.Domain.Events.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Events.Payment;

public record RefusedPaymentEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    DateTime Date,
    string? TransactionId) : DomainEventBase;