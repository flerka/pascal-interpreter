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
            // set current token to the first token taken from the input
            this.currentToken = GetNextToken();

            // we expect the current token to be a single-digit integer
            var left = this.currentToken;
            this.Eat(TokenType.INTEGER);

            // we expect the current token to be a '+' token
            var op = this.currentToken;
            if (op.Type != TokenType.PLUS && op.Type != TokenType.MINUS)
            {
                throw new Exception();
            }
            this.Eat(op.Type);

            // we expect the current token to be a single-digit integer
            var right = this.currentToken;
            this.Eat(TokenType.INTEGER);

            // at this point either the INTEGER PLUS INTEGER or
            // the INTEGER MINUS INTEGER sequence of tokens
            // has been successfully found and the method can just
            // return the result of adding or subtracting two integers,
            // thus effectively interpreting client input
            var leftNumber = int.Parse(left.Value);
            var rightNumber = int.Parse(right.Value);
            return op.Type == TokenType.MINUS ? leftNumber - rightNumber : leftNumber + rightNumber; 
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
                        throw new Exception();
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
                throw new Exception();
            }

            this.currentToken = GetNextToken();
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