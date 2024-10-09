using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameExpressionSyntax : ExpressionSyntax
{
    internal NameExpressionSyntax(SyntaxTree syntaxTree, Token identifierToken)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
        }

        public override TokenType Type => TokenType.NameExpression;
        public Token IdentifierToken { get; }
}
