using DDDPratical.Domain.Events.Base;
using DDDPratical.Domain.ValueObjects;

namespace DDDPratical.Domain.Events.Order;

public sealed record OrderSentEvent(
    Guid OrderId, 
    Guid ClientId, 
    DeliveryAddress DeliveryAddress) : DomainEventBase;
