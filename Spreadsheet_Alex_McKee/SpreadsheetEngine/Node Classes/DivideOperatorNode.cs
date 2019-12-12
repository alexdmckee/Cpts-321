// Alex McKee
// 011659950

using System;

namespace CptS321
{
    public partial class ExpressionTree
    {
        private class DivideOperatorNode : OperatorNode
        {

            public override char Operator => '/';
            public override int Precedence => 2;
            public override string Associativity => "Left";

            public DivideOperatorNode()
            {

            }

            public override double Evaluate()
            {
                double rightEval = this.Right.Evaluate();
                if (rightEval == 0 || rightEval == 0.0)
                {
                    throw new DivideByZeroException("Error in divide Evaluate: right child was zero");
                }

                return this.Left.Evaluate() / rightEval;

            }
        }

    }

}