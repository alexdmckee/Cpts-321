// Alex McKee
// CPTS 321

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CptS321;

namespace Spreadsheet_Alex_McKee
{
   public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet;

        public Form1()
        {

            InitializeComponent();
            reduToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;

            // Add columns A-Z
            for (char c = 'A'; c <= 'Z'; c++)
            {
                dataGridView1.Columns.Add(c.ToString(), c.ToString());
            }

            // Add 50 rows to the dataGrid
            dataGridView1.Rows.Add(50);

            // Change dataGrids rows header cell values to 1-50
            for (int i = 1; i <= 50; i++)
            {

                dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }

            spreadsheet = new Spreadsheet(50,26);
            spreadsheet.PropertyChanged += HandleCellPropertyChanged;

        }

        // Bubbles up the change in the spreadsheet to the dataGridView
        private void HandleCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FunctionalCell cell = sender as FunctionalCell;

            dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex).Value;
            dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = Color.FromArgb((int)spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex).BGColor);


            // Mechanics to update Undo and Redo dropdown menu items. 
            if (spreadsheet.UndoStackCount() == 0)
            {
                undoToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Text = "Undo";
            }

            else
            {
                undoToolStripMenuItem.Enabled = true;
                undoToolStripMenuItem.Text = "Undo " + spreadsheet.UndoStackTopTitle();
            }

            if (spreadsheet.RedoStackCount() == 0)
            {
                reduToolStripMenuItem.Enabled = false;
                reduToolStripMenuItem.Text = "Redo ";
            }

            else
            {
                reduToolStripMenuItem.Enabled = true;
                reduToolStripMenuItem.Text = "Redo " + spreadsheet.RedoStackTopTitle();
            }
        }

        // Update the spreadsheet cells whenever the user changes a value of any cell.
        // This starts the cascade of propertychange events
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            this.spreadsheet.GetCell(e.ColumnIndex, e.RowIndex).Text = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();

        }

        // When user focus first clicks on cell this is called
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // show what the cell text is to the datagrid
            // get the cells text
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        // When users focus leaves cell, this is called.
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            Cell cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);
            try
            {
                // if the edit didnt change anything, make it so the value still gets displayed in grid, instead of text.
                if (this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Value;
                }

                // The datagrids text was changed, which was showing the Cell text, thus
                // Cell text has changed. Update spreadsheet and save command to undo stack.
                else
                {
                    string oldText = cell.Text;


                    this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    CellValueChange cellValueChange = new CellValueChange(cell, oldText);
                    this.spreadsheet.AddUndo(cellValueChange);
                    undoToolStripMenuItem.Text = "Undo " + cellValueChange.Title;
                    undoToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }


        // Button to generate updates to UI from spreadsheet.
        private void button1_Click(object sender, EventArgs e)
        {

           Random rand = new Random();

            // 1. Set the values of ~50 cells to a string of your choice
           for (int i = 0; i < 50; i++)
            {
                this.spreadsheet.GetCell(rand.Next(0,20), rand.Next(2, 10)).Text = "Update from spreadsheet engine"; // How to access cells while keeping Cells private....
            }

           for (int i = 0; i < 50; i++)
            {
                this.spreadsheet.GetCell(i, 1).Text = "This is B" + (i + 1).ToString(); // How to access cells while keeping Cells private....
            }

           for (int i = 0; i < 50; i++)
            {
                this.spreadsheet.GetCell(i, 0).Text = "=B" + (i + 1).ToString(); // How to access cells while keeping Cells private....
            }
        }


        /// <summary>
        /// Opens the modify color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Cell> cells = new List<Cell>();
            List<uint> oldColors = new List<uint>();
            ColorDialog myDialog = new ColorDialog();


            // Allows the user to select a custom color.
            myDialog.AllowFullOpen = true;

            // Update the text box color if the user clicks OK
            if (myDialog.ShowDialog() == DialogResult.OK)
            {
                uint color = (uint)myDialog.Color.ToArgb();
                foreach (DataGridViewTextBoxCell cell in this.dataGridView1.SelectedCells)
                {
                    Debug.WriteLine(cell.ColumnIndex);
                    Debug.WriteLine(cell.RowIndex);

                    cells.Add(this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex));
                    oldColors.Add(this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex).BGColor);
                    this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex).BGColor = (uint)myDialog.Color.ToArgb();
                }
                BGColorChange bGColorChange = new BGColorChange(cells, oldColors);
                spreadsheet.AddUndo(bGColorChange);
                undoToolStripMenuItem.Text = "Undo " + bGColorChange.title;
                undoToolStripMenuItem.Enabled = true;

            }

        }

        /// <summary>
        /// Exucutes the undo cmd.
        /// </summary>
        /// <param name="sender"> user click </param>
        /// <param name="e"> event. </param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheet.UndoCmd();
        }

        /// <summary>
        /// Exucutes a Redo.
        /// </summary>
        /// <param name="sender"> user click. </param>
        /// <param name="e"> event. </param>
        private void reduToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheet.ExecuteRedo();
        }

        /// <summary>
        /// Toolstrip menu item to save a spreadsheets changed cells to an XML doc.
        /// </summary>
        /// <param name="sender"> user. </param>
        /// <param name="e"> save. </param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML File|*.xml";
            saveFileDialog1.Title = "Save an XML File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != string.Empty)
            {
                // Fs contains the stream for the user specified file location via OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();

                // Call the save method from the spreadsheet class.
                this.spreadsheet.Save(fs);

                fs.Close();
            }
        }

        /// <summary>
        /// Toolstrip menu item to load a previously saved spreadsheets changed cells 
        /// back to the GUI.
        /// </summary>
        /// <param name="sender"> user. </param>
        /// <param name="e"> load. </param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML File|*.xml";
            openFileDialog1.Title = "Open an XML File";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != string.Empty)
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)openFileDialog1.OpenFile();

                this.spreadsheet.Load(fs);

                fs.Close();
            }

        }


        // The following functions cause an error when deleted. So are aggregated near the bottom.
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

    }
}
