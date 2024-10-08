using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public sealed class EvaluateResoults 
{
    public EvaluateResoults(ReadOnlyCollection<Diagnostic> diagnostics, object? value)
    {
        Diagnostics = diagnostics;
        Value = value;
    }

    public ReadOnlyCollection<Diagnostic> Diagnostics { get; }
    public object? Value { get; }
    
}
