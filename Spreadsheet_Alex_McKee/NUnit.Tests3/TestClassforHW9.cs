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
    public class TestClassforHW9
    {
        [Test]

        // Checks if the list of cells changed has the right number of cells.
        public void TestCellsChanged()
        {
            Spreadsheet testSheet = new Spreadsheet(4, 4);
            testSheet.GetCell(0, 0).Text = "test";
            testSheet.GetCell(0, 1).Text = "Text";
            testSheet.GetCell(0, 1).BGColor = 0xFFFFFF00;

            int numChanged = testSheet.CellsChangedCount();
            Assert.AreEqual(2, numChanged);
        }


        [Test]

        // Checks if the save functionality to xml works...
        //a xml document is created that is easy to scan through tho...
        public void TestXMLSave()
        {
            Assert.Fail();
        }

        [Test]

        // Checks if the spreadsheet load functionality works..
        // not sure how to select a XML file from personal computer that would be valid for third party testers...
        public void TestXMLLoad()
        {
            Assert.Fail();
        }
    }
}
