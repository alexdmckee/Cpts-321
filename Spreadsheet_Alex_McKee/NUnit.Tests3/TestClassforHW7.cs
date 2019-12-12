/// Alex McKee
/// WSU id: 011659950

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnit.Tests3
{
    [TestFixture]
    public class TestClassforHW7
    {

        [Test]

        // Tests for division by zero.
        public void TestDivideByZeroExeption()
        {
            CptS321.ExpressionTree tree = new CptS321.ExpressionTree("2/0");
            Assert.Throws<DivideByZeroException>(new TestDelegate(() => tree.Evaluate()));
        }

        [Test]

        // test for complicated division by zero
        public void TestDivideByZeroExeptionComplicated()
        {
            CptS321.ExpressionTree tree = new CptS321.ExpressionTree("2+2*4/0");
            Assert.Throws<DivideByZeroException>(new TestDelegate(() => tree.Evaluate()));
        }

        [Test]

        // Test to see if updates variables in tree correctly
        public void TestGetVariableNames()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("A1+B1");
            Assert.AreEqual(2, test.GetVariableNames().Length);
        }

        [Test]

        // Test to see if GetVariableNames string array is right size.
        public void TestGetVariableNamesWithNumbers()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("A1+B1/2+1");
            Assert.AreEqual(2, test.GetVariableNames().Length);
        }


        [Test]

        // Test to see if GetVariableNames gets the right variables.
        public void TestGetVariableNamesGetsCorrectVariables()
        {
            CptS321.ExpressionTree test = new CptS321.ExpressionTree("A1+B1/2+1");
            string[] variables = test.GetVariableNames();

            Assert.AreEqual("A1", variables[0]);
            Assert.AreEqual("B1", variables[1]);
        }

        [Test]

        // Test to see if a cell expression with dependant cells, updates when 
        // the dependant cells do.
        public void TestSpreadsheetFormulaDependecies()
        {
            CptS321.Spreadsheet testSheet = new CptS321.Spreadsheet(4, 4);
            testSheet.GetCell("B1").Text = "10";
            testSheet.GetCell("C1").Text = "10";
            testSheet.GetCell("A1").Text = "=B1+C1";
            testSheet.GetCell("C1").Text = "20";

            Assert.AreEqual("30", testSheet.GetCell("A1").Value);

        }

        [Test]

        // Test to see if a cell expression with dependant cells, updates when 
        // the dependant cells do.
        public void TestSpreadsheetFormulaDependeciesBoundaryCondition()
        {
            CptS321.Spreadsheet testSheet = new CptS321.Spreadsheet(4, 4);
            testSheet.GetCell("C1").Text = "10";
            testSheet.GetCell("A1").Text = "=B1+C1";

            Assert.AreEqual("10", testSheet.GetCell("A1").Value);

        }
    }
}
