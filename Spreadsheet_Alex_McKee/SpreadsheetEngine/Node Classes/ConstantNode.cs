namespace CptS321
{
    public partial class ExpressionTree
    {
        // ConstantNode. i.e. a number 1, 2, 3, etc. Inherits from Node
        // Will be getting value from expression STRING.
        private class ConstantNode : Node
        {
            private readonly double number;

            public ConstantNode(double number)
            {
                this.number = number;
            }

            public override double Evaluate()
            {
                return this.number;
            }

        }
    }

}