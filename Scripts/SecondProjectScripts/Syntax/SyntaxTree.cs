using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Collections.ObjectModel;

public sealed class SyntaxTree 
{
    private Dictionary<SyntaxNode, SyntaxNode?>? _parents;

    private delegate void ParseHandler(SyntaxTree syntaxTree,
                                       out CompilationUnitSyntax root,
                                       out IEnumerable<Diagnostics> diagnostics);

    private SyntaxTree(SourceText text, ParseHandler handler)
    {
        Text = text;

        handler(this, out var root, out var diagnostics);

        Diagnostics = diagnostics;
        Root = root;
    }

    public SourceText Text { get; }
    public IEnumerable<Diagnostics> Diagnostics { get; }
    public CompilationUnitSyntax Root { get; }

    public static SyntaxTree Load(string fileName)
    {
        var text = File.ReadAllText(fileName);
        var sourceText = SourceText.From(text, fileName);
        return Parse(sourceText);
    }

    private static void Parse(SyntaxTree syntaxTree, out CompilationUnitSyntax root, out IEnumerable<Diagnostics> diagnostics)
    {
        var parser = new Parser(syntaxTree);
        root = parser.ParseCompilationUnit();
        diagnostics = parser.Diagnostics.ToList();
    }

    public static SyntaxTree Parse(string text)
    {
        var sourceText = SourceText.From(text);
        return Parse(sourceText);
    }

    public static SyntaxTree Parse(SourceText text)
    {
        return new SyntaxTree(text, Parse);
    }

    public static ReadOnlyCollection<Token> ParseTokens(string text, bool includeEndOfFile = false)
    {
        var sourceText = SourceText.From(text);
        return ParseTokens(sourceText, includeEndOfFile);
    }

    public static ReadOnlyCollection<Token> ParseTokens(string text, out ReadOnlyCollection<Diagnostics> diagnostics, bool includeEndOfFile = false)
    {
        var sourceText = SourceText.From(text);
        return ParseTokens(sourceText, out diagnostics, includeEndOfFile);
    }

    public static ReadOnlyCollection<Token> ParseTokens(SourceText text, bool includeEndOfFile = false)
    {
        return ParseTokens(text, out _, includeEndOfFile);
    }

    public static ReadOnlyCollection<Token> ParseTokens(SourceText text, out ReadOnlyCollection<Diagnostics> diagnostics, bool includeEndOfFile = false)
    {
        var tokens = new List<Token>();

        void ParseTokens(SyntaxTree st, out CompilationUnitSyntax root, out IEnumerable<Diagnostics> d)
        {
            var l = new Lexer(st);
            while (true)
            {
                var token = l.Tokenize();

                if (token.Type != TokenType.EOF || includeEndOfFile)
                    tokens.Add(token);

                if (token.Type == TokenType.EOF)
                {
                    root = new CompilationUnitSyntax(st, ReadOnlyCollection<MemberSyntax>.Empty, token);
                    break;
                }
            }

            d = l.Diagnostics.ToList();
        }

        var syntaxTree = new SyntaxTree(text, ParseTokens);
        diagnostics = syntaxTree.Diagnostics.ToList();
        return tokens.ToList();
    }

    internal SyntaxNode? GetParent(SyntaxNode syntaxNode)
    {
        if (_parents == null)
        {
            var parents = CreateParentsDictionary(Root);
            Interlocked.CompareExchange(ref _parents, parents, null);
        }

        return _parents[syntaxNode];
    }

    private Dictionary<SyntaxNode, SyntaxNode?> CreateParentsDictionary(CompilationUnitSyntax root)
    {
        var result = new Dictionary<SyntaxNode, SyntaxNode?>();
        result.Add(root, null);
        CreateParentsDictionary(result, root);
        return result;
    }

    private void CreateParentsDictionary(Dictionary<SyntaxNode, SyntaxNode?> result, SyntaxNode node)
    {
        foreach (var child in node.GetChildren())
        {
            result.Add(child, node);
            CreateParentsDictionary(result, child);
        }
    }
}
