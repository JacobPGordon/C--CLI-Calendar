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



//constant for the dates file
string DFILE = "./dates_file.csv";
List<Entry> dates = new List<Entry>();
STATES CURRENT_STATE = STATES.START;


//handles adding dates to the internal listing
void add_date(string in_date, string in_desc, int in_repeat){

    //preparing the bytes for hashing
    byte[] hashable = ASCIIEncoding.ASCII.GetBytes(in_date+in_desc);
    
    Entry new_entry = new Entry
    {
        date = DateTime.Parse(in_date),
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

//every sixty seconds or upon closing the application, the date file is automatically updated; due to the limitations of CSV-Helper it
//has to be recreated
void update_date_file(){
    STATES current_state = CURRENT_STATE;
    using (var writer = new StreamWriter("./TEMP", false))
    using (var csv_writer = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        csv_writer.WriteRecords(dates);
    }

    File.Move("./TEMP", DFILE,true);    
    Console.WriteLine("***Autosaved***");
    Thread.Sleep(3000);
    if(current_state == CURRENT_STATE){
        Console.SetCursorPosition(0, Console.CursorTop-1);
        Console.Write(new String(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop-1);
    }
    
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
              foreach (Entry date in dates){
        
                Console.WriteLine("{0}: {1}", date.date.ToString("MM/dd/YYYY"), date.desc);

                }  

            }

            Console.WriteLine("\n(A)dd new entry, (D)elete entry, (E)xit application");
            
            CURRENT_STATE = Controller();
            break;

        case STATES.ADD:

            Console.Clear();
            Console.WriteLine("Please enter a date formatted in mm/dd/yyyy hh:mm");
            
            //Initializers for Event object
            DateTime desired_date;
            int desired_repeat;
            string[] desired_desc;

            //Getting the desired date from the user
            
            bool input_done = false;
            while(input_done == false){
                var input = Console.ReadLine();

                //thank God for small miracles (no regex required)
                if(DateTime.TryParseExact(input, "MM/dd/yyyy hh:mm", null, DateTimeStyles.None, out DateTime dt)){

                    desired_date = dt;
                    input_done = true;
                    break;

                }else{

                    Console.WriteLine("Improper input! Press Enter to try again");
                    //spaghetti code for resetting the text and cursor
                    while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                    Console.SetCursorPosition(0, Console.CursorTop-1);
                    Console.Write(new String(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop-1);
                    Console.Write(new String(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                }
                
           }
            
           input_done = false;

            


            break;

        case STATES.EXIT:
            Console.Clear();
            ACTIVE = false;
            Console.WriteLine("Have a nice day!");
            Thread.Sleep(1000);
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
}

