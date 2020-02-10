using System;
using System.Collections.Generic;
using System.Text;

namespace PascalInterpreter
{
    public class Lexer
    {
        private int _position;
        private char? _currentChar;

        private readonly string _text;

        /// <param name="text">string input, e.g. "3+5"</param>
        public Lexer(string text)
        {
            _text = text;
            if (_text != null) _currentChar = _text[0];
        }

        /// <summary>
        /// Lexical analyzer (also known as scanner or tokenizer)
        /// This method is responsible for breaking a sentence
        /// apart into tokens. One token at a time.
        /// </summary>
        public Token GetNextToken()
        {
            while (_currentChar != null)
            {
                // if the character is a digit then convert it to
                // integer, create an INTEGER token, increment position
                // index to point to the next character after the digit,
                // and return the INTEGER token
                switch (_currentChar)
                {
                    case ' ':
                        SkipWhitespace();
                        continue;
                    case '*':
                        Advance();
                        return new Token(TokenType.MUL, _currentChar.ToString());
                    case '/':
                        Advance();
                        return new Token(TokenType.DIV, _currentChar.ToString());
                    case '+':
                        Advance();
                        return new Token(TokenType.PLUS, _currentChar.ToString());
                    case '-':
                        Advance();
                        return new Token(TokenType.MINUS, _currentChar.ToString());
                    case var _ when int.TryParse(_currentChar.ToString(), out _):
                        return new Token(TokenType.INTEGER, Integer());
                    default:
                        throw new InvalidSyntaxException();
                }
            }

            return null;
        }

        /// <summary>
        /// Advance the position pointer and set the currentChar variable.
        /// </summary>
        private void Advance()
        {
            _position++;
            if (_position > _text.Length - 1)
            {
                _currentChar = null;
            }
            else
            {
                _currentChar = _text[_position];
            }
        }

        /// <summary>
        /// Return a (multidigit) integer consumed from the input.
        /// </summary>
        private string Integer()
        {
            var result = string.Empty;
            while (_currentChar != null && char.IsDigit(_currentChar.Value))
            {
                result += _currentChar;
                Advance();
            }

            return result;
        }

        private void SkipWhitespace()
        {
            while (_currentChar != null && Char.IsWhiteSpace(_currentChar.Value))
            {
                Advance();
            }
        }
    }
}
