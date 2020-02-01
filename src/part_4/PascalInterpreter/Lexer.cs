﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PascalInterpreter
{
    public class Lexer
    {
        private int position = 0;
        private char? currentChar;

        private readonly string _text;

        /// <param name="text">string input, e.g. "3+5"</param>
        public Lexer(string text)
        {
            this._text = text;
            currentChar = _text[0];
        }

        /// <summary>
        /// Lexical analyzer (also known as scanner or tokenizer)
        /// This method is responsible for breaking a sentence
        /// apart into tokens. One token at a time.
        /// </summary>
        public Token GetNextToken()
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
                        this.SkipWhitespaces();
                        continue;
                    case '*':
                        UpdatePosition();
                        return new Token(TokenType.MUL, currentChar.ToString());
                    case '/':
                        UpdatePosition();
                        return new Token(TokenType.DIV, currentChar.ToString());
                    case var _ when int.TryParse(currentChar.ToString(), out int number):
                        return new Token(TokenType.INTEGER, GetMultidigitIntSubstring());
                    default:
                        throw new InvalidSyntaxException();
                }
            }

            return null;
        }

        private void UpdatePosition()
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

        /// <summary>
        /// Return a (multidigit) integer consumed from the input.
        /// </summary>
        private string GetMultidigitIntSubstring()
        {
            var result = string.Empty;
            while (currentChar != null && char.IsDigit(currentChar.Value))
            {
                result += currentChar;
                UpdatePosition();
            }

            return result;
        }

        private void SkipWhitespaces()
        {
            while (this.currentChar != null && Char.IsWhiteSpace(this.currentChar.Value))
            {
                UpdatePosition();
            }
        }
    }
}
