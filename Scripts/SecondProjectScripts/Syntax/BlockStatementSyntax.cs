using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStatementSyntax : StatementSyntax
{
    internal BlockStatementSyntax(SyntaxTree syntaxTree, Token openBraceToken, IEnumerable<StatementSyntax> statements, Token closeBraceToken)
    : base(syntaxTree)
    {
        OpenBraceToken = openBraceToken;
        Statements = statements;
        CloseBraceToken = closeBraceToken;
    }

    public override TokenType Type => TokenType.BlockStatement;
    public Token OpenBraceToken { get; }
    public IEnumerable<StatementSyntax> Statements { get; }
    public Token CloseBraceToken { get; }
}
