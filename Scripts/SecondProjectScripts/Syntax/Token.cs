using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public sealed class Token : SyntaxNode 
{
    public override TokenType Type{get;}
    public string Text{get;}
    public int Position{get;}
    public object Value{get;}

    public Token(SyntaxTree syntaxTree, TokenType type, string text, int position, object value)
    : base (syntaxTree)
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
    WhiteEspaceToken, // " "    
    BadToken,
    EOF,
    Bang, // !
    Pipe, // |
    PipePipe, // ||
    PipeEqual, // |=
    Ampersand, //&
    AmpersandAmpersand, // &&
    AmpersandEqual, // &=
    OpenParen, // (
    CloseParen, // )
    Equal, // =
    EqualEqual, // == 
    BangEqual, // !=
    Hat, // ^
    HatEqual, // ^=
    Comma, // ,
    TwoPoints, // :
    Tilde, // ~
    OpenBrace, //{
    CloseBrace, //}

    //Operators
    Plus, // +
    PlusEqual, // +=
    Minus, // - 
    MinusEqual, // -=
    Star, // *
    StarEqual, // *=
    Slash, // /
    SlashEqual, // /=
    Greater, // >
    GreaterEqual, // >=
    Less, // <
    LessEqual, // <=
    //Expresion
    ParenExpression,
    UnaryExpresion,
    BinaryExpresion,
    LiteralExpresion,

    LineBreakTrivia,
    WhitespaceTrivia,
    SingleLineCommentTrivia,
    MultiLineCommentTrivia,
    //Literal
    Identifier, 
    StringLiteral, 
    NumberLiteral,
    Number,
    String, 
    Bool,

    //KeyWords 
    True,
    False, 
    BreakKeyword,
    ContinueKeyword,
    ElseKeyword,
    ForKeyword,
    FunctionKeyword,
    IfKeyword,
    ReturnKeyword,
    LetKeyword,
    ReturnKeyword,
    ToKeyword,
    VarKeyword,
    DoKeyword
}
