using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Atom : IExpression
{
    public static readonly List<TokenType> moduleTypes = new List<TokenType>(){
        TokenType.ClosedBracket,TokenType.ClosedBrace, TokenType.Arrow, TokenType.StatementSeparator,
        TokenType.AssignParams,TokenType.ValueSeparator, TokenType.Equal, TokenType.AddEqual, TokenType.MulEqual,
        TokenType.For, TokenType.While, TokenType.SubEqual, TokenType.DivEqual,
    };
    public abstract object Evaluate(Context context, List<Card> targets);
}