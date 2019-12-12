// Alex McKee
// 011659950

namespace CptS321
{
    /// <summary>
    /// Class for operators nodes of the expression tree.
    /// </summary>
    public partial class ExpressionTree
    {
        // OperatorNode class.
        private class OperatorNode : Node
        {
            private char operatorChar;
            private Node left;
            private Node right;

            /// <summary>
            /// Initializes a new instance of the <see cref="OperatorNode"/> class.
            /// </summary>
            /// <param name="c"> Operator </param>
            public OperatorNode()
            {
                Left = Right = null;
            }

            // Properties
            public virtual char Operator { get => operatorChar; set => operatorChar = value; }

            public Node Left { get => left; set => left = value; }

            public Node Right { get => right; set => right = value; }

            public virtual int Precedence { get; set; }

            public virtual string Associativity { get; set; }

            public override double Evaluate()
            {
                throw new System.NotImplementedException();
            }
        }
    }

}