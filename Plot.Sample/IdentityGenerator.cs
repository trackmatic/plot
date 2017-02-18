using System;

namespace Plot.Sample
{
    public static class IdentityGenerator
    {
        private static readonly long EpochMilliseconds = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000L;

        public static Guid NewSequentialId()
        {
            // This code was not reviewed to guarantee uniqueness under most conditions, nor completely optimize for avoiding
            // page splits in SQL Server when doing inserts from multiple hosts, so do not re-use in production systems.
            var guidBytes = Guid.NewGuid().ToByteArray();

            // get the milliseconds since Jan 1 1970
            byte[] sequential = BitConverter.GetBytes((DateTime.Now.Ticks / 10000L) - EpochMilliseconds);

            // discard the 2 most significant bytes, as we only care about the milliseconds increasing, but the highest ones 
            // should be 0 for several thousand years to come (non-issue).
            if (BitConverter.IsLittleEndian)
            {
                guidBytes[0] = sequential[5];
                guidBytes[1] = sequential[4];
                guidBytes[2] = sequential[3];
                guidBytes[3] = sequential[2];
                guidBytes[4] = sequential[1];
                guidBytes[5] = sequential[0];
            }
            else
            {
                Buffer.BlockCopy(sequential, 2, guidBytes, 0, 6);
            }

            return new Guid(guidBytes);
        }
    }
}
