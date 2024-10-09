using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class VariableDeclarationSyntax : StatementSyntax
{
    internal VariableDeclarationSyntax(SyntaxTree syntaxTree, Token keyword, Token identifier, TypeClauseSyntax? typeClause, Token equalsToken, ExpresionSyntax initializer)
    : base(syntaxTree)
    {
        Keyword = keyword;
        Identifier = identifier;
        TypeClause = typeClause;
        EqualsToken = equalsToken;
        Initializer = initializer;
    }

    public override TokenType Type => TokenType.VariableDeclaration;
    public Token Keyword { get; }
    public Token Identifier { get; }
    public TypeClauseSyntax? TypeClause { get; }
    public Token EqualsToken { get; }
    public ExpresionSyntax Initializer { get; }
}
