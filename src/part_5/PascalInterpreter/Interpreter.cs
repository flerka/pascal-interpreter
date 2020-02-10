using System;

namespace PascalInterpreter
{
    public class Interpreter
    {
        private Token _currentToken;

        private readonly Lexer _lexer;

        /// <summary>
        /// Token ctr
        /// </summary>
        public Interpreter(Lexer lexer)
        {
            _lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));
            _currentToken = _lexer.GetNextToken();
        }

        /// <summary>
        /// Arithmetic expression parser / interpreter.
        /// </summary>
        public int Expr()
        {
            var result = Term();

            while (_currentToken?.Type == TokenType.PLUS || _currentToken?.Type == TokenType.MINUS)
            {
                switch (_currentToken.Type)
                {
                    case TokenType.PLUS:
                        Eat(TokenType.PLUS);
                        result += Factor();
                        break;
                    case TokenType.MINUS:
                        Eat(TokenType.MINUS);
                        result -= Factor();
                        break;
                    default:
                        throw new InvalidSyntaxException();
                }
            }

            return result;
        }

        /// <summary>
        /// Term : factor ((MUL | DIV) factor)
        /// </summary>
        /// <returns></returns>
        private int Term()
        {
            var result = Factor();

            while (_currentToken?.Type == TokenType.MUL || _currentToken?.Type == TokenType.DIV)
            {
                var token = _currentToken;
                switch (token.Type)
                {
                    case TokenType.MUL:
                        Eat(TokenType.MUL);
                        result *= Factor();
                        break;
                    case TokenType.DIV:
                        Eat(TokenType.DIV);
                        result /= Factor();
                        break;
                    default:
                        throw new InvalidSyntaxException();
                }
            }

            return result;
        }
        
        /// <summary>
        /// Return an INTEGER token value.
        /// </summary>
        private int Factor()
        {
            var token = _currentToken;

            switch (token.Type)
            {
                case TokenType.INTEGER:
                    Eat(TokenType.INTEGER);
                    return int.Parse(token.Value);
                case TokenType.LPAREN:
                    Eat(TokenType.LPAREN);
                    var result = Expr();
                    Eat(TokenType.RPAREN);
                    return result;
                default:
                    throw new InvalidSyntaxException();
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
            if (this._currentToken.Type != tokenType)
            {
                throw new InvalidSyntaxException();
            }

            _currentToken = _lexer.GetNextToken();
        }
    }
}