using System.Collections.Generic;
using UnityEngine;

public class Token
{
    public TokenType type;
    public string lexeme;
    public object literal;
    public int line;
    public int column;

    // Constructor
    public Token(TokenType type, string lexeme, object literal, int line, int column)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
        this.column = column;
    }
    
    public override string ToString()
    {
        string res = $"{type.ToString()} {lexeme}";
        if (literal != null) res += " " + literal.ToString();
        res += $" [ln {line}, col {column}]";
        return res;
    }
}

public enum TokenType
{
    OpenParen, // ( 
    ClosedParen, // )
    OpenBracket, // [
    ClosedBracket, // ]
    OpenBrace, // {
    ClosedBrace, // }
    ValueSeparator, // , 
    Dot, // .
    AssignParams, // :
    StatementSeparator, // ;
    Mod, // %
    Pow, // ^    
    Exclamation, // !
    ExclamationEqual, // !=
    Add, // +
    AddEqual, // +=
    Increment, // ++
    Sub, // -
    SubEqual, // -=
    Decrement, // --
    Mul, // *
    MulEqual, // *=
    Div, // / 
    DivEqual, // /=
    Equal, // =
    EqualEqual, // ==
    Arrow, // =>
    Greater, // >
    GreaterEqual, // >=
    Less, // <
    LessEqual, // <=
    Concat, // @ 
    ConcatConcat, // @@ 
    ConcatEqual, // @=
    Or, // ||
    And, // &&

    // Literales
    Identifier, 
    StringLiteral, 
    NumberLiteral, 
    True, 
    False,

    // Keywords
    //effect
    effect,
    Name,         
    Params,         
    Number,         
    String,         
    Bool,         
    Action,            
    TriggerPlayer,         
    Board,         
    HandOfPlayer,         
    FieldOfPlayer,         
    GraveyardOfPlayer,         
    DeckOfPlayer,         
    Hand,         
    Deck,         
    Field,         
    Graveyard,         
    Owner,         
    Find,         
    Push,         
    SendBottom,         
    Pop,         
    Remove,         
    Shuffle,        

    //card
    Card,         
    Type,         
    Faction,       
    Power,         
    Range,         
    OnActivation,   
    Effect,         
    Selector,       
    Source,         
    Single,         
    Predicate,         
    PostAction,         
    
    //-------------
    For,
    While,
    In,
    EOF
}