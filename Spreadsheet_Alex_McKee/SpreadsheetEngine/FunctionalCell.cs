// Alex McKee
// 011659950

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{

    // Class to be used in the spreadsheet. It inherits from abstract cell class.
    /// <summary>
    /// Factory cell.
    /// </summary>
    public class FunctionalCell : Cell
    {
        // Base keyword allows the row and col passed to "functional cell" to be used in the abstract class in a sense
        public FunctionalCell(int row, int col) : base(row, col)
        {

        }

       //  The big hint for this: It’s a protected property which means 
        // inheriting classes can see it.Inheriting classes should NOT be publically exposed to code outside the class library.
        // Make protected or privatE?

        /// <summary>
        /// Gets or sets value of a cell.
        /// </summary>
        public new string Value
        {
            get => value;

            set
            {
                    this.value = value; // comes from formula in the spreadsheet class. Under CellPropertyChanged function.
            }
        }

    }
}
