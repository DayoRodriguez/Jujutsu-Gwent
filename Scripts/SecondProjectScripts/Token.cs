using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

class Token : SyntaxNode 
{
    public override TokenType Type{get;}
    public string Text{get;}
    public int Position{get;}
    public object Value{get;}

    public Token(TokenType type, string text, int position, object value)
    {
        Type = type;
        Text = text;
        Value = value;
        Position = position;
    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>();
    }
}

public enum TokenType
{
    // Symbol,
    // Operator,
    ParenExpression,
    BinaryExpresion,
    NumberExpresion,
    BadToken,
    OpenParen, // (
    CloseParen, // )
    // OpenBracket, // [
    // ClosedBracket, // ]
    // OpenBrace, // {
    // ClosedBrace, // }
    // AssignationParam, // :
    // ValueSeparator, // ,
    // Point, // .
    // StatamentSeparator, // ;
    // Rest, // %
    // Pow, // ^
    // Exclamation, // !
    // ExclamationEqual, // !=
    Plus, // +
    // PlusEqual, // +=
    // Increment, // ++
     Minus, // - 
    // MinusEqual, // -=
    // Decrement, // --
    Star, // *
    // MulEqual, // *=
    Slash, // /
    // DivEqual, // /=
    // Equal, // =
    // EqualEqual, // ==
    // Arrow, // =>
    // Greater, // >
    // GreaterEqual, // >=
    // Less, // < 
    // LessEqual, // <=
    // Concat, // @
    // DobleConcat, // @@
    // Or, // ||
    // And, // &&
    WhiteEspaceToken, // " "

    //Literal
    Identifier, 
    StringLiteral, 
    NumberLiteral,
    True,
    False,

    //KeyWords
    //effect
    KeyWord,
    // effect,
    // Name,
    // Params,
    Number,
    String, 
    Bool, 
    // Action,
    // targets,
    // context,
    // TriggerPlayer,
    // Board,
    // HandOfPlayer,
    // FieldOfPlayer,
    // GraveyardOfPlayer,
    // DeckOfPlayer,
    // Hand,
    // Deck,
    // Field,
    // Graveyard,
    // Owner,
    // Find,
    // Push,
    // SendBottom,
    // Pop,
    // Remove,
    // Shuffle,

    //Card
    // Card,
    // Image,
    // Type,
    // Faction,
    // Power,
    // Range,
    // OnActivation,
    // Effect,
    // Selector,
    // Source,
    // Single,
    // Predicate,
    // PostAction,

    //
    // For,
    //Foreach,
    // While,
    // In,
    EOF
}
