using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace NUnit.Tests3
{
    [TestFixture]
    public class TestClassForHW10
    {
        [Test]

        // Test getting a cell name from its indices.
        public void TestGetCellName()
        {
            Spreadsheet testSheet = new Spreadsheet(2, 2);
            string name = testSheet.GetCellName(0, 0);

            Assert.AreEqual("A1", name);
        }

        [Test]

        // Test checking if a expression contain a bad reference. 
        public void TestCheckBadReference()
        {
            Spreadsheet testSheet = new Spreadsheet(2, 2);
            string[] variables = { "A1", "Z1" };
            bool tf = testSheet.CheckBadReference(variables);

            Assert.AreEqual(true, tf);
        }

        [Test]

        // Test to see if there is a circular reference.
        public void TestCheckCircularReference()
        {
            Spreadsheet testSheet = new Spreadsheet(2, 2);

            testSheet.GetCell(0, 0).Text = "=B1";
            testSheet.GetCell(0, 1).Text = "=A1";
          
            Assert.AreEqual(true, testSheet.CheckCircularReference("B1", testSheet.references["A1"]));
        }
    }
}
