namespace DummyLang.LexicalAnalysis;

public readonly struct TokenPositions
{
    private readonly TokenPosition[] _tokenPositions;

    public int Length { get; }

    public TokenPositions()
    {
        _tokenPositions = [];
        Length          = 0;
    }

    public TokenPositions(params TokenPosition?[]? tokenPositions)
    {
        if (tokenPositions is null or { Length: 0 })
        {
            _tokenPositions = [];
        }
        else
        {
            _tokenPositions = new TokenPosition[tokenPositions.Length];
            for (var i = 0; i < tokenPositions.Length; i++)
                _tokenPositions[i] = tokenPositions[i] ?? TokenPosition.Zero;
        }

        Length = _tokenPositions.Length;
    }

    public TokenPosition this[int index, bool ascending = true] =>
        ascending ? GetClosestToLast(index) : GetClosestToFirst(index);

    private TokenPosition GetClosestToFirst(int index)
    {
        var last = index;
        if ((uint)last >= (uint)_tokenPositions.Length)
            last = _tokenPositions.Length - 1;

        return GetClosestToFirstOrDefault(last);
    }

    private TokenPosition GetClosestToFirstOrDefault(int last)
    {
        for (var i = last; i >= 0; i--)
            if (_tokenPositions[i] != TokenPosition.Zero)
                return _tokenPositions[i];

        return TokenPosition.Zero;
    }

    private TokenPosition GetClosestToLast(int index)
    {
        var first = index;
        if ((uint)first >= (uint)_tokenPositions.Length)
            first = 0;

        return GetClosestToLastOrDefault(first);
    }

    private TokenPosition GetClosestToLastOrDefault(int first)
    {
        for (var i = first; i < _tokenPositions.Length; i++)
            if (_tokenPositions[i] != TokenPosition.Zero)
                return _tokenPositions[i];

        return TokenPosition.Zero;
    }
}