using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnaryExpressionSyntax : ExpresionSyntax
{
   public Token OperatorToken{get;}
   public ExpresionSyntax Operand{get;}

   public override TokenType Type => TokenType.UnaryExpresion;

   public UnaryExpressionSyntax(SyntaxTree syntaxTree, Token operatorToken, ExpresionSyntax operand)
   : base(syntaxTree)
   {
        OperatorToken = operatorToken;
        Operand = operand;
   }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OperatorToken;
        yield return Operand;
    }
}
