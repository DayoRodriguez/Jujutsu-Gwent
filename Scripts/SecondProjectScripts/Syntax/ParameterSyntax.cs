using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterSyntax : SyntaxNode
{
    internal ParameterSyntax(SyntaxTree syntaxTree, Token identifier, TypeClauseSyntax kind)
    : base(syntaxTree)
    {
        Identifier = identifier;
        Kind = kind;
    }

    public override TokenType Type => TokenType.Parameter;
    public Token Identifier { get; }
    public TypeClauseSyntax Kind { get; }
}
