using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class BoundBinaryOperator
{
    public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorKind kind, Type type)
    : this(tokenType, kind, type, type, type)
    {

    }

    public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorKind kind, Type operandType, Type resultType)
    : this(tokenType, kind, operandType, operandType, resultType)
    {

    }
    public BoundBinaryOperator(TokenType tokenType, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
    {
        TokenType = tokenType;
        Kind = kind;
        LeftType = leftType;
        RightType = rightType;
        ResultType = resultType;
    }

    public TokenType TokenType{get;}
    public BoundBinaryOperatorKind Kind{get;}
    public Type LeftType{get;}
    public Type RightType{get;}
    public Type ResultType{get;}

    private static BoundBinaryOperator[] _operators = 
    {
        new BoundBinaryOperator(TokenType.Plus, BoundBinaryOperatorKind.Addition, typeof(int)),
        new BoundBinaryOperator(TokenType.Minus, BoundBinaryOperatorKind.Subtraction, typeof(int)),
        new BoundBinaryOperator(TokenType.Star, BoundBinaryOperatorKind.Multiplication, typeof(int)),
        new BoundBinaryOperator(TokenType.Slash, BoundBinaryOperatorKind.Division, typeof(int)),
        new BoundBinaryOperator(TokenType.EqualEqual, BoundBinaryOperatorKind.Equal, typeof(int), typeof(bool)),
        new BoundBinaryOperator(TokenType.BangEqual, BoundBinaryOperatorKind.NotEqual, typeof(int), typeof(bool)),
        
        new BoundBinaryOperator(TokenType.AmpersandAmpersand, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
        new BoundBinaryOperator(TokenType.PipePipe, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
        new BoundBinaryOperator(TokenType.EqualEqual, BoundBinaryOperatorKind.Equal, typeof(bool)),
        new BoundBinaryOperator(TokenType.BangEqual, BoundBinaryOperatorKind.NotEqual, typeof(bool))
    };

    public static BoundBinaryOperator Bind(TokenType tokenType, Type leftType, Type rightType)
    {
        foreach(var op in _operators)
        {
            if(op.TokenType == tokenType && op.LeftType == leftType && op.RightType == rightType)
                return op;    
        }
        return null;
    }
}
