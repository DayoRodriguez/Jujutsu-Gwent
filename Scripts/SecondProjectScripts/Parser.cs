using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
class Parser
{
    private readonly Token[] _tokens;
    private int position;

    private List<string> diagnostics = new List<string>();

    public Parser(string text)
    {
        var tokens = new List<Token>();
        var lexer = new Lexer(text);
        Token token;

        do
        {
            token = lexer.Tokenize();
            if(token.Type != TokenType.WhiteEspaceToken || token.Type != TokenType.BadToken)
                tokens.Add(token);
        }while(token.Type != TokenType.EOF);

        _tokens = tokens.ToArray();
        diagnostics.AddRange(lexer.Diagnostics);
    }

    public IEnumerable<string> Diagnostics => diagnostics;

    private Token Peek(int offset)
    {
        var index = position + offset;
        if(index >= _tokens.Length)
            return _tokens[_tokens.Length-1];

        return _tokens[index];    
    }

    private Token Current => Peek(0);
        
    private Token NextToken()
    {
        var current = Current;
        position++;
        return current;
    }

    private Token Match(TokenType type)
    {
        if(Current.Type == type)
            return NextToken();
        
        diagnostics.Add($"ERROR : Unexpected token<{Current.Type}>, expected<{type}>");
        return new Token(type, null, Current.Position, null);   
    }

    private ExpresionSyntax ParseExpresion()
    {
        return ParseTerm();
    }

    public SyntaxTree Parse()
    {
        var expresion = ParseTerm ();
        var endOfFileToken = Match(TokenType.EOF);
        return new SyntaxTree(diagnostics, expresion, endOfFileToken);
    }
    public ExpresionSyntax ParseTerm()
    {
        var left = ParseFactor();

        while(Current.Type == TokenType.Plus || Current.Type == TokenType.Minus)
        {
            var operatorToken = NextToken();
            var right = ParseFactor();
            left = new BinaryExpresionSyntax(left, operatorToken, right);
        }

        return left;
    }

    public ExpresionSyntax ParseFactor()
    {
        var left = ParsePrimaryExpresion();

        while(Current.Type == TokenType.Star || Current.Type == TokenType.Slash)
        {
            var operatorToken = NextToken();
            var right = ParsePrimaryExpresion();
            left = new BinaryExpresionSyntax(left, operatorToken, right);
        }

        return left;
    }

    private ExpresionSyntax ParsePrimaryExpresion()
    {
        if(Current.Type == TokenType.OpenParen)
        {
            var left = NextToken();
            var expresion = ParseExpresion();
            var right = Match(TokenType.CloseParen);

            return new ParenthesizedExpressionSyntax(left, expresion, right);
        }
        var numberToken = Match(TokenType.NumberLiteral);
        return new NumberExpresionSyntax(numberToken);
    }
}
