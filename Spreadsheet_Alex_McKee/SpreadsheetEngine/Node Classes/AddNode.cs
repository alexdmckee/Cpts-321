namespace CptS321
{
    public partial class ExpressionTree
    {
        private class AddNode : OperatorNode
        {

            public override char Operator => '+';
            public override int Precedence => 1;
            public override string Associativity => "Left";

            public AddNode()
            {
            }

            public override double Evaluate()
            {
                return this.Left.Evaluate() + this.Right.Evaluate();
            }
        }
    }
}
