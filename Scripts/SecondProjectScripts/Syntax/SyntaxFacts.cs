using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class SyntaxFacts 
{
    public static int GetUnaryOperatorPrecedence(this TokenType type)
    {
        switch(type)
        {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.Bang:
                return 6;

            default :
                return 0;     
        }
    }

    public static int GetBinaryOperatorPrecedence(this TokenType type)
    {
        switch(type)
        {
            case TokenType.Star :
            case TokenType.Slash:
                return 5;

            case TokenType.Plus:
            case TokenType.Minus:
                return 4;

            case TokenType.EqualEqual:
            case TokenType.BangEqual:
                return 3;

             
            case TokenType.AmpersandAmpersand:
                return 2;

            case TokenType.PipePipe:
                return 1;

            default :
                return 0;     
        }
    }

    public static TokenType GetKeyWordKind(string text)
    {
        switch(text)
        {
            case "true":
                return TokenType.True;
            case "False":
                return TokenType.False;
            default :
                return TokenType.Identifier;    
        }
    }

            public static string? GetText(TokenType kind)
        {
            switch (kind)
            {
                case TokenType.Plus:
                    return "+";
                 case TokenType.PlusEqual:
                    return "+=";
                case TokenType.Minus:
                    return "-";
                case TokenType.MinusEqual:
                    return "-=";
                case TokenType.Star:
                    return "*";
                case TokenType.StarEqual:
                    return "*=";
                case TokenType.Slash:
                    return "/";
                case TokenType.SlashEqual:
                    return "/=";
                case TokenType.Bang:
                    return "!";
                case TokenType.Equal:
                    return "=";
                case TokenType.Tilde:
                    return "~";
                case TokenType.Less:
                    return "<";
                case TokenType.LessEqual:
                    return "<=";
                case TokenType.Greater:
                    return ">";
                case TokenType.GreaterEqual:
                    return ">=";
                case TokenType.Ampersand:
                    return "&";
                case TokenType.AmpersandAmpersand:
                    return "&&";
                case TokenType.AmpersandEqual:
                    return "&=";
                case TokenType.Pipe:
                    return "|";
                case TokenType.PipeEqual:
                    return "|=";
                case TokenType.PipePipe:
                    return "||";
                case TokenType.Hat:
                    return "^";
                case TokenType.HatEqual:
                    return "^=";
                case TokenType.EqualEqual:
                    return "==";
                case TokenType.BangEqual:
                    return "!=";
                case TokenType.OpenParen:
                    return "(";
                case TokenType.CloseParen:
                    return ")";
                case TokenType.OpenBrace:
                    return "{";
                case TokenType.CloseBrace:
                    return "}";
                case TokenType.TwoPoints:
                    return ":";
                case TokenType.Comma:
                    return ",";
                case TokenType.BreakKeyword:
                    return "break";
                case TokenType.ContinueKeyword:
                    return "continue";
                case TokenType.ElseKeyword:
                    return "else";
                case TokenType.FalseKeyword:
                    return "false";
                case TokenType.ForKeyword:
                    return "for";
                case TokenType.FunctionKeyword:
                    return "function";
                case TokenType.IfKeyword:
                    return "if";
                case TokenType.LetKeyword:
                    return "let";
                case TokenType.ReturnKeyword:
                    return "return";
                case TokenType.ToKeyword:
                    return "to";
                case TokenType.TrueKeyword:
                    return "true";
                case TokenType.VarKeyword:
                    return "var";
                case TokenType.WhileKeyword:
                    return "while";
                case TokenType.DoKeyword:
                    return "do";
                default:
                    return null;
            }
        }
}
