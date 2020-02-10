using System;

namespace PascalInterpreter
{
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException() { }

        public InvalidSyntaxException(string message) : base(message) { }

        public InvalidSyntaxException(string message, Exception inner) : base(message, inner) { }
    }
}
