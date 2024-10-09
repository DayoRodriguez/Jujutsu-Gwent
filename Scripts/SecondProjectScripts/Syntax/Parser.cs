using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;


internal sealed class Parser
{
    private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();
    private readonly SyntaxTree _syntaxTree;
    private readonly SourceText _text;
    private readonly IEnumerable<Token> _tokens;
    private int _position;

    public Parser(SyntaxTree syntaxTree)
    {
        var tokens = new List<Token>();
        var badTokens = new List<Token>();

        var lexer = new Lexer(syntaxTree);
        Token token;
        do
        {
            token = lexer.Tokenize();

            if (token.Type == TokenType.BadToken)
            {
                badTokens.Add(token);
            }
            else
            {
                if (badTokens.Count > 0)
                {
                    var leadingTrivia = token.LeadingTrivia.ToBuilder();
                    var index = 0;

                    foreach (var badToken in badTokens)
                    {
                        foreach (var lt in badToken.LeadingTrivia)
                            leadingTrivia.Insert(index++, lt);

                        var trivia = new SyntaxTrivia(syntaxTree, TokenType.SkippedTextTrivia, badToken.Position, badToken.Text);
                        leadingTrivia.Insert(index++, trivia);

                        foreach (var tt in badToken.TrailingTrivia)
                            leadingTrivia.Insert(index++, tt);
                    }

                    badTokens.Clear();
                    token =  new Token(token.SyntaxTree, token.Type, token.Position, token.Text, token.Value, leadingTrivia.ToImmutable(), token.TrailingTrivia);
                }

                tokens.Add(token);
            }
        } while (token.Type != TokenType.EOF);

        _syntaxTree = syntaxTree;
        _text = syntaxTree.Text;
        _tokens = tokens.ToList();
        _diagnostics.AddRange(lexer.Diagnostics);
    }

    public DiagnosticsBag Diagnostics => _diagnostics;

    private Token Peek(int offset)
    {
        var index = _position + offset;
        if (index >= _tokens.Length)
            return _tokens[_tokens.Length - 1];

        return _tokens[index];
    }

    private Token Current => Peek(0);

    private Token NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private Token MatchToken(TokenType Type)
    {
        if (Current.Type == Type)
            return NextToken();

        _diagnostics.ReportUnexpectedToken(Current.Location, Current.Type, Type);
        return new Token(_syntaxTree, Type, Current.Position, null, null, IEnumerable<SyntaxTrivia>.Empty, IEnumerable<SyntaxTrivia>.Empty);
    }

    public CompilationUnitSyntax ParseCompilationUnit()
    {
        var members = ParseMembers();
        var endOfFileToken = MatchToken(TokenType.EOF);
        return new CompilationUnitSyntax(_syntaxTree, members, endOfFileToken);
    }

    private MemberSyntax ParseMember()
    {
        if (Current.Type == TokenType.FunctionKeyword)
            return ParseFunctionDeclaration();

        return ParseGlobalStatement();
    }

    private MemberSyntax ParseFunctionDeclaration()
    {
        var functionKeyword = MatchToken(TokenType.FunctionKeyword);
        var identifier = MatchToken(TokenType.Identifier);
        var openParenthesisToken = MatchToken(TokenType.OpenParen);
        var parameters = ParseParameterList();
        var closeParenthesisToken = MatchToken(TokenType.CloseParen);
        var type = ParseOptionalTypeClause();
        var body = ParseBlockStatement();
        return new FunctionDeclarationSyntax(_syntaxTree, functionKeyword, identifier, openParenthesisToken, parameters, closeParenthesisToken, type, body);
    }

    private SeparatedSyntaxList<ParameterSyntax> ParseParameterList()
    {
        var nodesAndSeparators = IEnumerable.CreateBuilder<SyntaxNode>();

        var parseNextParameter = true;
        while (parseNextParameter &&
               Current.Type != TokenType.CloseParen &&
                Current.Type != TokenType.EOF)
        {
            var parameter = ParseParameter();
            nodesAndSeparators.Add(parameter);

            if (Current.Type == TokenType.Comma)
            {
                var comma = MatchToken(TokenType.Comma);
                nodesAndSeparators.Add(comma);
            }
            else
            {
                parseNextParameter = false;
            }
        }

        return new SeparatedSyntaxList<ParameterSyntax>(nodesAndSeparators.ToImmutable());
    }

    private ParameterSyntax ParseParameter()
    {
        var identifier = MatchToken(TokenType.Identifier);
        var type = ParseTypeClause();
        return new ParameterSyntax(_syntaxTree, identifier, type);
    }

    private MemberSyntax ParseGlobalStatement()
    {
        var statement = ParseStatement();
        return new GlobalStatementSyntax(_syntaxTree, statement);
    }

    private StatementSyntax ParseStatement()
    {
        switch (Current.Type)
        {
            case TokenType.OpenBrace:
                return ParseBlockStatement();
            case TokenType.LetKeyword:
            case TokenType.VarKeyword:
                return ParseVariableDeclaration();
            case TokenType.IfKeyword:
                return ParseIfStatement();
            case TokenType.WhileKeyword:
                return ParseWhileStatement();
            case TokenType.DoKeyword:
                return ParseDoWhileStatement();
            case TokenType.ForKeyword:
                return ParseForStatement();
            case TokenType.BreakKeyword:
                return ParseBreakStatement();
            case TokenType.ContinueKeyword:
                return ParseContinueStatement();
            case TokenType.ReturnKeyword:
                return ParseReturnStatement();
            default:
                return ParseExpressionStatement();
        }
    }

    private BlockStatementSyntax ParseBlockStatement()
    {
        var statements = IEnumerable.CreateBuilder<StatementSyntax>();

        var openBraceToken = MatchToken(TokenType.OpenBrace);

        while (Current.Type != TokenType.EOF &&
               Current.Type != TokenType.CloseBrace)
        {
            var startToken = Current;

            var statement = ParseStatement();
            statements.Add(statement);

                // If ParseStatement() did not consume any tokens,
                // we need to skip the current token and continue
                // in order to avoid an infinite loop.
                //
                // We don't need to report an error, because we'll
                // already tried to parse an expression statement
                // and reported one.
            if (Current == startToken)
            NextToken();
        }

        var closeBraceToken = MatchToken(TokenType.CloseBrace);

        return new BlockStatementSyntax(_syntaxTree, openBraceToken, statements.ToImmutable(), closeBraceToken);
    }

    private StatementSyntax ParseVariableDeclaration()
    {
        var expected = Current.Type == TokenType.LetKeyword ? TokenType.LetKeyword : TokenType.VarKeyword;
        var keyword = MatchToken(expected);
        var identifier = MatchToken(TokenType.Identifier);
        var typeClause = ParseOptionalTypeClause();
        var equals = MatchToken(TokenType.Equal);
        var initializer = ParseExpression();
        return new VariableDeclarationSyntax(_syntaxTree, keyword, identifier, typeClause, equals, initializer);
    }

    private TypeClauseSyntax? ParseOptionalTypeClause()
    {
        if (Current.Type != TokenType.ColonToken)
            return null;

        return ParseTypeClause();
    }

    private TypeClauseSyntax ParseTypeClause()
    {
        var colonToken = MatchToken(TokenType.ColonToken);
        var identifier = MatchToken(TokenType.Identifier);
        return new TypeClauseSyntax(_syntaxTree, colonToken, identifier);
    }

    private StatementSyntax ParseIfStatement()
    {
        var keyword = MatchToken(TokenType.IfKeyword);
        var condition = ParseExpression();
        var statement = ParseStatement();
        var elseClause = ParseOptionalElseClause();
        return new IfStatementSyntax(_syntaxTree, keyword, condition, statement, elseClause);
    }

    private ElseClauseSyntax? ParseOptionalElseClause()
    {
        if (Current.Type != TokenType.ElseKeyword)
            return null;

        var keyword = NextToken();
        var statement = ParseStatement();
        return new ElseClauseSyntax(_syntaxTree, keyword, statement);
    }

    private StatementSyntax ParseWhileStatement()
    {
        var keyword = MatchToken(TokenType.WhileKeyword);
        var condition = ParseExpression();
        var body = ParseStatement();
        return new WhileStatementSyntax(_syntaxTree, keyword, condition, body);
    }

    private StatementSyntax ParseDoWhileStatement()
    {
        var doKeyword = MatchToken(TokenType.DoKeyword);
        var body = ParseStatement();
        var whileKeyword = MatchToken(TokenType.WhileKeyword);
        var condition = ParseExpression();
        return new DoWhileStatementSyntax(_syntaxTree, doKeyword, body, whileKeyword, condition);
    }

    private StatementSyntax ParseForStatement()
    {
        var keyword = MatchToken(TokenType.ForKeyword);
        var identifier = MatchToken(TokenType.Identifier);
        var equalsToken = MatchToken(TokenType.Equal);
        var lowerBound = ParseExpression();
        var toKeyword = MatchToken(TokenType.ToKeyword);
        var upperBound = ParseExpression();
        var body = ParseStatement();
        return new ForStatementSyntax(_syntaxTree, keyword, identifier, equalsToken, lowerBound, toKeyword, upperBound, body);
    }

    private StatementSyntax ParseBreakStatement()
    {
        var keyword = MatchToken(TokenType.BreakKeyword);
        return new BreakStatementSyntax(_syntaxTree, keyword);
    }

    private StatementSyntax ParseContinueStatement()
    {
        var keyword = MatchToken(TokenType.ContinueKeyword);
        return new ContinueStatementSyntax(_syntaxTree, keyword);
    }

    private StatementSyntax ParseReturnStatement()
    {
        var keyword = MatchToken(TokenType.ReturnKeyword);
        var keywordLine = _text.GetLineIndex(keyword.Span.Start);
        var currentLine = _text.GetLineIndex(Current.Span.Start);
        var isEof = Current.Type == TokenType.EOF;
        var sameLine = !isEof && keywordLine == currentLine;
        var expression = sameLine ? ParseExpression() : null;
        return new ReturnStatementSyntax(_syntaxTree, keyword, expression);
    }

    private ExpressionStatementSyntax ParseExpressionStatement()
    {
        var expression = ParseExpression();
        return new ExpressionStatementSyntax(_syntaxTree, expression);
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private ExpressionSyntax ParseAssignmentExpression()
    {
        if (Peek(0).Type == TokenType.Identifier)
        {
            switch (Peek(1).Type)
            {
                case TokenType.PlusEqual:
                case TokenType.MinusEqual:
                case TokenType.StarEqual:
                case TokenType.SlashEqual:
                case TokenType.AmpersandEqual:
                case TokenType.PipeEqual:
                case TokenType.HatEqual:
                case TokenType.Equal:
                    var identifierToken = NextToken();
                    var operatorToken = NextToken();
                    var right = ParseAssignmentExpression();
                    return new AssignmentExpressionSyntax(_syntaxTree, identifierToken, operatorToken, right);
            }

        }
        return ParseBinaryExpression();
    }
    private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left;
        var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseBinaryExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(_syntaxTree, operatorToken, operand);
        }
        else
        {
            left = ParsePrimaryExpression();
        }

        while (true)
        {
            var precedence = Current.Type.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
                break;

            var operatorToken = NextToken();
            var right = ParseBinaryExpression(precedence);
            left = new BinaryExpresionSyntax(_syntaxTree, left, operatorToken, right);
        }

        return left;
    }

    private ExpresionSyntax ParsePrimaryExpression()
    {
        switch (Current.Type)
        {
            case TokenType.OpenParen:
                return ParseParenthesizedExpression();

            case TokenType.FalseKeyword:
            case TokenType.TrueKeyword:
                return ParseBooleanLiteral();

            case TokenType.Number:
                return ParseNumberLiteral();

            case TokenType.String:
                return ParseStringLiteral();

            case TokenType.Identifier:
            default:
                return ParseNameOrCallExpression();
        }
    }

    private ExpressionSyntax ParseParenthesizedExpression()
    {
        var left = MatchToken(TokenType.OpenParen);
        var expression = ParseExpression();
        var right = MatchToken(TokenType.CloseParen);
        return new ParenthesizedExpressionSyntax(_syntaxTree, left, expression, right);
    }

    private ExpresionSyntax ParseBooleanLiteral()
    {
        var isTrue = Current.Type == TokenType.TrueKeyword;
        var keywordToken = isTrue ? MatchToken(TokenType.TrueKeyword) : MatchToken(TokenType.FalseKeyword);
        return new LiteralExpresionSyntax(_syntaxTree, keywordToken, isTrue);
    }

    private ExpresionSyntax ParseNumberLiteral()
    {
        var numberToken = MatchToken(TokenType.NumberLiteral);
        return new LiteralExpresionSyntax(_syntaxTree, numberToken);
    }

    private ExpresionSyntax ParseStringLiteral()
    {
        var stringToken = MatchToken(TokenType.StringLiteral);
        return new LiteralExpresionSyntax(_syntaxTree, stringToken);
    }

    private ExpresionSyntax ParseNameOrCallExpression()
    {
        if (Peek(0).Type == TokenType.Identifier && Peek(1).Type == TokenType.OpenParen)
            return ParseCallExpression();

        return ParseNameExpression();
    }

    private ExpresionSyntax ParseCallExpression()
    {
        var identifier = MatchToken(TokenType.Identifier);
        var openParenthesisToken = MatchToken(TokenType.OpenParen);
        var arguments = ParseArguments();
        var closeParenthesisToken = MatchToken(TokenType.CloseParen);
        return new CallExpressionSyntax(_syntaxTree, identifier, openParenthesisToken, arguments, closeParenthesisToken);
    }

    private SeparatedSyntaxList<ExpressionSyntax> ParseArguments()
    {
        var nodesAndSeparators = IEnumerable.CreateBuilder<SyntaxNode>();

        var parseNextArgument = true;
        while (parseNextArgument &&
                Current.Type != TokenType.CloseParen &&
                Current.Type != TokenType.EOF)
        {
            var expression = ParseExpression();
            nodesAndSeparators.Add(expression);

            if (Current.Type == TokenType.Comma)
            {
                var comma = MatchToken(TokenType.Comma);
                nodesAndSeparators.Add(comma);
            }
            else
            {
                parseNextArgument = false;
            }
        }

        return new SeparatedSyntaxList<ExpresionSyntax>(nodesAndSeparators.ToIenumrator());
    }

    private ExpresionSyntax ParseNameExpression()
    {
        var identifierToken = MatchToken(TokenType.Identifier);
        return new NameExpressionSyntax(_syntaxTree, identifierToken);
    }
}
