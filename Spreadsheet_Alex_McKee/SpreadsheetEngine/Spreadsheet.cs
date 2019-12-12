// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// Alex McKee
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CptS321
{
    /// <summary>
    /// Layer under datagridview to perform excel like operations and keep track of values etc.
    /// </summary>
    public class Spreadsheet
    {
        // Fields.
        private Cell[,] Cells; // Leaving uppercase as this is an important field.
        private int rowCount;
        private int colCount;
        private List<string> validCellNames;
        public Dictionary<string, List<string>> references;
        private Stack<ICommand> Undo = new Stack<ICommand>();
        private Stack<ICommand> Redo = new Stack<ICommand>();
        public event PropertyChangedEventHandler PropertyChanged; // delcare event

        // Utility functions.
        public int RowCount { get => rowCount; }
        public int ColCount { get => colCount; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Spreadsheet constructor.
        /// </summary>
        /// <param name="numRows"> Number of rows in spreadsheet. </param>
        /// <param name="numCols"> Number of cols in spreadsheet. </param>
        public Spreadsheet(int numRows, int numCols)
        {
            this.rowCount = numRows;
            this.colCount = numCols;

            validCellNames = new List<string>();
            references = new Dictionary<string, List<string>>();

            // Create a 2d array to be populated array with new cells
            Cells = new FunctionalCell[numRows, numCols]; // 2d Cell array

            // Flesh out Cells array with cells. 
            for (int curRow = 0; curRow < numRows; curRow++)
            {
                for (int curCol = 0; curCol < numCols; curCol++)
                {
                    Cells[curRow, curCol] = new FunctionalCell(curRow, curCol);
                    Cells[curRow, curCol].PropertyChanged += CellPropertyChanged;  // cell in 2d array is propertyChanged in binded to onpropertchanged
                    validCellNames.Add(GetCellName(curRow, curCol));
                    references.Add(GetCellName(curRow, curCol), new List<string>());
                }
            }

            Debug.WriteLine("At end of spreadsheet constructor");
        }

       
        /// <summary>
        /// CellPropertyChanged event.  Sets a vlue for a cell if its text has changed.
        /// </summary>
        /// <param name="sender"> cell in spreadsheet where the text has changed. </param>
        /// <param name="e"> event arguments. </param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Sender will be a FunctionalCell from the spreadsheet
            // Need to account for if first char is =
            // "cell" is DataGridView cell that has been updated
            FunctionalCell cell = sender as FunctionalCell;
            string cellNombre = GetCellName(cell.RowIndex, cell.ColumnIndex);

            int fromCellRow;
            int fromCellColumn;
            Debug.WriteLine("In CellPropertyChanged");
            if (cell.Text == "" || cell.Text.Length == 1)
            {
                cell.Value = cell.Text;
            }

           else if (e.PropertyName == "Text" || e.PropertyName == "OnReferenceCellChanged")
            {
                int alphabetCounter = Regex.Matches(cell.Text, @"[a-zA-Z]").Count;


                string expression = cell.Text;
                expression = expression.Substring(1);

                ExpressionTree tree = new ExpressionTree(expression);
                string[] cellNames = tree.GetVariableNames();
                List<string> listNames = cellNames.ToList<string>();

                if (cell.Text.Contains(cellNombre))
                {
                    cell.Value = "!(self-ref)";
                }

                else if (CheckBadReference(cellNames))
                {
                    cell.Value = "!(bad-ref";
                }

                else if (CheckCircularReference(cellNombre, listNames))
                {
                    cell.Value = "!(circular-ref)";

                    //Clear old references, dont want them to update the cell.
                    ClearOldReferences(cell);

                    // add refereces incase one changes to break circular reference. 

                    foreach (string cellName in cellNames)
                    {
                        // add references

                        Cell referenceCell = GetCell(cellName);
                        references[cellNombre].Add(cellName);
                        if (referenceCell != null && referenceCell != cell)
                        {
                            // For future HW's need to unsubscribe dependencies when expression changes (-=)?
                            referenceCell.PropertyChanged -= cell.OnReferenceChange;
                            referenceCell.PropertyChanged += cell.OnReferenceChange;
                        }
                    }
                    }

                else if (cell.Text.StartsWith("="))
                {

                    if (cell.Text[1] >= '0' && cell.Text[1] <= '9' && alphabetCounter == 0)
                    {

                        cell.Value = tree.Evaluate().ToString();

                        // clear old references for cell
                        ClearOldReferences(cell);
                    }

                    // Cell contains a formula and we need an expression tree to evaluate
                    else
                    {
                        // Flow of steps:
                        // get expression from = on. (C1+D10)
                        // build expression tree
                        // in expression tree get (string) values of C1 and D10 from GetCell

                        // Clear old references
                        ClearOldReferences(cell);

                        foreach (string cellName in cellNames)
                        {
                            // add references
                            
                            Cell referenceCell = GetCell(cellName);
                            references[cellNombre].Add(cellName);
                            if (referenceCell != null && referenceCell != cell)
                            {
                                // For future HW's need to unsubscribe dependencies when expression changes (-=)?
                                referenceCell.PropertyChanged -= cell.OnReferenceChange;
                                referenceCell.PropertyChanged += cell.OnReferenceChange;
                            }

                            if (double.TryParse(GetCell(cellName).Value, out double val))
                            {
                                tree.SetVariable(cellName, val);

                            }
                            else
                            {
                                tree.SetVariable(cellName, 0.0);
                            }
                        }

                        cell.Value = tree.Evaluate().ToString();
                    }
                }

                else
                {
                    Debug.WriteLine("In cell propert cahnged, did not start with =");
                    cell.Value = cell.Text;
                }
            }

            Debug.WriteLine(cellNombre + " Value has changed to " + GetCell(cell.RowIndex, cell.ColumnIndex).Value + " from " + e.PropertyName);

            PropertyChanged?.Invoke(this.GetCell(cell.RowIndex, cell.ColumnIndex), new PropertyChangedEventArgs(e.PropertyName)); // This was throwing that the sender was null in HandCellPropertyChanged event
        }


        private void ClearOldReferences(Cell cell)
        {
            string cellNombre = GetCellName(cell.RowIndex, cell.ColumnIndex);

            // Clear out old  references and unsubsribe from their propertyChange event
            List<string> oldRefs = references[cellNombre]; // no longer has any references
            foreach (string oldRef in oldRefs)
            {
                Cell oldReference = GetCell(oldRef);
                oldReference.PropertyChanged -= cell.OnReferenceChange;  // B1 no longer updates A1
            }

            references[cellNombre].Clear();
        }
    // So to summarize, this Value property is a getter only and you’ll have to implement a way
    //  to allow the spreadsheet class to set the value, but no other class can
    // {Create a new cell at the Cells 2d array location with a new value??   }
    // { Indirectly set value through property changed event? When text changes... } 

    /// <summary>
    /// Returns a functional cell from the Cells 2d array
    /// </summary>
    /// <param name="row">row </param>
    /// <param name="col">col </param>
    /// <returns> functional cell at positions row,col</returns>
    public Cell GetCell(int row, int col)
        {
            if (row >= 0 && row <= RowCount && col >= 0 && col <= ColCount)
            {
                return (FunctionalCell)this.Cells[row, col];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns cell.
        /// </summary>
        /// <param name="cellName"> String rep. A1 or B2 etc. </param>
        /// <returns> cell at location.
        /// </returns>
        public Cell GetCell(string cellName)
        {
            int col = cellName[0] - 65;
            int row = int.Parse(cellName.Substring(1)) - 1;

            return GetCell(row, col);
        }

        /// <summary>
        /// Return string name of cell (A1 etc.) from coordinates.
        /// </summary>
        /// <param name="row"> row. </param>
        /// <param name="col"> col. </param>
        /// <returns> ex: A1 </returns>
        public string GetCellName(int row, int col)
        {
            string name = ((char)(col + 65)).ToString() + (row + 1).ToString();
            return name;
        }


        /// <summary>
        /// Adds a command to spreadsheets Undo stack.
        /// </summary>
        /// <param name="cmd"> cmd. (</param>
        public void AddUndo(ICommand cmd)
        {
            Undo.Push(cmd);
        }

        /// <summary>
        /// Keep Undo stack encapsulated.
        /// </summary>
        public void UndoCmd()
        {
            try
            {
                ICommand cmd = Undo.Pop();
                Redo.Push(cmd);
                cmd.Undo();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Keeps Undo and Redo Stack encapsulated.
        /// </summary>
        public void ExecuteRedo()
        {
            ICommand cmd = Redo.Pop();
            Undo.Push(cmd);
            cmd.Redo();
        }


        /// <summary>
        /// Keeps Undo Stack encapsulated.  Used in Form class.
        /// </summary>
        /// <returns>
        /// Number of items in the stack
        /// </returns>
        public int UndoStackCount()
        {
            return Undo.Count;
        }

        /// <summary>
        /// Keeps the Redo stack encapsulated. Used in Form class.
        /// </summary>
        /// <returns>Number of items in the Redo stack./// </returns>
        public int RedoStackCount()
        {
            return Redo.Count;
        }


        /// <summary>
        /// Keeps Undo encapsalted. Used to get title for dropdown menu item.
        /// </summary>
        /// <returns> cmd title. </returns>
        public string UndoStackTopTitle()
        {
            try
            {
                ICommand cmd = Undo.Peek();
                return cmd.Title;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "Error in UndoStackTopTitle.";
            }
        }

        /// <summary>
        /// Keeps Redo stack encapsulated. Used to get title for dropdown menu item.
        /// </summary>
        /// <returns> Redo stack top title. </returns>
        public string RedoStackTopTitle()
        {
            try
            {
                ICommand cmd = Redo.Peek();
                return cmd.Title;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "Error in RedoStackTopTitle.";
            }
        }


        /// <summary>
        /// Clears all elements from both stacks.
        /// </summary>
        private void ClearStacks()
        {
            while (Redo.Count > 0)
            {
                Redo.Pop();
            }

            while (Undo.Count > 0)
            {
                Undo.Pop();
            }
        }

        /// <summary>
        /// Saves a spreadsheet changed cells to an XML document. 
        /// </summary>
        /// <param name="s"> File stream to save to. </param>
        public void Save (Stream s)
        {

            List<Cell> cellsChanged = CellsChanged();

            // XmlWriterSettings settings = new XmlWriterSettings();
            // XmlWriter writer = XmlWriter.Create(s, settings);
            XDocument doc = new XDocument(new XComment("Saving all non empty cells into XML format"),

               new XElement("SpreadsheetCells", // first element

                   from cell in cellsChanged  // XDoc supports LINQ. Iterate over changed cells.
                   select new XElement(
                       "Cell",
                       new XElement("name", (((char)(cell.ColumnIndex + 65)).ToString()) + (cell.RowIndex + 1).ToString()),
                       new XElement("Text", cell.Text),
                       new XElement("BGColor", cell.BGColor.ToString()))
               ));

            doc.Save(s); 
        } // end save

        /// <summary>
        /// Loads a spreadsheets cells from a XML Document.
        /// </summary>
        /// <param name="s"> File stream. </param>
        public void Load(Stream s)
        {
            XDocument savedSheet = XDocument.Load(s);

            // Clear spreadsheet
            for (int row = 0; row < this.RowCount; row++)
            {
                for (int col = 0; col < this.ColCount; col++)
                {
                    Cell tempCell = GetCell(row, col);
                    tempCell = (FunctionalCell)tempCell;

                    tempCell.Text = "";
                    tempCell.BGColor = 0xFFFFFFFF;
                }
            }

            // For each cell element block in savedSheet (XDocument).
            foreach (XElement cell in savedSheet.Descendants("Cell"))
            {
                // Identifiers.
                string cellName = null;
                string savedText = "";
                uint savedBGColor = 0xFFFFFFFF;

                // For each cell element between <cell> </cell>
                foreach (XElement cellElement in cell.Descendants())
                {
                    if (cellElement.Name == "name")
                    {
                        cellName = cellElement.Value;
                    }
                    else if (cellElement.Name == "Text")
                    {
                        savedText = cellElement.Value;
                    }
                    else if (cellElement.Name == "BGColor")
                    {
                        if (uint.TryParse(cellElement.Value, out uint val))
                        {
                            savedBGColor = val;
                        }
                    }

                    if (cellName != null)
                    {
                        GetCell(cellName).Text = savedText;
                        GetCell(cellName).BGColor = savedBGColor;
                    }

                }

                // Clear both stacks.
                ClearStacks();
            }
        }

        /// <summary>
        /// Gets a list of the cells changed in the spreadsheet.
        /// </summary>
        /// <returns> List of Cell(s) changed. </returns>
        private List<Cell> CellsChanged()
        {
            List<Cell> cellsChanged = new List<Cell>();
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    Cell cell = this.GetCell(row, col);
                    if (cell.Text != null || cell.BGColor != 0xFFFFFFFF)
                    {
                        if (cell.Text == null)
                        {
                            cell.Text = "";
                        }

                        cellsChanged.Add(cell);
                    }
                }
            }

            return cellsChanged;
        }


        /// <summary>
        /// Used in test.
        /// </summary>
        /// <returns> num cells changed. </returns>
        public int CellsChangedCount()
        {
            List<Cell> test = CellsChanged();
            return test.Count;
        }


        /// <summary>
        /// See if variable is not a cell name.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public bool CheckBadReference(string[] variables)
        {
            bool returnVal = false;

            foreach (string var in variables)
            {
                if (!validCellNames.Contains(var))
                {
                    returnVal = true;
                }
            }

            return returnVal;
        }


        /// <summary>
        /// Checks for ciruclar references.
        /// </summary>
        /// <param name="initial"> initial cell. </param>
        /// <param name="listReferences"> list of cells it references. </param>
        /// <returns> True if contains circular reference. </returns>
        public bool CheckCircularReference(string initial, List<string> listReferences)
        {
            // look through list of references, for each reference cell.
            // base case is looked through all
            bool returnVal = false;

            foreach (string referenceName in listReferences)
            {
                if (initial == referenceName)
                {
                    return true;
                }

                return CheckCircularReference(initial, references[referenceName]);
            }

            return returnVal;
        }
    }
}
