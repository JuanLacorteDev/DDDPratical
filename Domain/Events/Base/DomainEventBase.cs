using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Events.Base;

public abstract record class DomainEventBase : IDomainEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
