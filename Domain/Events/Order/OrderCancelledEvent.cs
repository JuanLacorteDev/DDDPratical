using DDDPratical.Domain.Commom.Enums;
using DDDPratical.Domain.Events.Base;
using DDDPratical.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Events.Order;

public sealed record OrderCancelledEvent(
    Guid OrderId, 
    Guid ClientId,
    OrderStatus PreviousStatus,
    ReasonCancellation Reason,
    Guid? PaymentId): DomainEventBase;
