using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BinaryExpresionSyntax : ExpresionSyntax
{
   public ExpresionSyntax Left{get;}
   public Token OperatorToken{get;}
   public ExpresionSyntax Right{get;}

   public override TokenType Type => TokenType.BinaryExpresion;

   public BinaryExpresionSyntax(SyntaxTree syntaxTree, ExpresionSyntax left, Token operatorToken, ExpresionSyntax right)
   : base(syntaxTree)
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
