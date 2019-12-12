//Alex McKee
// 011659950

namespace CptS321
{
    public partial class ExpressionTree
    {
        private class MultiplyOperatorNode : OperatorNode
        {

            public override char Operator => '*';
            public override int Precedence => 2;
            public override string Associativity => "Left";

            public MultiplyOperatorNode()
            {

            }

            public override double Evaluate()
            {
                return this.Left.Evaluate() * this.Right.Evaluate();
            }
        }
    }

}

    



