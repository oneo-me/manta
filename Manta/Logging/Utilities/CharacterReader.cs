namespace Manta.Logging.Utilities;

ref struct CharacterReader(ReadOnlySpan<char> s)
{
    ReadOnlySpan<char> _s = s;

    public bool End => _s.IsEmpty;

    public char Peek => _s[0];

    int Position { get; set; }

    public char Take()
    {
        ++Position;
        var num = (int)_s[0];
        _s = _s[1..];
        return (char)num;
    }

    public ReadOnlySpan<char> TakeUntil(char c)
    {
        var num = 0;
        while (num < _s.Length && _s[num] != c)
            ++num;
        var until = _s[..num];
        _s = _s[num..];
        Position += num;
        return until;
    }
}
