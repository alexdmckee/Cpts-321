using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Class to hold cells affected by previous command (change text, change bgColor, etc.) and what their last values were.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 
        /// </summary>

        string Title { get;}

        void Undo();
        void Redo();
    }
}