// Alex McKee

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{

    /// <summary>
    /// Cell wil give you a place (rowIndex, colIndex) in the spreadsheet, and if the cell value is changed it will fire off a propertychanged event to dependant (linked) cells.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        // fields.
        protected string value;
        protected uint BGcolor;
        private string text;
        private int rowIndex;
        private int columnIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowIndex"> row </param>
        /// <param name="columnIndex"> col </param>
        protected Cell(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            BGcolor = 0xFFFFFFFF;
        }

        
        /// <summary>
        /// Properties to get the private values, dont want ability to set them outside of class
        /// </summary>
        public int ColumnIndex { get => columnIndex; }

        public int RowIndex { get => rowIndex;  }

        /// <summary>
        /// Gets and set text field.
        /// </summary>
        public string Text
        {
            get => text;

            set
            {
                if (text != value)
                {
                    Debug.WriteLine("In Cell Text propert");
                    text = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                   // ReferencePropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public uint BGColor
        {
            get => BGcolor;

            set
            {
                if (BGcolor != value)
                {
                    Debug.WriteLine("In Cell BGColor property changing color to" + (ConsoleColor)value + "... ");
                    BGcolor = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("BGColor"));
                }
            }
        }


        /// <summary>
        /// Event to update an dependant cell if a reference cell changes.
        /// </summary>
        /// <param name="sender"> reference cell. </param>
        /// <param name="e"> Event arguments. </param>
        public void OnReferenceChange(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("In OnReferenceChange");
            Cell senderCell = sender as Cell;

            if (senderCell.Value != "!(circular-ref)")
            {
                PropertyChanged(this, new PropertyChangedEventArgs("OnReferenceCellChanged"));
            }
        }

        // So to summarize, this Value property is a getter only and you’ll have to .
        // implement a way to allow the spreadsheet class to set the value, but no other class can.
        public string Value { get => value; } //set => this.value = value; }
    }
}
