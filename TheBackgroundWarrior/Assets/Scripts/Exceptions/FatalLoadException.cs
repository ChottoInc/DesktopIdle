using System;

public class FatalLoadException : Exception
{
    public FatalLoadException() { }

    public FatalLoadException(string message) : base(message) { }

    public FatalLoadException(string message, Exception inner) : base(message, inner) { }
}
