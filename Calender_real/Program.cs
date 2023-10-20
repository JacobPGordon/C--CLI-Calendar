//CSV Assistance from CsvHelper library https://joshclose.github.io/CsvHelper/

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Threading;
using System.Timers;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;



//constant for the dates file
string DFILE = "./dates_file.csv";
List<Entry> dates = new List<Entry>();
STATES CURRENT_STATE = STATES.START;
int bottom_cursor = 0;


//handles adding dates to the internal listing
void add_date(DateTime in_date, string in_desc, int in_repeat){

    //preparing the bytes for hashing
    byte[] hashable = ASCIIEncoding.ASCII.GetBytes(in_date+in_desc);
    
    Entry new_entry = new Entry
    {
        date = in_date,
        desc = in_desc,
        repeat = in_repeat,
        id = MD5.HashData(hashable),
    };
    
    dates.Add(new_entry);
    
}

void remove_date(byte[] id){

    int index = dates.FindIndex(i => i.id == id);
    dates.RemoveAt(index);

}

//ClearBuffer method taken from https://stackoverflow.com/questions/64621972/clearing-the-keyboard-buffer-or-blocking-input-c-console
static void ClearBuffer()
{
    while (Console.KeyAvailable) {
        Console.ReadKey(true);
    }            
}

//every sixty seconds or upon closing the application, the date file is automatically updated; due to the limitations of CSV-Helper it
//has to be recreated
void update_date_file(){
    STATES current_state = CURRENT_STATE;
    using (var writer = new StreamWriter("./TEMP", false))
    using (var csv_writer = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        csv_writer.WriteRecords(dates);
    }

    File.Move("./TEMP", DFILE,true);    
    if(current_state != STATES.EXIT) {Console.WriteLine("***Autosaved***");}
    else{Console.WriteLine("Saving: Please Wait");}
    Thread.Sleep(3000);
    if(current_state == CURRENT_STATE && CURRENT_STATE != STATES.EXIT){
        Console.SetCursorPosition(0, Console.CursorTop-1);
        Console.Write(new String(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop-1);
    }
    
}

//Clears the console up to the line given by the input number (3 preserves the first 3 lines, etc.)\
//For removal of lines going bottom to top, you can pass in CursorTop minus the number of lines you want removed; this is useful for 
//removing things from the bottom when the console size is determined at runtime (such as removing an UI element after printing a list of unknown size)
void clear_input(int preserved_lines){

    //counter for loops
    int i = 0;
    //used to decrement cursor
    int offset = Console.CursorTop-preserved_lines;
    while (i < offset){
        
        Console.SetCursorPosition(0, Console.CursorTop-1);
        Console.Write(new String(' ', Console.WindowWidth));
        i++;

    }
    Console.SetCursorPosition(0, Console.CursorTop);

    return;
}



//Fetch dates from the file upon load
void load_dates(){

    //quick initialization for first run (or if the user deleted the file for whatever reason)
    if (!File.Exists(DFILE)){
        using (StreamWriter writer = File.AppendText(DFILE)){
            writer.WriteLine("id,date,desc,repeat");
        }
        return;
    }

    //Otherwise, we load the file and use CSVHelper to write 
    using (var reader = new StreamReader(DFILE))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)){
        var date = new Entry();
        var read_dates = csv.EnumerateRecords(date);
        foreach (var row in read_dates){
           dates.Add(row);
        }



    }
    return;

}


//Checks for user input and compares it to state to determine action
STATES Controller(){
    ConsoleKey current_key;
    while(true){
        current_key = Console.ReadKey(true).Key;

        //Calendar page
        if(CURRENT_STATE == STATES.CALENDAR){
            //Exit handler
            if (current_key == ConsoleKey.E){
                return STATES.EXIT;
            }

            //Removing date handler
            if (current_key == ConsoleKey.D){
                return STATES.REMOVE;
            }
            
            //Adding date handler
            if (current_key == ConsoleKey.A){
                return STATES.ADD;
            }
        }
        
        


    }
}

//Initializing variables for runtime
Console.Clear();
bool ACTIVE = true;
load_dates();

//initializing autosave timer
System.Timers.Timer autosave = new System.Timers.Timer(60000);
autosave.Elapsed += (sender,e) => update_date_file();

autosave.Start();

while(ACTIVE){

    switch(CURRENT_STATE){

        case STATES.START:
            Console.WriteLine("_______________");
            int i=0;
            while(i < 4){
                Console.WriteLine("|_|_|_|_|_|_|_|");
                i++;
            }
    
            Console.WriteLine("\n\nWelcome back!");
            Console.WriteLine("Please press enter to continue");
            while(Console.ReadKey(true).Key != ConsoleKey.Enter);
            CURRENT_STATE = STATES.CALENDAR;
            break;

        case STATES.CALENDAR:
            Console.Clear();
            if (dates.Count == 0){
                Console.WriteLine("No entries detected!");
            }

            else{
              //Displaying dates  
              int number = 0;
              foreach (Entry date in dates){
                number++;
                Console.WriteLine("{0}. {1} : {2}",number, date.date.ToString("MM/dd/yyyy HH:mm"), date.desc);

                }  


            }

            Console.WriteLine("\n(A)dd new entry, (D)elete entry, (E)xit application");

            bottom_cursor = Console.CursorTop;
            CURRENT_STATE = Controller();
            break;

        case STATES.ADD:

            Console.Clear();
            Console.WriteLine("Please enter a date formatted in mm/dd/yyyy hh:mm");
            
            //Initializers for Event object
            DateTime desired_date = new DateTime();
            int desired_repeat = 0;
            string desired_desc = "";

            //Getting the desired date from the user
            
            bool input_done = false;
            while(input_done == false){
                var input = Console.ReadLine();
                if(input is not null){ input = input.Trim();}

                //thank God for small miracles (no regex required)
                if(DateTime.TryParseExact(input, "MM/dd/yyyy hh:mm", null, DateTimeStyles.None, out DateTime dt)){

                    DateTime current_date = DateTime.Now;
                    if(DateTime.Compare(dt,current_date) < 0){

                        Console.WriteLine("This date is in the past! Please try again.");
                        //spaghetti code for resetting the text and cursor
                        while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                        clear_input(1);

                    }else{

                        desired_date = dt;
                        input_done = true;
                        break;
                    }


                
                }else{

                    Console.WriteLine("Improper input! Press Enter to try again");
                    //spaghetti code for resetting the text and cursor
                    while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                    clear_input(1);
                }
                
           }
            
           input_done = false;

           //Getting description

           Console.Clear();
           Console.WriteLine("Please enter a description for this calendar entry; leave the input blank for no description.");
           
           while(input_done == false){

            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)){

                input_done = true;
                desired_desc = "[No description]";

            }else{
                //sanity check to prevent input bomb
                if (input.Length > 300){

                    Console.WriteLine("Input too long! Press enter to try again.");
                    while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                    clear_input(1);

                }else{
                    desired_desc = input.Trim();
                    input_done = true;   
                }

            }

           }

            input_done = false;

           //Getting repeat (if any)

           Console.Clear();
           Console.WriteLine("Please enter the number of minutes you would like to have inbetween notifications for this item; leave blank for none.");

           while(input_done == false){

            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)){
                input_done = true;
                desired_repeat = 0;
                break;
            }

            input = input.Trim();
            if(int.TryParse(input, out var input_int) && input.Contains(".") == false){
                input_done = true;
                desired_repeat = input_int;
            }else{
                
                Console.WriteLine("Non-numerical input! Press enter to try again.");
                while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                clear_input(1);

            }


           }

           add_date(desired_date, desired_desc, desired_repeat);
           Console.WriteLine("Date added! Press enter to return to your calender.");
           while(Console.ReadKey(true).Key != ConsoleKey.Enter);
           CURRENT_STATE = STATES.CALENDAR;

            break;

        case STATES.EXIT:
            Console.Clear();
            ACTIVE = false;
            update_date_file();
            Console.WriteLine("Have a nice day!");
            Thread.Sleep(1000);
            break;

        //Removing calendar entries
        case STATES.REMOVE:

            if(dates.Count == 0){

                Console.WriteLine("No entries to delete!");
                Thread.Sleep(1000);

                CURRENT_STATE = STATES.CALENDAR;
                break;

            }

            clear_input(bottom_cursor-1);
            bool deletions_done = false;
            Console.WriteLine("Please press the number for the entry you want to delete, or hit d again to cancel");

            //control loop for deletions
            while(deletions_done == false){
                
                //Assistance from https://stackoverflow.com/questions/28955029/how-do-i-convert-a-console-readkey-to-an-int-c-sharp

                ConsoleKeyInfo user_input; 

                user_input = Console.ReadKey(true);
                //Cancel key
                if(user_input.Key == ConsoleKey.D){deletions_done = true;}
                //We want to exclude zero because entries are listed 1-9
                if(char.IsDigit(user_input.KeyChar) && user_input.KeyChar != '0'){

                    //Checking if that entry exists in the current scope
                    int input_number = int.Parse(user_input.KeyChar.ToString());

                    if(input_number > dates.Count){
                        
                        Console.WriteLine("That entry doesn't exist!");
                        Thread.Sleep(1000);
                        ClearBuffer();
                        clear_input(Console.CursorTop-1);

                    }else{
                        
                    }

                }
                



            }

            CURRENT_STATE = STATES.CALENDAR;
            break;

        default:

            // if something goes terribly wrong or the code does something impossible we try to save and kill the app
            Console.Clear();
            Console.WriteLine("Unexpected application state entered. Saving and shutting down.");
            autosave.Stop();
            update_date_file();
            ACTIVE = false;
            break;

    }

  

}



enum STATES{
    START,
    CALENDAR,
    EXIT,
    ADD,
    REMOVE,
}

