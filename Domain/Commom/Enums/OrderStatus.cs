using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Enums
{
    public enum OrderStatus
    {
        Created = 1,
        Pending = 2,
        Paid = 3,
        Shipped = 4,
        Delivered = 5,
        Canceled = 6
    }
}
