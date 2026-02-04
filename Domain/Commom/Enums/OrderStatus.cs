using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        InSeparation = 3,
        Shipped = 4,
        Delivered = 5,
        Canceled = 6
    }
}
