using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

sealed class SyntaxTree 
{
    public ExpresionSyntax Root{get;}
    public Token EndOfFileToken{get;}
    public IReadOnlyList<string> Diagnostics{get;}

    public SyntaxTree(IEnumerable<string> diagnostics, ExpresionSyntax root, Token endOfFileToken)
    {
        Diagnostics = diagnostics.ToArray();
        Root = root;
        EndOfFileToken = endOfFileToken;
    }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }
}
