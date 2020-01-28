using System;

namespace PascalInterpreter
{
    public class Interpreter
    {
        private readonly string _text;

        private int position = 0;
        private Token currentToken = null;

        /// <summary>
        /// Token ctr
        /// </summary>
        /// <param name="text">string input, e.g. "3+5"</param>
        public Interpreter(string text)
        {
            this._text = text;
        }

        /// <summary>
        /// expr -> INTEGER PLUS INTEGER
        /// </summary>
        public int BuildExpression()
        {
            // set current token to the first token taken from the input
            this.currentToken = GetNextToken();

            // we expect the current token to be a single-digit integer
            var left = this.currentToken;
            this.Eat(TokenType.INTEGER);

            // we expect the current token to be a '+' token
            var op = this.currentToken;
            this.Eat(TokenType.PLUS);

            // we expect the current token to be a single-digit integer
            var right = this.currentToken;
            this.Eat(TokenType.INTEGER);

            // at this point INTEGER PLUS INTEGER sequence of tokens
            // has been successfully found and the method can just
            // return the result of adding two integers, thus
            // effectively interpreting client input
            return int.Parse(left.Value) + int.Parse(right.Value);
        }

        /// <summary>
        /// Lexical analyzer (also known as scanner or tokenizer)
        /// This method is responsible for breaking a sentence
        /// apart into tokens.One token at a time.
        /// </summary>
        private Token GetNextToken()
        {
            // is self.pos index past the end of the self.text ?
            // so, then return EOF token because there is no more
            // input left to convert into tokens
            if (position > _text.Length - 1)
            {
                return new Token(TokenType.EOF, string.Empty);
            }

            // get a character at the position self.pos and decide
            // what token to create based on the single character
            var currentChar = _text[position];

            // if the character is a digit then convert it to
            // integer, create an INTEGER token, increment self.pos
            // index to point to the next character after the digit,
            // and return the INTEGER token
            switch (currentChar)
            {
                case '+':
                    this.position++;
                    return new Token(TokenType.PLUS, currentChar.ToString());
                case var _ when int.TryParse(currentChar.ToString(), out int number):
                    this.position++;
                    return new Token(TokenType.INTEGER, number.ToString());
                default:
                    throw new Exception();
            }
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
    }
}