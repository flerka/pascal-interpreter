using System;

namespace PascalInterpreter
{
    public class Interpreter
    {
        private Token currentToken = null;

        private readonly Lexer _lexer;

        /// <summary>
        /// Token ctr
        /// </summary>
        public Interpreter(Lexer lexer)
        {
            if (lexer == null)
            {
                throw new ArgumentNullException(nameof(lexer));
            }

            _lexer = lexer;
            currentToken = _lexer.GetNextToken();
        }

        /// <summary>
        /// Parser / Interpreter
        /// expr -> INTEGER PLUS INTEGER
        /// expr -> INTEGER MINUS INTEGER
        /// </summary>
            public int BuildExpression()
        {
            var result = GetTerm();

            while (currentToken?.Type == TokenType.MUL || currentToken?.Type == TokenType.DIV)
            {
                switch (currentToken.Type)
                {
                    case (TokenType.MUL):
                        Eat(TokenType.MUL);
                        result *= GetTerm();
                        break;
                    case (TokenType.DIV):
                        Eat(TokenType.DIV);
                        result /= GetTerm();
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Return an INTEGER token value.
        /// </summary>
        private int GetTerm()
        {
            var token = currentToken;
            Eat(TokenType.INTEGER);
            return int.Parse(token.Value);
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

            this.currentToken = _lexer.GetNextToken();
        }
    }
}