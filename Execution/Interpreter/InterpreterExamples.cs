using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace Execution.Interpreter
{
    public interface IElement
    {
        int Value { get;  }
    }

    public class Integer : IElement
    {
        public int Value { get; }

        public Integer(int value)
        {
            Value = value;
        }
    }

    public class BinaryOperation : IElement
    {
        public enum Type
        {
            Addition, Subtraction
        }
        public Type MyType;
        public IElement Left, Right;

        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Subtraction:
                        return Left.Value - Right.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public class Token
    {
        public enum Type
        {
            Integer, Plus, Minus, Lparen, Rparen
        }

        public Type MyType;
        public string Text;

        public Token(Type myType, string text)
        {
            MyType = myType;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public override string ToString()
        {
            return $"'{Text}'";
        }
    }
    public class InterpreterExamples
    {
        static List<Token> Lex(string input)
        {
            var result = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        result.Add(new Token(Token.Type.Plus, input[i].ToString()));
                        break;
                    case '-':
                        result.Add(new Token(Token.Type.Minus, input[i].ToString()));
                        break;
                    case '(':
                        result.Add(new Token(Token.Type.Lparen, input[i].ToString()));
                        break;
                    case ')':
                        result.Add(new Token(Token.Type.Rparen, input[i].ToString()));
                        break;
                    default:
                        var sb = new StringBuilder(input[i].ToString());

                        for (int j = i + 1; j < input.Length; ++j)
                        {
                            if (char.IsDigit(input[j]))
                            {
                                sb.Append(input[j]);
                                ++i;
                            }
                            else
                            {
                                result.Add(new Token(Token.Type.Integer, sb.ToString()));
                                break;
                            }
                        }
                        break;
                }
            }

            return result;
        }

        public static void HandmadeInterpreter_Lex()
        {
            string input = "(13+4)-(12+1)"; // "( 13 + 4 ) - ( 12 + 1 )"
            var tokens = Lex(input);

            WriteLine(string.Join("\t", tokens));
        }

        static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOperation();
            bool haveLHS = false;   // boolean indicator if have left hand side or no...

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                // look at the type of token
                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        var integer = new Integer(int.Parse(token.Text));

                        if (!haveLHS)
                        {
                            result.Left = integer;
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = integer;
                        }
                        break;
                    case Token.Type.Plus:
                        result.MyType = BinaryOperation.Type.Addition;
                        break;
                    case Token.Type.Minus:
                        result.MyType = BinaryOperation.Type.Subtraction;
                        break;
                    case Token.Type.Lparen:
                        int j = i;

                        for (; j < tokens.Count; ++j)
                            if (tokens[j].MyType == Token.Type.Rparen)
                                break;  // found it 

                        // process subexpression w/o opening (
                        var subexpression = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                        var element = Parse(subexpression);

                        if (!haveLHS)
                        {
                            result.Left = element;
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = element;
                        }

                        i = j; // advance
                        break;
                    //case Token.Type.Rparen:
                    //    break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static void HandmadeInterpreter_Parsing()
        {
            string input = "(13+4)-(12+1)"; // "( 13 + 4 ) - ( 12 + 1 )"
            var tokens = Lex(input);

            WriteLine(string.Join("\t", tokens));

            var parsed = Parse(tokens);
            WriteLine($"{input} = {parsed.Value}");
        }

        /// <summary>
        /// ANTLR (Another Tool for Language Recgonition) is a powerful parser generator for reading, processing, executing, or translating structured text or binary files. 
        /// It's widely used to build languages, tools, and frameworks. From a grammar, ANTLR generates a parser that can build and walk parse trees.
        /// <seealso cref="www.antlr.org"/>
        /// </summary>
        public static void ANTLR()
        {
            //
        }

        public static void Exercise()
        {
            var expr1 = "1+2+3";    // should return 6
            var expr2 = "1+2+xy";   // should return 0
            var expr3 = "10-2-x";   // when x = 3, shoulf return 5

            var processor = new ExpressionProcessor();

            WriteLine($"{expr1}={processor.Calculate(expr1)}");
            WriteLine($"{expr1}={processor.Calculate(expr2)}");
            WriteLine($"{expr1}={processor.Calculate(expr3)}");

            processor.Variables.Add('x', 3);
            WriteLine($"{expr1}={processor.Calculate(expr3)}, with x = 3");
        }         
    }

    public class ExpressionProcessor
    {
        public Dictionary<char, int> Variables = new Dictionary<char, int>();
        public enum NextOper
        {
            Nothing, Plus, Minus
        }

        public int Calculate(string expression)
        {
            int current = 0;
            var nextOper = ExpressionProcessor.NextOper.Nothing;
            var parts = Regex.Split(expression, @"(?<=[+-])");  // "1+2+3" will return ["1+", "2+", "3"]...

            foreach (var part in parts)
            {
                var noOp = part.Split(new[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);
                var first = noOp[0];    // gets the first char before the operator
                int value;

                if (int.TryParse(first, out int z))
                {
                    value = z;
                }
                else if ((first.Length == 1) && (Variables.ContainsKey(first[0])))
                {
                    value = Variables[first[0]];
                }
                else
                {
                    return 0;
                }

                switch (nextOper)
                {
                    case NextOper.Nothing:
                        current = value;
                        break;
                    case NextOper.Plus:
                        current += value;
                        break;
                    case NextOper.Minus:
                        current -= value;
                        break;
                }

                if (part.EndsWith("+"))
                    nextOper = NextOper.Plus;
                else if (part.EndsWith("-"))
                    nextOper = NextOper.Minus;
            }

            return current;
        }
    }
}
