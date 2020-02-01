using System;

namespace PascalInterpreter
{
    public class Interpreter
    {
        private readonly string _text;

        private int position = 0;
        private Token currentToken = null;
        private char? currentChar;

        /// <summary>
        /// Token ctr
        /// </summary>
        /// <param name="text">string input, e.g. "3+5"</param>
        public Interpreter(string text)
        {
            this._text = text;
            currentChar = _text[0];
        }

        /// <summary>
        /// Parser / Interpreter
        /// expr -> INTEGER PLUS INTEGER
        /// expr -> INTEGER MINUS INTEGER
        /// </summary>
        public int Expr()
        {
            currentToken = GetNextToken();
            var result = Factor();

            while (currentToken?.Type == TokenType.PLUS || currentToken?.Type == TokenType.MINUS)
            {
                switch (currentToken.Type)
                {
                    case (TokenType.PLUS):
                        Eat(TokenType.PLUS);
                        result += Factor();
                        break;
                    case (TokenType.MINUS):
                        Eat(TokenType.MINUS);
                        result -= Factor();
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Lexical analyzer (also known as scanner or tokenizer)
        /// This method is responsible for breaking a sentence
        /// apart into tokens. One token at a time.
        /// </summary>
        private Token GetNextToken()
        {
            while (currentChar != null)
            {
                // if the character is a digit then convert it to
                // integer, create an INTEGER token, increment self.pos
                // index to point to the next character after the digit,
                // and return the INTEGER token
                switch (currentChar)
                {
                    case ' ':
                        this.SkipWhitespace();
                        continue;
                    case '+':
                        Advance();
                        return new Token(TokenType.PLUS, currentChar.ToString());
                    case '-':
                        Advance();
                        return new Token(TokenType.MINUS, currentChar.ToString());
                    case var _ when int.TryParse(currentChar.ToString(), out int number):
                        return new Token(TokenType.INTEGER, Integer());
                    default:
                        throw new InvalidSyntaxException();
                }
            }

            return null;
        }

        /// <summary>
        /// Return a (multidigit) integer consumed from the input.
        /// </summary>
        private string Integer()
        {
            var result = string.Empty;
            while(currentChar != null && char.IsDigit(currentChar.Value))
            {
                result += currentChar;
                Advance();
            }

            return result;
        }

        /// <summary>
        /// Compare the current token type with the passed token
        /// type and if they match then "eat" the current token
        /// and assign the next token to the self.current_token,
        /// otherwise raise an exception.
        /// </summary>
        private void Eat(TokenType tokenType)
        {
            if (this.currentToken.Type != tokenType)
            {
                throw new InvalidSyntaxException();
            }

            this.currentToken = GetNextToken();
        }

        /// <summary>
        /// Return an INTEGER token value.
        /// </summary>
        private int Factor()
        {
            var token = currentToken;
            Eat(TokenType.INTEGER);
            return int.Parse(token.Value);
        }

        /// <summary>
        /// Advance the position pointer and set the currentChar variable.
        /// </summary>
        private void Advance()
        {
            position++;
            if (position > _text.Length - 1)
            {
                currentChar = null;
            }
            else
            {
                currentChar = _text[position];
            }
        }

        private void SkipWhitespace()
        {
            while (this.currentChar != null && Char.IsWhiteSpace(this.currentChar.Value))
            {
                Advance();
            }
        }
    }
}