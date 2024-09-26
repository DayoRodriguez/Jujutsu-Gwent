using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token 
{
    public TokenType type;
    public string value;

    public Token(TokenType type, string value)
    {
        this.type = type;
        this.value = value;
    }

    public List<Token> Tokenize(string input)
    {
        List<Token> tokens = new List<Token>();
        int index = 0;

        while(index < input.Length)
        {
            char current = input[index];

            if(char.IsWhiteSpace(current))
            {
                index++;
                continue;
            }
            //KeyWords
            if(char.IsLetter(current))
            {
                string word = "";
                
                while(index < input.Length && char.IsLetterOrDigit(input[index]))
                {
                    word += input[index];
                    index++;
                }
                if(word == "Param")
                {
                    tokens.Add(new Token(TokenType.Param, word));
                }
                else if(word == "Effect")
                {
                    tokens.Add(new Token(TokenType.Effect, word));
                }
                else if(word == "effect")
                {
                    tokens.Add(new Token(TokenType.effect, word));
                }
                else if(word == "Name")
                {
                    tokens.Add(new Token(TokenType.Name, word));
                }
                else if(word == "Number")
                {
                    tokens.Add(new Token(TokenType.Number, word));
                }
                else if(word == "String")
                {
                    tokens.Add(new Token(TokenType.String, word));
                }
                else if(word == "Bool")
                {
                    tokens.Add(new Token(TokenType.Bool, word));
                }
                else if(word == "Action")
                {
                    tokens.Add(new Token(TokenType.Action, word));
                }
                else if(word == "TriggerPlayer")
                {
                    tokens.Add(new Token(TokenType.TriggerPlayer, word));
                }
                else if(word == "Board")
                {
                    tokens.Add(new Token(TokenType.Board, word));
                }
                else if(word == "HandOfPlayer")
                {
                    tokens.Add(new Token(TokenType.HandOfPlayer, word));
                }
                else if(word == "FieldOfPlayer")
                {
                    tokens.Add(new Token(TokenType.FieldOfPlayer, word));
                }
                else if(word == "GraveyardOfPlayer")
                {
                    tokens.Add(new Token(TokenType.GraveyardOfPlayer, word));
                }
                else if(word == "DeckOfPlayer")
                {
                    tokens.Add(new Token(TokenType.DeckOfPlayer, word));
                }
                else if(word == "Hand")
                {
                    tokens.Add(new Token(TokenType.Hand, word));
                }
                else if(word == "Deck")
                {
                    tokens.Add(new Token(TokenType.Deck, word));
                }
                else if(word == "Field")
                {
                    tokens.Add(new Token(TokenType.Field, word));
                }
                else if(word == "Graveyard")
                {
                    tokens.Add(new Token(TokenType.Graveyard, word));
                }
                else if(word == "Owner")
                {
                    tokens.Add(new Token(TokenType.Owner, word));
                }
                else if(word == "Find")
                {
                    tokens.Add(new Token(TokenType.Find, word));
                }
                else if(word == "Push")
                {
                    tokens.Add(new Token(TokenType.Push, word));
                }
                else if(word == "SendBottom")
                {
                    tokens.Add(new Token(TokenType.SendBottom, word));
                }
                else if(word == "Pop")
                {
                    tokens.Add(new Token(TokenType.Pop, word));
                }
                else if(word == "Remove")
                {
                    tokens.Add(new Token(TokenType.Remove, word));
                }
                else if(word == "Shuffle")
                {
                    tokens.Add(new Token(TokenType.Shuffle, word));
                }
                else if(word == "Card")
                {
                    tokens.Add(new Token(TokenType.Card, word));
                }
                else if(word == "Type")
                {
                    tokens.Add(new Token(TokenType.Type, word));
                }
                else if(word == "Faction")
                {
                    tokens.Add(new Token(TokenType.Faction, word));
                }
                else if(word == "Power")
                {
                    tokens.Add(new Token(TokenType.Power, word));
                }
                else if(word == "Range")
                {
                    tokens.Add(new Token(TokenType.Range, word));
                }
                else if(word == "OnActivation")
                {
                    tokens.Add(new Token(TokenType.OnActivation, word));
                }
                else if(word == "Selector")
                {
                    tokens.Add(new Token(TokenType.Selector, word));
                }
                else if(word == "Source")
                {
                    tokens.Add(new Token(TokenType.Source, word));
                }
                else if(word == "Single")
                {
                    tokens.Add(new Token(TokenType.Single, word));
                }
                else if(word == "Predicate")
                {
                    tokens.Add(new Token(TokenType.Predicate, word));
                }
                else if(word == "PostAction")
                {
                    tokens.Add(new Token(TokenType.PostAction, word));
                }
                else if(word == "for")
                {
                    tokens.Add(new Token(TokenType.For, word));
                }
                else if(word == "while")
                {
                    tokens.Add(new Token(TokenType.While, word));
                }
                else if(word == "in")
                {
                    tokens.Add(new Token(TokenType.In, word));
                }
                else if(word == "true")
                {
                    tokens.Add(new Token(TokenType.True, word));
                }
                else if(word == "false")
                {
                    tokens.Add(new Token(TokenType.False, word));
                }
                else
                {
                    tokens.Add(new Token(TokenType.Identifier, word));
                }
            }

            //numbers
            else if(char.IsDigit(current))
            {
                string num = "";
                while(index < input.Length && char.IsDigit(input[index]))
                {
                    num += input[index];
                    index++;
                }
                tokens.Add(new Token(TokenType.NumberLiteral, num));
            }

            //strings
            else if(input[index] == '"')
            {
                string str = "";
                index++;
                while(index < input.Length && input[index] != '"')
                {
                    str += input[index];
                    index++;
                }
                tokens.Add(new Token(TokenType.StringLiteral, str));
            }

            //Simbols and operator
            else if(current == '(') 
                    tokens.Add(new Token(TokenType.OpenParen, "("));
            
            else if(current == ')') 
                    tokens.Add(new Token(TokenType.CloseParen, ")"));
            
            else if(current == '[') 
                    tokens.Add(new Token(TokenType.OpenBracket, "["));
            
            else if(current == ']') 
                    tokens.Add(new Token(TokenType.ClosedBracket, "]"));
            
            else if(current == '{') 
                    tokens.Add(new Token(TokenType.OpenBrace, "{"));
            
            else if(current == '}') 
                    tokens.Add(new Token(TokenType.ClosedBrace, "}"));
            
            else if(current == ':') 
                    tokens.Add(new Token(TokenType.AssignationParam, ":"));
            
            else if(current == ',') 
                    tokens.Add(new Token(TokenType.ValueSeparator, ","));
            
            else if(current == '(') 
                    tokens.Add(new Token(TokenType.OpenParen, "("));

            else if(current == '.')
                    tokens.Add(new Token(TokenType.Point, "."));

            else if(current == ';')
                    tokens.Add(new Token(TokenType.StatamentSeparator, ";"));

            else if(current == '%')
                    tokens.Add(new Token(TokenType.Rest, "%"));

            else if(current == '^')
                    tokens.Add(new Token(TokenType.Pow, "^"));

            else if(current == '+')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.PlusEqual, "+="));
                    index ++;
                }
                else if(input[index + 1] == '+')
                {
                    tokens.Add(new Token(TokenType.Increment, "++"));
                    index ++;
                }
                else 
                    tokens.Add(new Token(TokenType.Plus, "+"));
            }

            else if(current == '-')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.MinusEqual, "-="));
                    index ++;
                }
                else if(input[index + 1] == '-')
                {
                    tokens.Add(new Token(TokenType.Decrement, "--"));
                    index ++;
                }
                else 
                    tokens.Add(new Token(TokenType.Minus, "-"));
            }                                                    

            else if(current == '*')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.MulEqual, "*="));
                    index ++;
                }
                else 
                    tokens.Add(new Token(TokenType.Mul, "*"));
            }            

            else if(current == '/')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.DivEqual, "/="));
                    index++;
                }
                else 
                    tokens.Add(new Token(TokenType.Div, "/"));
            }

            else if(current == '=')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.EqualEqual, "=="));
                    index++;
                }
                else if(input[index + 1] == '>')
                {
                    tokens.Add(new Token(TokenType.Arrow, "=>"));
                    index++;
                }
                else 
                    tokens.Add(new Token(TokenType.Equal, "="));
            }

            else if(current == '>')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.GreaterEqual, ">="));
                    index++;
                }
                else 
                    tokens.Add(new Token(TokenType.Greater, ">"));
            }
            else if(current == '<')
            {
                if(input[index + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.LessEqual, "<="));
                    index ++;
                }
                else 
                    tokens.Add(new Token(TokenType.Less, "<"));
            }  

            else if(current == '@')
            {
                if(input[index + 1] == '@')
                {
                    tokens.Add(new Token(TokenType.DobleConcat, "@@"));
                    index ++;
                }
                else 
                    tokens.Add(new Token(TokenType.Concat, "@"));
            }     

            else if(current == '|' && index + 1 < input.Length && input[index + 1] == '|')
            {
                tokens.Add(new Token(TokenType.Or, "||"));
                index++;
            }
            else if(current == '&' && index + 1 < input.Length && input[index + 1] == '&')
            {
                tokens.Add(new Token(TokenType.And, "&&"));
                index++;
            }

            index++;
        }

        return tokens;
    }
}

public enum TokenType
{
    OpenParen, // (
    CloseParen, // )
    OpenBracket, // [
    ClosedBracket, // ]
    OpenBrace, // {
    ClosedBrace, // }
    AssignationParam, // :
    ValueSeparator, // ,
    Point, // .
    StatamentSeparator, // ;
    Rest, // %
    Pow, // ^
    Exclamation, // !
    ExclamationEqual, // !=
    Plus, // +
    PlusEqual, // +=
    Increment, // ++
    Minus, // - 
    MinusEqual, // -=
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
    DobleConcat, // @@
    Or, // ||
    And, // &&
    WhitesEspaceToken, // " "

    //Literal
    Identifier, 
    StringLiteral, 
    NumberLiteral,
    True,
    False,

    //KeyWords
    //effect
    effect,
    Name,
    Param,
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

    //Card
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

    //
    For,
    While,
    In,
    EOF
}
