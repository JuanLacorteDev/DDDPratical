using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Base;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot() : base() { }

    protected AggregateRoot(Guid id) : base(id) { }
}
