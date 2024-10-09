using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallExpressionSyntax : ExpresionSyntax
{
    internal CallExpressionSyntax(SyntaxTree syntaxTree, Token identifier, Token openParenthesisToken, SeparatedSyntaxList<ExpresionSyntax> arguments, Token closeParenthesisToken)
    : base(syntaxTree)
    {
        Identifier = identifier;
        OpenParenthesisToken = openParenthesisToken;
        Arguments = arguments;
        CloseParenthesisToken = closeParenthesisToken;
    }

    public override TokenType Type => TokenType.CallExpression;
    public Token Identifier { get; }
    public Token OpenParenthesisToken { get; }
    public SeparatedSyntaxList<ExpresionSyntax> Arguments { get; }
    public Token CloseParenthesisToken { get; }
}
