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
    public class TestClassforHW8
    {
        [Test]

        // Test BGColor property
        public void TestColorModify()
        {
            Spreadsheet testSheet = new Spreadsheet(2, 2);
            testSheet.GetCell(0, 0).Text = "ab";
            testSheet.GetCell(0, 0).BGColor = (uint)ConsoleColor.Cyan;
            Assert.AreEqual(testSheet.GetCell(0, 0).BGColor, (uint)ConsoleColor.Cyan);
        }

        [Test]

        // Tests the Undo process for a spreadsheet.
        public void TestUndo()
        {
            Spreadsheet testSheet = new Spreadsheet(2, 2);
            Cell A1 = testSheet.GetCell(0, 0);

            testSheet.GetCell(0, 0).Text = "w1";
            testSheet.AddUndo(new CellValueChange(A1, "12"));
            testSheet.UndoCmd();

            Assert.AreEqual(testSheet.GetCell(0, 0).Text, "12");
        }

        [Test]

        // Tests the Redo process for a spreadsheet.
        public void TestRedo()
        {
            Spreadsheet testSheet = new Spreadsheet(2, 2);
            Cell A1 = testSheet.GetCell(0, 0);

            testSheet.GetCell(0, 0).Text = "w1";
            testSheet.AddUndo(new CellValueChange(A1, "a2"));
            testSheet.UndoCmd();
            testSheet.ExecuteRedo();

            Assert.AreEqual(testSheet.GetCell("A1").Text, "w1");
        }
    }
}
