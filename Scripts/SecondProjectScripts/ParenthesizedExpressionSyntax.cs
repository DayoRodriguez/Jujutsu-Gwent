using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class ParenthesizedExpressionSyntax : ExpresionSyntax
{
    public override TokenType Type => TokenType.ParenExpression;
    public Token OpenParenToken{get;}
    public ExpresionSyntax Expresion{get;}
    public Token CloseParenToken{get;}

    public ParenthesizedExpressionSyntax(Token openParenToken, ExpresionSyntax expresion, Token closeParenToken)
    {
        OpenParenToken = openParenToken;
        Expresion = expresion;
        CloseParenToken = closeParenToken;
    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenToken;
        yield return Expresion;
        yield return CloseParenToken;
    }
}
