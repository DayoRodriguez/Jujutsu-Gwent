using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class Lexer
{
    private readonly string text;
    private int position;
    private List<string> diagnostics = new List<string>();

    public IEnumerable<string> Diagnostics => diagnostics;

    private char Current
    {
        get
        {
            if(position >= text.Length) return '\0';
            return text[position];
        }
    }
    public Lexer(string input)
    {
        text = input;
    }

    private void Advance()
    {
        position++;
    }

    public Token Tokenize()
    {
        if(position >= text.Length)
        {
            return new Token(TokenType.EOF, "\0", position, null);
        }
        //numbers 
        if(char.IsDigit(Current))
        {
            var start = position;

            while(char.IsDigit(Current))
                Advance();

            var length = position - start;
            var _text = text.Substring(start, length);
            if(!int.TryParse(_text, out var value))
            {
                diagnostics.Add($"The number '{_text}' can not be represented by any Int32");
            }
            return new Token(TokenType.NumberLiteral, _text, start,  value);   
        }
        if(char.IsWhiteSpace(Current))
        {
            var start = position;

            while(char.IsWhiteSpace(Current))
                Advance();

            var length = position - start;
            var _text = text.Substring(start, length);
            return new Token(TokenType.WhiteEspaceToken, _text, start, null);
        }
        if(Current == '+')
            return new Token(TokenType.Plus, "+", position++, null);
        else if(Current == '-')
            return new Token(TokenType.Minus, "-", position++, null);
        else if(Current == '*')
            return new Token(TokenType.Star, "*", position++, null);
        else if(Current == '/')
            return new Token(TokenType.Slash, "/", position++, null);
        else if(Current == '(')
            return new Token(TokenType.OpenParen, "(", position++, null);
        else if(Current == ')')
            return new Token(TokenType.CloseParen, ")", position++, null); 

        diagnostics.Add($"ERREOR : bad charecter input : '{Current}'");
        return new Token(TokenType.BadToken, text.Substring(text[position-1],1), position++, null);                       
    }
}