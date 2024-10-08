using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class Lexer
{
    private readonly string text;
    private readonly SyntaxTree _syntaxTree;
    private readonly SourceText _text;
    private int position;
    private List<string> diagnostics = new List<string>();

    private int _start;
    private TokenType _type;
    private object? _value;
    private IEnumerable<SyntaxTrivia>.Builder _triviaBuilder = IEnumerable.CreateBuilder<SyntaxTrivia>();
    public IEnumerable<string> Diagnostics => diagnostics;

    private char Current => Peek(0);
    private char Lookahead => Peek(1);
    public Lexer(string input)
    {
        text = input;
    }

    private char Peek(int offset)
    {
        var index = position + offset;

        if(index >= text.Length)
            return '\0';

        return text[index];    
    }

    private void Advance()
    {
        position++;
    }

    public Token Tokenize()
    {
        ReadTrivia(leading: true);

        var leadingTrivia = _triviaBuilder.ToImmutable();
        var tokenStart = position;

        ReadToken();

        var tokenKind = _type;
        var tokenValue = _value;
        var tokenLength = position - _start;

        ReadTrivia(leading: false);

        var trailingTrivia = _triviaBuilder.ToImmutable();

        var tokenText = SyntaxFacts.GetText(tokenKind);
        if (tokenText == null)
           tokenText = _text.ToString(tokenStart, tokenLength);

        return new Token(_syntaxTree, tokenKind, tokenStart, tokenText, tokenValue, leadingTrivia, trailingTrivia);
                       
    }
        private void ReadTrivia(bool leading)
        {
            _triviaBuilder.Clear();

            var done = false;

            while (!done)
            {
                _start = position;
                _type = TokenType.BadToken;
                _value = null;

                switch (Current)
                {
                    case '\0':
                        done = true;
                        break;
                    case '/':
                        if (Lookahead == '/')
                        {
                            ReadSingleLineComment();
                        }
                        else if (Lookahead == '*')
                        {
                            ReadMultiLineComment();
                        }
                        else
                        {
                            done = true;
                        }
                        break;
                    case '\n':
                    case '\r':
                        if (!leading)
                            done = true;
                        ReadLineBreak();
                        break;
                    case ' ':
                    case '\t':
                        ReadWhiteSpace();
                        break;
                    default:
                        if (char.IsWhiteSpace(Current))
                            ReadWhiteSpace();
                        else
                            done = true;
                        break;
                }

                var length = position - _start;
                if (length > 0)
                {
                    var text = _text.ToString(_start, length);
                    var trivia = new SyntaxTrivia(_syntaxTree, _type, _start, text);
                    _triviaBuilder.Add(trivia);
                }
            }
        }

        private void ReadLineBreak()
        {
            if (Current == '\r' && Lookahead == '\n')
            {
                position += 2;
            }
            else
            {
                position++;
            }

            _type = TokenType.LineBreakTrivia;
        }

        private void ReadWhiteSpace()
        {
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        done = true;
                        break;
                    default:
                        if (!char.IsWhiteSpace(Current))
                            done = true;
                        else
                            position++;
                        break;
                }
            }

            _type = TokenType.WhitespaceTrivia;
        }


        private void ReadSingleLineComment()
        {
            position += 2;
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        done = true;
                        break;
                    default:
                        position++;
                        break;
                }
            }

            _type = TokenType.SingleLineCommentTrivia;
        }

        private void ReadMultiLineComment()
        {
            position += 2;
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                        var span = new TextSpan(_start, 2);
                        var location = new TextLocation(_text, span);
                        diagnostics.ReportUnterminatedMultiLineComment(location);
                        done = true;
                        break;
                    case '*':
                        if (Lookahead == '/')
                        {
                            position++;
                            done = true;
                        }
                        position++;
                        break;
                    default:
                        position++;
                        break;
                }
            }

            _type = TokenType.MultiLineCommentTrivia;
        }

        private void ReadToken()
        {
            _start = position;
            _type = TokenType.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    _type = TokenType.EOF;
                    break;
                case '+':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Plus;
                    }
                    else
                    {
                        _type = TokenType.PlusEqual;
                        position++;
                    }
                    break;
                case '-':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Minus;
                    }
                    else
                    {
                        _type = TokenType.MinusEqual;
                        position++;
                    }
                    break;
                case '*':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Star;
                    }
                    else
                    {
                        _type = TokenType.StarEqual;
                        position++;
                    }
                    break;
                case '/':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Slash;
                    }
                    else
                    {
                        _type = TokenType.SlashEqual;
                        position++;
                    }
                    break;
                case '(':
                    _type = TokenType.OpenParen;
                    position++;
                    break;
                case ')':
                    _type = TokenType.CloseParen;
                    position++;
                    break;
                case '{':
                    _type = TokenType.OpenBrace;
                    position++;
                    break;
                case '}':
                    _type = TokenType.CloseBrace;
                    position++;
                    break;
                case ':':
                    _type = TokenType.TwoPoints;
                    position++;
                    break;
                case ',':
                    _type = TokenType.Comma;
                    position++;
                    break;
                case '~':
                    _type = TokenType.Tilde;
                    position++;
                    break;
                case '^':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Hat;
                    }
                    else
                    {
                        _type = TokenType.HatEqual;
                        position++;
                    }
                    break;
                case '&':
                    position++;
                    if (Current == '&')
                    {
                        _type = TokenType.AmpersandAmpersand;
                        position++;
                    }
                    else if (Current == '=')
                    {
                        _type = TokenType.AmpersandEqual;
                        position++;
                    }
                    else
                    {
                        _type = TokenType.Ampersand;
                    }
                    break;
                case '|':
                    position++;
                    if (Current == '|')
                    {
                        _type = TokenType.PipePipe;
                        position++;
                    }
                    else if (Current == '=')
                    {
                        _type = TokenType.PipeEqual;
                        position++;
                    }
                    else
                    {
                        _type = TokenType.Pipe;
                    }
                    break;
                case '=':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Equal;
                    }
                    else
                    {
                        _type = TokenType.EqualEqual;
                        position++;
                    }
                    break;
                case '!':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Bang;
                    }
                    else
                    {
                        _type = TokenType.BangEqual;
                        position++;
                    }
                    break;
                case '<':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Less;
                    }
                    else
                    {
                        _type = TokenType.LessEqual;
                        position++;
                    }
                    break;
                case '>':
                    position++;
                    if (Current != '=')
                    {
                        _type = TokenType.Greater;
                    }
                    else
                    {
                        _type = TokenType.GreaterEqual;
                        position++;
                    }
                    break;
                case '"':
                    ReadString();
                    break;
                case '0': case '1': case '2': case '3': case '4':
                case '5': case '6': case '7': case '8': case '9':
                    ReadNumber();
                    break;
                case '_':
                    ReadIdentifierOrKeyword();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    }
                    else
                    {
                        var span = new TextSpan(position, 1);
                        var location = new TextLocation(_text, span);
                        diagnostics.ReportBadCharacter(location, Current);
                        position++;
                    }
                    break;
            }
        }

        private void ReadString()
        {
            // Skip the current quote
            position++;

            var sb = new StringBuilder();
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        var span = new TextSpan(_start, 1);
                        var location = new TextLocation(_text, span);
                        diagnostics.ReportUnterminatedString(location);
                        done = true;
                        break;
                    case '"':
                        if (Lookahead == '"')
                        {
                            sb.Append(Current);
                            position += 2;
                        }
                        else
                        {
                            position++;
                            done = true;
                        }
                        break;
                    default:
                        sb.Append(Current);
                        position++;
                        break;
                }
            }

            _type = TokenType.String;
            _value = sb.ToString();
        }

        private void ReadNumber()
        {
            while (char.IsDigit(Current))
                position++;

            var length = position - _start;
            var text = _text.ToString(_start, length);
            if (!int.TryParse(text, out var value))
            {
                var span = new TextSpan(_start, length);
                var location = new TextLocation(_text, span);
                diagnostics.ReportInvalidNumber(location, text, TypeSymbol.Int);
            }

            _value = value;
            _type = TokenType.Number;
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetterOrDigit(Current) || Current == '_')
                position++;

            var length = position - _start;
            var text = _text.ToString(_start, length);
            _type = SyntaxFacts.GetKeywordKind(text);
    }
}


