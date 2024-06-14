namespace DummyLang.LexicalAnalysis;

public sealed class Lexer
{
    private readonly string _source;

    private int _position;
    private char _current;

    public Lexer(string source)
    {
        _source = source;
        _position = 0;
        _current = '\0';
    }

    public Token ReadNext()
    {
        if (_position >= _source.Length)
        {
            return new Token(TokenType.Eof, string.Empty);
        }

        _current = _source[_position];
        _position++;

        return _current switch
        {
            ',' => new Token(TokenType.Comma, ","),
            ';' => new Token(TokenType.Semicolon, ";"),
            ':' => new Token(TokenType.Colon, ":"),
            '=' => new Token(TokenType.Assign, "="),
            '+' => new Token(TokenType.Plus, "+"),
            '-' => new Token(TokenType.Minus, "-"),
            '*' => new Token(TokenType.Star, "*"),
            '/' => new Token(TokenType.Slash, "/"),
            '<' => new Token(TokenType.LessThan, "<"),
            '>' => new Token(TokenType.GreaterThan, ">"),
            '(' => new Token(TokenType.LeftParen, "("),
            ')' => new Token(TokenType.RightParen, ")"),
            '{' => new Token(TokenType.LeftBrace, "{"),
            '}' => new Token(TokenType.RightBrace, "}"),
            '[' => new Token(TokenType.LeftBracket, "["),
            ']' => new Token(TokenType.RightBracket, "]"),
            _ => char.IsLetter(_current) || _current == '_'
                ? ReadWord()
                : char.IsDigit(_current)
                    ? ReadNumber()
                    : ReadOther()
        };
    }

    private Token ReadWord()
    {
        // TODO: Implement ReadWord method
        return new Token(TokenType.Invalid, string.Empty);
    }

    private Token ReadNumber()
    {
        // TODO: Implement ReadNumber method
        return new Token(TokenType.Invalid, string.Empty);
    }

    private Token ReadOther()
    {
        // TODO: Implement ReadOther method
        return new Token(TokenType.Invalid, string.Empty);
    }
}