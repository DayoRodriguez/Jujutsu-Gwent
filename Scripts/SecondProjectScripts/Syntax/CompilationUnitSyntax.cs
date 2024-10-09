using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompilationUnitSyntax : SyntaxNode
{
    internal CompilationUnitSyntax(SyntaxTree syntaxTree, IEnumerable<MemberSyntax> members, Token eOF)
    : base(syntaxTree)
    {
        Members = members;
        EOF = eOF;
    }

    public override TokenType Type => TokenType.CompilationUnit;
    public IEnumerable<MemberSyntax> Members { get; }
    public Token EOF { get; }
}
