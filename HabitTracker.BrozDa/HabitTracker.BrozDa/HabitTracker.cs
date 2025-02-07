﻿namespace HabitTracker.BrozDa
{
    internal class HabitTracker
    {
        private DatabaseManager _databaseManager;
        private InputOutputManager _IOmanager;
        public HabitTracker()
        {
            _databaseManager = new DatabaseManager();
            _IOmanager = new InputOutputManager();
        }


        public void Start()
        {
            //_databaseManager.CreateNewTable("WaterIntake");
            //_databaseManager.GetTableColumnNames("WaterIntake");
            
            while (true) 
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                PrintMainMenu();
                ProcessUserInput();
            }
            
        }
        public void PrintMainMenu()
        {
            Console.WriteLine("Welcome to habit tracker application");
            Console.WriteLine("Habit tracker tracks number of glasses of water drank during the day");
            Console.WriteLine();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. View all records");
            Console.WriteLine("\t2. Insert record");
            Console.WriteLine("\t3. Update record");
            Console.WriteLine("\t4. Delete record");
            Console.WriteLine("\t5. Close the application");

            Console.Write("Your selection: ");

        }
        private int GetUserInput()
        {
            string? input = Console.ReadLine();
            int numericInput;

            while (!int.TryParse(input, out numericInput) || numericInput < 1 || numericInput > 5)
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }
           
            return numericInput;
        }
        private void ProcessUserInput() 
        {
            
            int input = GetUserInput();
            string table = "WaterIntake";
            //DatabaseRecord record;

            int recordID;
            switch (input)
            {
                case 1: //print records
                    _databaseManager.PrintRecordsFromATable(table);
                    break;
                case 2: //new record
                    DatabaseRecord newRecord = _IOmanager.GetNewRecord(table);
                    _databaseManager.InsertRecord(newRecord, table);
                    break;
                case 3: //update record
                    _databaseManager.PrintRecordsFromATable(table);
                    recordID = GetValidRecordID(table, "update");
                    DatabaseRecord oldRecord = _databaseManager.GetRecord(recordID);
                    DatabaseRecord updatedRecord = _IOmanager.UpdateRecord(oldRecord);
                    _databaseManager.UpdateRecord(updatedRecord, table);
                    break;
                case 4: //delete record
                    _databaseManager.PrintRecordsFromATable(table);
                    recordID = GetValidRecordID(table, "delete");
                    _databaseManager.DeleteRecord(recordID, "WaterIntake");
                    break;
                case 5: //exit
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey(true);
        }
       public int GetValidRecordID(string table, string operation)
        {
            int recordID = _IOmanager.GetRecordIdFromUser("operation");
            while (!_databaseManager.IsIdPresentInDatabase(recordID, table))
            {
                Console.WriteLine("Entered ID is not present in the database");
                recordID = _IOmanager.GetRecordIdFromUser("update");
            }
            return recordID;

        }
    }
    
}
