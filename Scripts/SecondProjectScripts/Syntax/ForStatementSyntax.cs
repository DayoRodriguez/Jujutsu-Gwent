using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForStatementSyntax : StatementSyntax
{
    internal ForStatementSyntax(SyntaxTree syntaxTree, Token keyword, Token identifier, Token equalsToken, ExpresionSyntax lowerBound, Token toKeyword, ExpresionSyntax upperBound, StatementSyntax body)
    : base(syntaxTree)
    {
        Keyword = keyword;
        Identifier = identifier;
        EqualsToken = equalsToken;
        LowerBound = lowerBound;
        ToKeyword = toKeyword;
        UpperBound = upperBound;
        Body = body;
    }

    public override TokenType Type => TokenType.ForStatement;
    public Token Keyword { get; }
    public Token Identifier { get; }
    public Token EqualsToken { get; }
    public ExpresionSyntax LowerBound { get; }
    public Token ToKeyword { get; }
    public ExpresionSyntax UpperBound { get; }
    public StatementSyntax Body { get; }
}

