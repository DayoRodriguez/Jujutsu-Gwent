using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


internal sealed class Parser
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

    private ExpresionSyntax ParseExpresion(int parentPrecedence = 0)
    {
        ExpresionSyntax left;
        var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();

        if(unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseExpresion(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else 
        {
            left = ParsePrimaryExpresion();
        }

        while(true)
        {
            var precedence = Current.Type.GetBinaryOperatorPrecedence();
            if(precedence == 0 || precedence <= parentPrecedence)
                break;

            var operatorToken = NextToken();
            var right = ParseExpresion();
            left = new BinaryExpresionSyntax(left, operatorToken, right);        
        }
        return left;
    }

    private Token MatchToken(TokenType type)
    {
        if(Current.Type == type)
            return NextToken();
        
        diagnostics.Add($"ERROR : Unexpected token<{Current.Type}>, expected<{type}>");
        return new Token(type, null, Current.Position, null);   
    }

    
    public SyntaxTree Parse()
    {
        var expresion = ParseExpresion();
        var endOfFileToken = MatchToken(TokenType.EOF);
        return new SyntaxTree(diagnostics, expresion, endOfFileToken);
    }

    private ExpresionSyntax ParsePrimaryExpresion()
    {
        switch(Current.Type)
        {
            case TokenType.OpenParen:
            {
                var left = NextToken();
                var expresion = ParseExpresion();
                var right = MatchToken(TokenType.CloseParen);

                return new ParenthesizedExpressionSyntax(left, expresion, right);
            }
            case TokenType.False:
            case TokenType.True:
            {
                var keyWordToken = NextToken();
                var value = keyWordToken.Type == TokenType.True;
                return new LiteralExpresionSyntax(keyWordToken, value);
            }
            default:
            {
                var numberToken = MatchToken(TokenType.NumberLiteral);
                return new LiteralExpresionSyntax(numberToken);
            }    
        }
    }
}
