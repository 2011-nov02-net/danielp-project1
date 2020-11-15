using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.ConsoleView
{
    /// <summary>
    /// Represents the current state of the program.
    /// </summary>
    interface IMenu
    {
        //Display
        /// <summary>
        /// Handles input for the current menu, displays anything, then 
        /// </summary>
        /// <returns>Returns the next IMenu (state), or null to exit the program.</returns>
        IMenu DisplayMenu();
    }
}
