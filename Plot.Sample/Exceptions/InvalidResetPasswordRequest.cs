using System;

namespace Plot.Sample.Exceptions
{
    public class InvalidResetPasswordRequest : Exception
    {
        public InvalidResetPasswordRequest() : base("The reset password request has expired")
        {
            
        }
    }
}
