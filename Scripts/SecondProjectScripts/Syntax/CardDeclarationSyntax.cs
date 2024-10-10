using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeclarationSyntax : ExpresionSyntax
{
    public override TokenType Type => TokenType.Card;

    public CardDeclarationSyntax(SintaxTree sintaxTree, )

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        throw new System.NotImplementedException();
    }
}
