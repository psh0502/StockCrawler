using System;

namespace TomTang.Core
{
    public class CopyrightViolationException : Exception
    {
        public CopyrightViolationException(string message) : base(message) { }
    }
}
