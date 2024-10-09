using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class BoundNode
{
    public abstract BoundNodeKind Kind{get;}
}

public enum BoundNodeKind
{
    UnaryExpression,
    BinaryExpression,
    LiteralExpression,
    VariableExpression,
    AssignmentExpression,
    CallExpression,
    ConversionExpression,
    ExpressionStatement,
    NopStatement,
    VariableDeclaration,
    GotoStatement,
    ConditionalGotoStatement,
    LabelStatement,
    ReturnStatement  
}
