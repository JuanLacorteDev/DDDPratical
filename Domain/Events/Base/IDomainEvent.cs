using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Events.Base
{
    public interface IDomainEvent
    {
        DateTime DateOccurred { get; }
    }
}
