using System;
using System.Collections.Generic;
using System.Text;

namespace DDDPratical.Domain.Commom.Base
{
    public abstract class Entity
    {

        //protected set to allow only derived classes to set the properties
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        //protected constructor to prevent direct instantiation
        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        // useful to ORM like Entity Framework
        protected Entity(Guid id)
        {
            Id = id;
        }

        protected void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }


        //assert equality based on Id or Reference when id are empty
        public override bool Equals(object? obj)
        {
            if (obj is not Entity other) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

    }
}
