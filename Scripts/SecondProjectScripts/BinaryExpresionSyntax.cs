using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class BinaryExpresionSyntax : ExpresionSyntax
{
   public ExpresionSyntax Left{get;}
   public SyntaxNode OperatorToken{get;}
   public ExpresionSyntax Right{get;}

   public override TokenType Type => TokenType.BinaryExpresion;

   public BinaryExpresionSyntax(ExpresionSyntax left, SyntaxNode operatorToken, ExpresionSyntax right)
   {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
   }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return OperatorToken;
        yield return Right;
    }
}
