// Alex McKee
// 011659950

using System.Collections.Generic;

namespace CptS321
{
    public partial class ExpressionTree
    {
        private class VariableNode : Node
        {
            private string name;
            private double value;
            private Dictionary<string, double> variables;

           // public string Name { get => name; set => name = value; }
           //  public double Value { get => value; set => this.value = value; }

            public VariableNode(string variable, ref Dictionary<string, double> variables)
            {
                this.name = variable;
                this.variables = variables;
            }

            public override double Evaluate()
            {
                // if varaible not set, return 0.0
                double retVal = 0.0;
                if (this.variables.ContainsKey(this.name))
                {
                    retVal = variables[name];
                }

                return retVal;
            }
        }
    }

}