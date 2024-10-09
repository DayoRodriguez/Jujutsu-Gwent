using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;

public sealed class Token : SyntaxNode 
{
    public override TokenType Type{get;}
    public string Text{get;}
    public int Position{get;}
    public object Value{get;}
    public override TextSpan Span => new TextSpan(Position, Text.Length);
    public override TextSpan FullSpan
    {
        get
        {
            var start = LeadingTrivia.Count() == 0
                            ? Span.Start
                            : LeadingTrivia.First().Span.Start;
            var end = TrailingTrivia.Count() == 0
                            ? Span.End
                            : TrailingTrivia.Last().Span.End;
            return TextSpan.FromBounds(start, end);
        }
    }
    public bool IsMissing { get; }

    public Token(SyntaxTree syntaxTree, TokenType type, int position, string text, object value, IEnumerable<SyntaxTrivia> leadingTrivia, IEnumerable<SyntaxTrivia> trailingTrivia)
    : base (syntaxTree)
    {
        Type = type;
        Text = text;
        Value = value;
        IsMissing = text == null;
        Position = position;
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }

    public IEnumerable<SyntaxTrivia> LeadingTrivia { get;}
    public IEnumerable<SyntaxTrivia> TrailingTrivia { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Array.Empty<SyntaxNode>();
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
    CallExpression,
     // Statements
    BlockStatement,
    VariableDeclaration,
    IfStatement,
    WhileStatement,
    DoWhileStatement,
    ForStatement,
    BreakStatement,
    ContinueStatement,
    ReturnStatement,
    ExpressionStatement,

    SkippedTextTrivia,
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
    TrueKeyword,
    FalseKeyword, 
    BreakKeyword,
    ContinueKeyword,
    ElseKeyword,
    ForKeyword,
    FunctionKeyword,
    IfKeyword,
    ReturnKeyword,
    LetKeyword,
    ToKeyword,
    VarKeyword,
    DoKeyword,
    WhileKeyword,

    CompilationUnit,
    GlobalStatement,
    Parameter,
    TypeClause,
    FunctionDeclaration
}
