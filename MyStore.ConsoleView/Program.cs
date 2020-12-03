using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MyStore.DataModel;
using MyStore.Store;

namespace MyStore.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptions<MyStoreDbContext> dbContextOptions = SetupContextOptions();

            if(dbContextOptions == null)
            {
                Console.WriteLine("Exiting Program . . .");
                return;
            }

            IDbRepository repo = new DBRepositoryNoConnection(dbContextOptions);
            Console.WriteLine("Welcome to the store!");

            Console.WriteLine("Loading database data . . .");
            repo.LoadDBDataToModel();

            //basically hold the current state of the program
            IMenu CurrentMenu = new StartMenu(repo);

            while( CurrentMenu != null)
            {
                CurrentMenu = CurrentMenu.DisplayMenu();
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Checks if the input string from a user was valid.
        /// </summary>
        /// <param name="input">The user's input string. Will be trimmed and set to lowercase.</param>
        /// <param name="validInput">The list of valid input strings.</param>
        /// <param name="ChoiceIndex">The index of the user's choice in the validInput array, or -1 if it was not in the array.</param>
        /// <returns></returns>
        internal static bool ValidOption(string input, List<string> validInput, out int ChoiceIndex)
        {
            //normalize input
            input = input.Trim().ToLower();
            ChoiceIndex = -1;
            bool WasValid = false;

            for(int i = 0; i < validInput.Count && !WasValid; i++)
            {
                if(validInput[i] == input)
                {
                    ChoiceIndex = i;
                    WasValid = true;
                }
            }

            if (!WasValid)
            {
                Console.WriteLine($"\"{input}\" Was not recognized as valid input.");

                Console.Write("Please choose from:");

                foreach(string option in validInput)
                {
                    Console.Write($" \"{option}\",");
                }

                Console.WriteLine();
            }

            return WasValid;
        }


        /// <summary>
        /// Reads and validates a response to a yes or no question already asked.
        /// </summary>
        /// <returns> 0 for yes, 1 for no. </returns>
        public static int ValidYesNoOption()
        {
            int choice;
            //wait until valid response.
            while (!Program.ValidOption(Console.ReadLine(), new List<string> { "y", "n" }, out choice))
            {

            }
            
            return choice;
        }

        /// <summary>
        /// Get any integer amount
        /// </summary>
        /// <returns>The Amount</returns>
        public static int GetIntegerAmount()
        {
            bool isvalid = false;
            int userInput;

            do
            {
                string inputStr = Console.ReadLine().Trim();
                isvalid = Int32.TryParse(inputStr, out userInput);

                if (!isvalid)
                {
                    Console.WriteLine($"Please input a number. The program could not interpret {userInput} properly.");
                }

            } while (!isvalid);

            return userInput;
        }

        /// <summary>
        /// Get a number from the user within the range given inclusive.
        /// </summary>
        /// <param name="max">The maximum number the user can select inclusive.</param>
        /// <param name="min">The minimum number the user can select inclusive.</param>
        /// <returns></returns>
        public static int GetIntegerAmount(int max, int min = 0)
        {
            if( max < min)
            {
                int temp = min;
                min = max;
                max = temp;
            }

            bool isvalid = false;
            int userInput;

            do
            {
                string inputStr = Console.ReadLine().Trim();
                isvalid = Int32.TryParse(inputStr, out userInput);

                if (!isvalid)
                {
                    Console.WriteLine($"Please input a number. The program could not interpret {userInput} properly.");
                } else
                {
                    isvalid = isvalid && userInput >= min && userInput <= max;
                    if (userInput < min)
                    {
                        Console.WriteLine($"Error, must be greater than {min}.");
                    } else if (userInput > max)
                    {
                        Console.WriteLine($"Error, must be less than {max}.");
                    }
                }

            } while (!isvalid);

            return userInput;
        }


        private static DbContextOptions<MyStoreDbContext> SetupContextOptions()
        {
            DbContextOptionsBuilder<MyStoreDbContext> optionsBuilder = new DbContextOptionsBuilder<MyStoreDbContext>();
            string fileloc = "./../../../../MyStore.dataModel/ConnectionString.txt";
            if (!File.Exists(fileloc))
            {
                Console.WriteLine($"Error: Expected a file called \"ConnectionString.txt\" at {fileloc} holding only the database connection string.");
                return null;
            }

            string connectionStr = File.ReadAllText(fileloc);

            optionsBuilder.UseSqlServer(connectionStr);
            return optionsBuilder.Options;
        }
    }
}
