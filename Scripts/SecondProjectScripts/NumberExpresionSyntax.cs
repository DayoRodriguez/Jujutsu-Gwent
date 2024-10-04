using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class NumberExpresionSyntax : ExpresionSyntax
{
    public Token NumberToken{get;}

    public NumberExpresionSyntax(Token numberToken)
    {
        NumberToken = numberToken;
    }

    public override TokenType Type => TokenType.NumberExpresion;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NumberToken;
    }
}
