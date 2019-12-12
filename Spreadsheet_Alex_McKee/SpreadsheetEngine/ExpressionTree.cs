// Alex McKee
// 011659950

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public partial class ExpressionTree
    {
        // Fields.
        private Node root = null;
        public string infixExpression = null;
        private string postFix = null;
        private Dictionary<string, double> variables = new Dictionary<string, double>();
        // operators
        string[] operators = { "+", "-", "*", "/" };

        // Return root node.
        public Node GetRoot()
        {
            return (OperatorNode)root;
        }

        // Sets a user defined variable with a value.
        private void SetVariableHelper(string variableName, double variableValue)
        {
            if (variables.ContainsKey(variableName))
            {
                variables[variableName] = variableValue;
            }
            else
            {
                variables.Add(variableName, variableValue);
            }
        }

        // Made so the SetVariable signature could be private as prescribed in assignment.
        public void SetVariable(string variableName, double variableValue)
        {
            SetVariableHelper(variableName, variableValue);
        }


            /// <summary>
            /// Takes a postfix expression from an expression tree and filters out the variables.
            /// </summary>
            /// <returns>
            /// retVal: The variables of an expression.
            /// </returns>
        public string[] GetVariableNames()
        {
            string[] expressionNoOperators = postFix.Split('+', '-', '/', '*', ' ');

            // filter out empty strings
            IEnumerable<string> justVariables = expressionNoOperators.Where(n => n.Length >= 1);

            // Filter out constants.
            string[] retVal = justVariables.Where(n => n[0] >= 'A' && n[0] <= 'z').ToArray();
            return retVal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Constructor
        /// </summary>
        /// <param name="expression"> Expression to build tree from</param>
        public ExpressionTree(string expression)
        {

            // get operator
            // split on operator
            // convert to postfix a b + c + d +
            // build tree from left to right using postfix
            infixExpression = expression;

            if (expression.Length > 0)
            {
                postFix = PostFixExpression(expression);
                root = BuildTree(postFix.ToString());
            }

        }

        /// <summary>
        /// Returns an expression tree.
        /// </summary>
        /// <param name="expression"> postFix expression</param>
        /// <returns> Expression tree. </returns>
        public Node BuildTree(string expression)
        {

            if (expression.Length == 0)
            {
                return null;
            }

            // Stack of Nodes
            Stack<Node> treeStack = new Stack<Node>();

            // Split the expression on the space character
            string[] args = expression.Split();

            // Split the expression on the space character
            // Scan the arguments array left to right
            // If operator make an operator node and set the left and right children to the next two arguments on the stack
            // If argument is begins with a-Z make a variable node (no children) and push on stack
            // Else its a constant node and push on stack
            foreach (var arg in args)
            {
                if (arg.Length == 0)
                {

                }
                else
                {
                    if (operators.Contains(arg))
                    {
                        OperatorNode node = ExpressionTreeFactory.CreateOperatorNode(arg[0]);
                        node.Right = treeStack.Pop();
                        node.Left = treeStack.Pop();
                        treeStack.Push(node);
                    }
                    else if (arg[0] >= 'A' && arg[0] <= 'z')
                    {
                        VariableNode node = new VariableNode(arg, ref variables);
                        treeStack.Push(node);
                    }
                    else
                    {
                        ConstantNode node = new ConstantNode(int.Parse(arg));
                        treeStack.Push(node);
                    }
                }
            }

            // Scanned through argument array
            this.root = treeStack.Pop();
            return root;
        }

        // Evaluate the expression tree.
        public double Evaluate()
        {
            return EvaluateHelper(root);
        }

        // Helper so Evaluate can be accesed outside class.
        public double EvaluateHelper(Node node)
        {
            return node.Evaluate();
        }

        /// <summary>
        /// Converts an infix expression into postFix, or "reverse polish notation".  
        /// </summary>
        /// <param name="expression"> infix expression. </param>
        /// <returns> The infix expression to postfix.  Used in building the expression tree.
        /// </returns>
        public string PostFixExpression(string expression)
        {
            // SHUNTING YARD ALGORITHM

            // Code block to check if parentheses match.
            int leftparen = 0;
            int rightParen = 0;
            foreach (char c in expression)
            {
                if (c == '(')
                {
                    leftparen++;
                }

                if (c == ')')
                {
                    rightParen++;
                }
            }

            if (rightParen != leftparen)
            {
                return "Mismatched Parentheses";
            }

            // variables
            List<string> postFixListArray = new List<string>(); // loop through and add to string builder with space each time
            StringBuilder postFix = new StringBuilder();
            char[] operators = { '+', '-', '*', '/', '(', ')'};
            Stack<char> operatorStack = new Stack<char>();

            // SHUNTING YARD ALGORITHM
            // SCAN THROUGH WHOLE EXPRESSION. 
            for (int i = 0; i < expression.Length; i++)
            {
                char character = expression[i];
                // handle leading spaces
                if (expression[i] == ' ')
                {
                    continue;
                }

                // 1.If the incoming symbols is an operand, output it..
                // If operand, get whole operand.
                else if (!operators.Contains(expression[i]))
                {
                    StringBuilder operand = new StringBuilder();
                    // scan until an operator or parenthesis is hit 
                    while (!operators.Contains(expression[i]) && expression[i] != '(' && expression[i] != ')')
                    {
                        // handle trailing spaces.
                        if (expression[i] != ' ')
                        {
                            operand.Append(expression[i]);
                        }

                        i++;

                        if (i >= expression.Count())
                        {
                            break;
                        }
                    }

                    // at this point i is on an operator or parentheses. and we have gotten the whole variable
                    postFix.Append(operand + " ");
                }

                if (i < expression.Count())
                {
                    //2. If the incoming symbol is a left parenthesis, push it on the stack.
                    if (expression[i] == '(')
                    {
                        operatorStack.Push(expression[i]);
                    }

                    /* 3.If the incoming symbol is a right parenthesis: discard the right
                           parenthesis, pop and print the stack symbols until you see a left
                           parenthesis.Pop the left parenthesis and discard it. */
                    else if (expression[i] == ')')
                    {
                        while (operatorStack.Count != 0 && operatorStack.Peek() != '(')
                        {
                            postFix.Append(operatorStack.Pop().ToString() + " ");
                        }

                        // see a left paren.
                        if (operatorStack.Count != 0)
                        {
                            operatorStack.Pop();
                        }
                    }

                    // 4.If the incoming symbol is an operator and the stack is empty or contains
                    // a left parenthesis on top, push the incoming operator onto the stack.
                    else if (operators.Contains(expression[i]))
                    {
                        OperatorNode temp = ExpressionTreeFactory.CreateOperatorNode(expression[i]);
                       
                        if (operatorStack.Count == 0 || operatorStack.Peek() == '(')
                        {
                            operatorStack.Push(expression[i]);
                        }

                        /*  5.If the incoming symbol is an operator and has either higher precedence
                              than the operator on the top of the stack, or has the same precedence as
                              the operator on the top of the stack and is right associative --push it on
                              the stack. */
                        else
                        {
                            bool v = ExpressionTreeFactory.CreateOperatorNode(operatorStack.Peek()).Precedence == temp.Precedence;
                            if (ExpressionTreeFactory.CreateOperatorNode(operatorStack.Peek()).Precedence < temp.Precedence || (v && temp.Associativity != "Left"))
                            {
                                operatorStack.Push(expression[i]);
                            }

                            /*  6.If the incoming symbol is an operator and has either lower precedence
                                  than the operator on the top of the stack, or has the same precedence as
                                  the operator on the top of the stack and is left associative -- continue to
                                  pop the stack until this is not true.Then, push the incoming operator */
                            else
                            {
                                OperatorNode opOnTopOfStack = ExpressionTreeFactory.CreateOperatorNode(operatorStack.Peek());
                                // incoming symbol has lower precedence                                      // equal precedence but                            left associative
                                while ((temp.Precedence < opOnTopOfStack.Precedence) || ((temp.Precedence == opOnTopOfStack.Precedence) && (temp.Associativity == "Left"))) // incoming symbol has lower precedence
                                {

                                    postFix.Append(operatorStack.Pop() + " ");

                                    if (operatorStack.Count == 0)
                                    {
                                        break;
                                    }

                                    opOnTopOfStack = ExpressionTreeFactory.CreateOperatorNode(operatorStack.Peek());
                                }

                                // then push the operator on the stack.
                                operatorStack.Push(temp.Operator);
                            }
                        }
                    }
                }

            }

            /* 7.At the end of the expression, pop and print all operators on the stack.
                (No parentheses should remain.) */
            while (operatorStack.Count > 0)
            {
                postFix.Append(operatorStack.Pop() + " ");
            }

            return postFix.ToString();

    }
    }
}