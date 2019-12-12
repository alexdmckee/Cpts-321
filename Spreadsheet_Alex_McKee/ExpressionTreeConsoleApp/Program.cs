using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTreeConsoleApp
{
    public class Program
    {
        /// <summary>
        /// entry point.
        /// </summary>
        /// <param name="args"> args. </param>
        static void Main(string[] args)
        {
            CptS321.ExpressionTree tree = new CptS321.ExpressionTree("2+2");
            Console.WriteLine("Menu (The current expression is 2+2) \n");

            int choice = 0;
            do
            {
                Console.WriteLine("Menu (The current expression is " + tree.infixExpression +")");
                Console.WriteLine("\t1 - Enter a new expression");
                Console.WriteLine("\t2 - Set a variable value");
                Console.WriteLine("\t3 - Evaluate Tree");
                Console.WriteLine("\t4 - Quit");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter new expression: ");
                        string newExpression = Console.ReadLine();
                        string postFix = tree.PostFixExpression(newExpression);
                        tree.infixExpression = newExpression;
                        tree.BuildTree(postFix);
                        break;

                    case 2:
                        Console.Write("Enter variable name: ");
                        string variable = Console.ReadLine();
                        Console.Write("\n Enter value: ");
                        string value = Console.ReadLine();
                        double parsedDouble;
                        double.TryParse(value, out parsedDouble);
                        tree.SetVariable(variable, parsedDouble);
                        break;

                    case 3:
                        Console.WriteLine("The expression evaluates to: " + tree.Evaluate());
                        break;
                    case 4:
                        Console.WriteLine("Done");
                        break;

                }
            } while (choice != 4);

        }
    }
}
