using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class CellValueChange : ICommand
    {
        /// <summary>
        /// Fields.
        /// </summary>
        Cell cellChanged;
        private string title = "Text Change";
        string oldText;
        string newText;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cell"> cell that has been changed. </param>
        /// <param name="oldText"> cells oldText. </param>
        public CellValueChange(Cell cell, string oldText)
        {
            cellChanged = cell;
            this.oldText = oldText;
            this.newText = cell.Text;
        }

        public string Title
        {
            get => title; 
        }


        /// <summary>
        /// Resets a cells text to its previous value.
        /// </summary>
        public void Undo()
        {
            if (oldText != null)
                this.cellChanged.Text = oldText;
            else
            {
                this.cellChanged.Text = "";
            }
        }

        /// <summary>
        /// Undoes an undo. ;).
        /// </summary>
        public void Redo()
        {
            cellChanged.Text = newText;
        }
    }

}
