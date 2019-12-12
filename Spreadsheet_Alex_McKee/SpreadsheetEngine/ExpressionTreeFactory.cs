// Alex McKee
// 011659950

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Creates operated nodes for the expression tree. In use since OperatorNOde is a private class.
    /// </summary>
    public partial class ExpressionTree
    {
        // ExpressionTreeNodeFactory class.
        private class ExpressionTreeFactory
        {
           
           public static OperatorNode CreateOperatorNode(char op)
            {
                switch (op)
                {
                    case '+':
                        AddNode node = new AddNode();
                        return node;
                    case '-':
                        MinusOperatorNode node1 = new MinusOperatorNode();
                        return node1;
                    case '*':
                        MultiplyOperatorNode node2 = new MultiplyOperatorNode();
                        return node2;
                    case '/':
                        DivideOperatorNode node3 = new DivideOperatorNode();
                        return node3;
                }

                // if it is not any of the operators that we support, throw an exception:
                throw new NotSupportedException(
                    "Operator " + op + " not supported.");
            }
        }
    }
}
