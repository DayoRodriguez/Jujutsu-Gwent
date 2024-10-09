using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakStatementSyntax : StatementSyntax
{
    internal BreakStatementSyntax(SyntaxTree syntaxTree, Token keyword)
    : base(syntaxTree)
    {
        Keyword = keyword;
    }

    public override TokenType Type => TokenType.BreakStatement;
    public Token Keyword { get; }
}
