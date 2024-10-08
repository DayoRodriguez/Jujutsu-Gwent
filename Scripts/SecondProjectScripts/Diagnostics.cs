using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Diagnostics 
{
    private Diagnostics(bool isError, TextLocation location, string message)
    {
        IsError = isError;
        Location = location;
        Message = message;
        IsWarning = !IsError;
    }

    public bool IsError { get; }
    public TextLocation Location { get; }
    public string Message { get; }
    public bool IsWarning { get; }

    public override string ToString() => Message;

    public static Diagnostics Error(TextLocation location, string message)
    {
        return new Diagnostics(isError: true, location, message);
    }

    public static Diagnostics Warning(TextLocation location, string message)
    {
        return new Diagnostics(isError: false, location, message);
    }
}
