using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class WhileStatementSyntax : StatementSyntax
{
    internal WhileStatementSyntax(SyntaxTree syntaxTree, Token whileKeyword, ExpresionSyntax condition, StatementSyntax body)
    : base(syntaxTree)
    {
        WhileKeyword = whileKeyword;
        Condition = condition;
        Body = body;
    }

    public override TokenType Type => TokenType.WhileStatement;
    public Token WhileKeyword { get; }
    public ExpresionSyntax Condition { get; }
    public StatementSyntax Body { get; }
}
