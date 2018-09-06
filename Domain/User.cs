using System;

namespace Domain
{
    public class User : Entity
    {
        public override object UniqueId
        {
            get { return Id; }
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int? Gender { get; set; }

    }
}
