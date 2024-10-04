using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Evaluator 
{
    private readonly ExpresionSyntax _root;

    public Evaluator(ExpresionSyntax root)
    {
        _root = root;
    }

    public int Evaluate()
    {
        return EvaluateExpresion(_root);
    }

    private int EvaluateExpresion(ExpresionSyntax node)
    {
        //BinaryExpresion
        //NumberExpresion

        if(node is NumberExpresionSyntax n)
        {
            return (int) n.NumberToken.Value;
        }

        if(node is BinaryExpresionSyntax b)
        {
            var left = EvaluateExpresion(b.Left);
            var right = EvaluateExpresion(b.Right);

            if(b.OperatorToken.Type == TokenType.Plus)
            {
                return (int) left + right;
            }
            if(b.OperatorToken.Type == TokenType.Minus)
            {
                return (int) left - right;
            }
            else if(b.OperatorToken.Type == TokenType.Star)
            {
                return (int) left * right;
            }
            else if(b.OperatorToken.Type == TokenType.Slash)
            {
                return (int) left / right;
            }
            else 
                throw new Exception($"Unexpected binary operator'{b.OperatorToken.Type}'");
        }

        if(node is ParenthesizedExpressionSyntax p)
            return EvaluateExpresion(p.Expresion);

        throw new Exception($"Unexpected node '{node.Type}'");
    }
}
