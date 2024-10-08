using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class BoundUnaryOperator
{
    public BoundUnaryOperator(TokenType tokenType, BoundUnaryOperatorKind kind, Type operandType) 
    : this(tokenType, kind, operandType, operandType)
    {

    }
    public BoundUnaryOperator(TokenType tokenType, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
    {
        TokenType = tokenType;
        Kind = kind;
        OperandType = operandType;
        ResultType = resultType;
    }

    public TokenType TokenType{get;}
    public BoundUnaryOperatorKind Kind{get;}
    public Type OperandType{get;}
    public Type ResultType{get;}

    private static BoundUnaryOperator[] _operators = 
    {
        new BoundUnaryOperator(TokenType.Bang, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
        new BoundUnaryOperator(TokenType.Plus, BoundUnaryOperatorKind.Identity, typeof(int)),
        new BoundUnaryOperator(TokenType.Minus, BoundUnaryOperatorKind.Negation, typeof(int))
    };

    public static BoundUnaryOperator Bind(TokenType tokenType, Type operandType)
    {
        foreach(var op in _operators)
        {
            if(op.TokenType == tokenType && op.OperandType == operandType)
                return op;    
        }
        return null;
    }
}
