using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class TypeClauseSyntax : SyntaxNode
{
    internal TypeClauseSyntax(SyntaxTree syntaxTree, Token colonToken, Token identifier)
    : base(syntaxTree)
    {
        ColonToken = colonToken;
        Identifier = identifier;
    }

    public override TokenType Type => TokenType.TypeClause;
    public Token ColonToken { get; }
    public Token Identifier { get; }
}