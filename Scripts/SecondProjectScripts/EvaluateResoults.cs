using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EvaluateResoults 
{
    public EvaluateResoults(IEnumerable<Diagnostics> diagnostics, object? value)
    {
        Diagnostics = diagnostics;
        Value = value;
    }

    public IEnumerable<Diagnostics> Diagnostics { get; }
    public object? Value { get; }
    
}
