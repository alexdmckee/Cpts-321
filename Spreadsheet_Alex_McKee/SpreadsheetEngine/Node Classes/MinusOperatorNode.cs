// Alex McKee
// 011659950

namespace CptS321
{
    /// <summary>
    /// Part of expression tree.
    /// </summary>
    public partial class ExpressionTree
    {
        private class MinusOperatorNode : OperatorNode
        {

            // Properties used in shunting algorithm.
            public override char Operator => '-';
            public override int Precedence => 1;
            public override string Associativity => "Left";

            // Constructor
            public MinusOperatorNode()
            {
            }

            public override double Evaluate()
            {
                return this.Left.Evaluate() - this.Right.Evaluate();
            }
        }
    }

}