using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Base
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetEqualityComponents().ToArray());
        }


        //if these operators are not overloaded,
        //comparing two value objects using == will check for reference equality, not value equality.
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }
    }
}
