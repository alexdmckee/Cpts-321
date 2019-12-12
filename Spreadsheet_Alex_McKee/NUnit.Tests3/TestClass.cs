// Alex McKee
// 011659950

// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace NUnit.Tests3
{
    [TestFixture]

    // Test the expression tree.
    public class TestClass
    {

        // For the following tests, the name is explanatory enough to document what the test does. Hence no documentation for each. 
        

        [Test]
        public void TestEval()
        {

            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1+2+3+4");
            Assert.AreEqual(10.0, test.Evaluate());
        }

        [Test]
        public void TestEval2()
        {

            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1+2");
            Assert.AreEqual(3.0, test.Evaluate());
        }

        [Test]
        public void TestEval3()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1*2*3");
            Assert.AreEqual(6.0, test.Evaluate());
        }

        [Test]
        public void TestEval4()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1");
            Assert.AreEqual(1.0, test.Evaluate());
        }

        [Test]

        // Test the shunting-yard algorithm in PostFixExpression
        public void TestPostFixConversion()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1");

            // Standard expression
            string expression1 = "x + y / (5 * z) + 10";
            Assert.AreEqual("x y 5 z * / + 10 + ", test.PostFixExpression(expression1));
            
    
        }

        [Test]

        // Test the shunting-yard algorithm in PostFixExpression
        public void TestPostFixConversionMultipleParenthesis()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1");

            // Multiple parenthesesis
            string expression2 = "(((((2+2)))))";
            Assert.AreEqual("2 2 + ", test.PostFixExpression(expression2));
        }

        [Test]

        // Test the shunting-yard algorithm in PostFixExpression
        public void TestPostFixConversionMismatchedParens()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1");

            // Mismatched Parentheses
            string expression3 = "((((2+1)))";
            Assert.AreEqual("Mismatched Parentheses", test.PostFixExpression(expression3));
        }

        [Test]

        // Test the shunting-yard algorithm in PostFixExpression
        public void TestPostFixConverion2()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("1");

            string expression3 = "2 +  A1 + B2 * (55/19)";
            Assert.AreEqual("2 A1 + B2 55 19 / * + ", test.PostFixExpression(expression3));

        }

    }
}
