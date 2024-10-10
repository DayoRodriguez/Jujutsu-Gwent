using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class ExpressionStatementSyntax : StatementSyntax
{
    internal ExpressionStatementSyntax(SyntaxTree syntaxTree, ExpresionSyntax expression)
    : base(syntaxTree)
    {
        Expression = expression;
    }

    public override TokenType Type => TokenType.ExpressionStatement;
    public ExpresionSyntax Expression { get; }
}
