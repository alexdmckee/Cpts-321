using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CptS321
{
    public class BGColorChange : ICommand
    {
        // Fields
        public string title = "BGColorChange";  // Field to show name in dropdodwn menu (Undo ____).
        List<Cell> cells;
        List<uint> oldColors;
        uint newColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BGColorChange"/> class.
        /// Constructor. 
        /// </summary>
        /// <param name="cells"> List of Cells affected in spreadsheet</param>
        /// <param name="oldColors"> the cells old colors. </param>
        public BGColorChange(List<Cell> cells, List<uint> oldColors)
        {
            this.cells = cells;
            this.oldColors = oldColors;
            this.newColor = cells[0].BGColor;
        }

        public string Title
        {
            get => title;
        }

        /// <summary>
        /// Changes each cell in list of cells to the newColor.
        /// </summary>
        public void Redo()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].BGColor = newColor;
            }
        }

        /// <summary>
        /// Changes each cell in the list of cells back to its old color.
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Cell cell = cells[i];
                uint color = oldColors[i];

                cell.BGColor = color;
            }
        }
    }
}
