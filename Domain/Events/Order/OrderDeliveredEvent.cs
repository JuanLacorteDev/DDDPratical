using DDDPratical.Domain.Events.Base;
using DDDPratical.Domain.ValueObjects;

namespace DDDPratical.Domain.Events.Order;

public sealed record OrderDeliveredEvent(
    Guid Orderid, 
    Guid ClientId) : DomainEventBase;

