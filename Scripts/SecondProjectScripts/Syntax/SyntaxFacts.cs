using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public static IEnumerable<TokenType> GetUnaryOperatorKinds()
        {
            var kinds = (TokenType[]) Enum.GetValues(typeof(TokenType));
            foreach (var kind in kinds)
            {
                if (GetUnaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static IEnumerable<TokenType> GetBinaryOperatorKinds()
        {
            var kinds = (TokenType[]) Enum.GetValues(typeof(TokenType));
            foreach (var kind in kinds)
            {
                if (GetBinaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

    public static TokenType GetKeyWordKind(string text)
    {
        switch(text)
        {
            case "break":
                return TokenType.BreakKeyword;
            case "continue":
                return TokenType.ContinueKeyword;
            case "else":
                return TokenType.ElseKeyword;
            case "false":
                return TokenType.FalseKeyword;
            case "for":
                return TokenType.ForKeyword;
            case "function":
                return TokenType.FunctionKeyword;
            case "if":
                return TokenType.IfKeyword;
            case "let":
                return TokenType.LetKeyword;
            case "return":
                return TokenType.ReturnKeyword;
            case "to":
                return TokenType.ToKeyword;
            case "true":
                return TokenType.TrueKeyword;
            case "var":
                return TokenType.VarKeyword;
            case "while":
                return TokenType.WhileKeyword;
            case "do":
                return TokenType.DoKeyword;
            default :
                return TokenType.Identifier;    
        }
    }

    public static bool IsComment(this TokenType kind)
    {
        return kind == TokenType.SingleLineCommentTrivia ||
                kind == TokenType.MultiLineCommentTrivia;
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

    public static bool IsTrivia(this TokenType kind)
        {
            switch (kind)
            {
                case TokenType.SkippedTextTrivia:
                case TokenType.LineBreakTrivia:
                case TokenType.WhitespaceTrivia:
                case TokenType.SingleLineCommentTrivia:
                case TokenType.MultiLineCommentTrivia:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsKeyword(this TokenType kind)
        {
            return kind.ToString().EndsWith("Keyword");
        }

        public static bool IsToken(this TokenType kind)
        {
            return !kind.IsTrivia() &&
                   (kind.IsKeyword() || kind.ToString().EndsWith("Token"));
        }

    public static TokenType GetBinaryOperatorOfAssignmentOperator(TokenType kind)
    {
        switch(kind)
        {
            case TokenType.PlusEqual:
                return TokenType.Plus;
            case TokenType.MinusEqual:
                return TokenType.Minus;
            case TokenType.StarEqual:
                return TokenType.Star;
            case TokenType.SlashEqual:
                return TokenType.Slash;
            case TokenType.AmpersandEqual:
                return TokenType.Ampersand;
            case TokenType.PipeEqual:
                return TokenType.Pipe;
            case TokenType.HatEqual:
                return TokenType.Hat;
            default:
                throw new Exception($"Unexpected syntax: '{kind}'");
        }
    }    
}
