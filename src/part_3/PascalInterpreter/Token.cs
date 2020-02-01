
namespace PascalInterpreter
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set; }

        /// <summary>
        /// Interpreter ctr
        /// </summary>
        /// <param name="type">token value: 0, 1, 2. 3, 4, 5, 6, 7, 8, 9, '+', or None</param>
        /// <param name="value">token type: INTEGER, PLUS, or EOF</param>
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() => $"{Type}, {Value}";
    }
}