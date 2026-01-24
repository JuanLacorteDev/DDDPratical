using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Approved = 2,
        Refused = 3,
        Refunded = 4,
        Canceled = 5
    }
}
