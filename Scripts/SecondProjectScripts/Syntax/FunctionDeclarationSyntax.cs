using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class FunctionDeclarationSyntax : MemberSyntax
{
    internal FunctionDeclarationSyntax(SyntaxTree syntaxTree, Token functionKeyword, Token identifier, Token openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, Token closeParenthesisToken, TypeClauseSyntax? kind, BlockStatementSyntax body)
    : base(syntaxTree)
    {
        FunctionKeyword = functionKeyword;
        Identifier = identifier;
        OpenParenthesisToken = openParenthesisToken;
        Parameters = parameters;
        CloseParenthesisToken = closeParenthesisToken;
        Kind = kind;
        Body = body;
    }

    public override TokenType Type => TokenType.FunctionDeclaration;

    public Token FunctionKeyword { get; }
    public Token Identifier { get; }
    public Token OpenParenthesisToken { get; }
    public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
    public Token CloseParenthesisToken { get; }
    public TypeClauseSyntax? Kind { get; }
    public BlockStatementSyntax Body { get; }
}
