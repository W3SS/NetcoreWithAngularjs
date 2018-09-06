using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public abstract class Entity
    {
        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool Equals(object obj)
        {
            if (!((obj != null) && (obj is Entity)))
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            Entity entity = obj as Entity;
            if (entity.IsTransient() || this.IsTransient())
            {
                return false;
            }
            return this.UniqueId.Equals(entity.UniqueId);
        }

        /// <summary>
        /// The GetHashCode
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public override int GetHashCode()
        {
            return ((this.UniqueId == null) ? base.GetHashCode() : this.UniqueId.GetHashCode());
        }

        /// <summary>
        /// The IsTransient
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        public virtual bool IsTransient()
        {
            return ((this.UniqueId == null) || (this.UniqueId == null));
        }


        public static bool operator ==(Entity left, Entity right)
        {
            if (object.Equals(left, null))
            {
                return object.Equals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Gets or sets the UniqueId
        /// </summary>
        public virtual Object UniqueId { get; set; }
    }
}
