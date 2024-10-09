using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal abstract class BoundExpression : BoundNode
{
    public abstract Type Type{get;}
}

internal enum BoundUnaryOperatorKind
{
    Identity,
    Negation,
    LogicalNegation,
    OnesComplement
}
