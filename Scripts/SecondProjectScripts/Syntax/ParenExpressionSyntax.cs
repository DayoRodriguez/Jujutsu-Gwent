using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ParenExpressionSyntax : ExpresionSyntax
{
    public override TokenType Type => TokenType.ParenExpression;
    
    public Token OpenParenToken{get;}
    public ExpresionSyntax Expression{get;}
    public Token CloseParenToken{get;}

    public ParenExpressionSyntax(SyntaxTree syntaxTree, Token openParenToken, ExpresionSyntax expression, Token closeParenToken)
    : base(syntaxTree)
    {
        OpenParenToken = openParenToken;
        Expression = expression;
        CloseParenToken = closeParenToken;
    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenToken;
        yield return Expression;
        yield return CloseParenToken;
    }
}
