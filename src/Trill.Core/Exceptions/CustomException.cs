using System;

namespace Trill.Core.Exceptions
{
    public abstract class CustomException : Exception
    {
        public abstract string Code { get; }

        public CustomException(string message) : base(message)
        {
        }
    }
}