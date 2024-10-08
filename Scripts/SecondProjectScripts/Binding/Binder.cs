using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

internal sealed class Binder
{
    private readonly List<string> _diagnostics = new List<string>();
    public IEnumerable<string> Diagnostics => _diagnostics;
    public BoundExpression BindExpression(ExpresionSyntax syntax)
    {
        switch(syntax.Type)
        {
            case TokenType.LiteralExpresion:
                return BindLiteralExpression((LiteralExpresionSyntax)syntax);
            case TokenType.UnaryExpresion:
                return BindUnaryExpression((UnaryExpressionSyntax)syntax);
            case TokenType.BinaryExpresion:
                return BindBinaryExpression((BinaryExpresionSyntax)syntax);
            case TokenType.ParenExpression:
                return BindExpression(((ParenExpressionSyntax)syntax).Expression);
            default :
                throw new Exception($"Unexpected syntax '{syntax.Type}'");    
        }
    }

    private BoundExpression BindLiteralExpression(LiteralExpresionSyntax syntax)
    {
        var value = syntax.Value ?? 0;
        return new BoundLiteralExpression(value);
    }
    
    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
    {
        var boundOperand = BindExpression(syntax.Operand);
        var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Type, boundOperand.Type);
        
        if(boundOperator == null)
        {
            _diagnostics.Add($"Unary Operator '{syntax.OperatorToken.Text}' is not defined fot type {boundOperand.Type}");
            return boundOperand;
        }
        
        return new BoundUnaryExpression(boundOperator, boundOperand);
    }
    
    private BoundExpression BindBinaryExpression(BinaryExpresionSyntax syntax)
    {
        var boundLeft = BindExpression(syntax.Left);
        var boundRight = BindExpression(syntax.Right);
        var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Type, boundLeft.Type, boundRight.Type);
        
        if(boundOperator == null)
        {
            _diagnostics.Add($"Binary Operator '{syntax.OperatorToken.Text}' is not defined fot type {boundLeft.Type} and '{boundRight.Type}'");
            return boundLeft;
        }
        
        return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
    }

    private BoundUnaryOperatorKind? BindUnaryOperatorKind(TokenType kind, Type operandType)
    {
        if(operandType == typeof(int))
        {
            switch(kind)
            {
                case TokenType.Plus:
                    return BoundUnaryOperatorKind.Identity;
                case TokenType.Minus:
                    return BoundUnaryOperatorKind.Negation;
            }
        }
        if(operandType == typeof(bool))
        {
            switch(kind)
            {
                case TokenType.Plus:
                    return BoundUnaryOperatorKind.LogicalNegation;
            }
        }
        return null;
    }

    private BoundBinaryOperatorKind? BindBinaryOperatorKind(TokenType kind, Type leftType, Type rightType)
    {
        if(leftType == typeof(int) || rightType == typeof(int))
        {
            switch(kind)
            {
                case TokenType.Plus:
                    return BoundBinaryOperatorKind.Addition;
                case TokenType.Minus:
                    return BoundBinaryOperatorKind.Subtraction;
                case TokenType.Star:
                    return BoundBinaryOperatorKind.Multiplication;
                case TokenType.Slash:
                    return BoundBinaryOperatorKind.Division;    
            }
        }
        if(leftType == typeof(bool) || rightType == typeof(bool))
        {
            switch(kind)
            {
                case TokenType.AmpersandAmpersand:
                    return BoundBinaryOperatorKind.LogicalAnd;
                case TokenType.PipePipe:
                    return BoundBinaryOperatorKind.LogicalOr;
            }
        }
        
        return null;    
        
    }
}
