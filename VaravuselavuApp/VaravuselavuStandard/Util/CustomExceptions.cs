using System;

namespace VaravuselavuStandard.Util
{
    public class CustomExceptions : Exception
    {
        public CustomExceptions() :base()
        {
        }

    public CustomExceptions(string message)
        : base(message)
        {
        }
    }
}