using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LiteralExpresionSyntax : ExpresionSyntax
{
    public LiteralExpresionSyntax(SyntaxTree syntaxTree, Token literalToken):this (syntaxTree, literalToken, literalToken.Value)
    {
        
    }
    public LiteralExpresionSyntax(SyntaxTree syntaxTree, Token literalToken, object value)
    : base(syntaxTree)
    {
        LiteralToken = literalToken;
        Value = value;
    }

    public Token LiteralToken{get;}
    public object Value{get;}

    public override TokenType Type => TokenType.LiteralExpresion;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }
}
