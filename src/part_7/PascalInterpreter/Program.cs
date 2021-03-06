﻿using System;

namespace PascalInterpreter
{
    static class Program
    {
        private static void Main()
        {
            while (true)
            {
                try
                {
                    var text = Console.ReadLine();
                    var interpreter = new Interpreter(new Lexer(text));
                    var result = interpreter.Expr();
                    Console.WriteLine(result);
                }
                catch(InvalidSyntaxException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}