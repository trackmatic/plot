using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Site
    {
        public Site()
        {
            Assets = new List<Asset>();
            People = new List<Person>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.Operates)]
        public virtual IList<Asset> Assets { get; set; }

        [Relationship(Relationships.Runs, Reverse = true)]
        public virtual Organisation Organisation { get; set; }

        [Relationship(Relationships.Contracts)]
        public virtual IList<Person> People { get; set; }

        public virtual void Set(Organisation organisation)
        {
            if (Organisation == organisation)
            {
                return;
            }
            Organisation = organisation;
            organisation?.Add(this);
        }

        public virtual void Add(Asset asset)
        {
            Utils.Add(Assets, asset, () => asset.Add(this));
        }

        public virtual void Add(Person person)
        {
            Utils.Add(People, person, () => person.Add(this));
        }

        public override int GetHashCode()
        {
            return ProxyUtils.GetHashCode(Id);
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}