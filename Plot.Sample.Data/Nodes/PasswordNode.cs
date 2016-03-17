using System;

namespace Plot.Sample.Data.Nodes
{
    public class PasswordNode
    {
        public PasswordNode()
        {
            
        }

        public PasswordNode(Password item)
        {
            Id = item.Id;
            Hash = item.Hash;
            Salt = item.Salt;
            Created = item.Created;
        }

        public string Id { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public DateTime Created { get; set; }

        public Password AsPassword()
        {
            return new Password
            {
                Id = Id,
                Hash = Hash,
                Salt = Salt,
                Created = Created
            };
        }
    }
}
