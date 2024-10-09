using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SeparatedSyntaxList
{
    private protected SeparatedSyntaxList()
    {
    }

    public abstract IEnumerable<SyntaxNode> GetWithSeparators();
}

public sealed class SeparatedSyntaxList<T> : SeparatedSyntaxList, IEnumerable<T>
where T: SyntaxNode
{
    private readonly IEnumerable<SyntaxNode> _nodesAndSeparators;

    internal SeparatedSyntaxList(IEnumerable<SyntaxNode> nodesAndSeparators)
    {
        _nodesAndSeparators = nodesAndSeparators;
    }

    public int Count => (_nodesAndSeparators.Length + 1) / 2;

    public T this[int index] => (T) _nodesAndSeparators[index * 2];

    public Token GetSeparator(int index)
    {
        if (index < 0 || index >= Count - 1)
            throw new ArgumentOutOfRangeException(nameof(index));

        return (Token) _nodesAndSeparators[index * 2 + 1];
    }

    public override IEnumerable<SyntaxNode> GetWithSeparators() => _nodesAndSeparators;

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
