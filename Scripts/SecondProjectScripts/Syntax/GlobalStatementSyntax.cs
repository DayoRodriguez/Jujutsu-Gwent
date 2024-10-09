using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatementSyntax : MemberSyntax
{
    internal GlobalStatementSyntax(SyntaxTree syntaxTree, StatementSyntax statement)
    : base(syntaxTree)
    {
        Statement = statement;
    }

    public override TokenType Type => TokenType.GlobalStatement;
    public StatementSyntax Statement { get; }
}
