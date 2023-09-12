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
    using (var writer = new StreamWriter("./TEMP", false))
    using (var csv_writer = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        csv_writer.WriteRecords(dates);
    }
    File.Move("./TEMP", DFILE,true);    
    Console.WriteLine("TEST");
}

void SaveOnClose(){
    using (var writer = new StreamWriter("./TEMP", false))
    using (var csv_writer = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        csv_writer.WriteRecords(dates);
    }
    File.Move("./TEMP", DFILE,true);    
    Console.WriteLine("TEST");
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
            dates.Add(date);
        }



    }
    return;

}



//Initializing variables for runtime
Console.Clear();
bool ACTIVE = true;
load_dates();

//initializing autosave timer
System.Timers.Timer autosave = new System.Timers.Timer(30000);
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


        default:
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
}

