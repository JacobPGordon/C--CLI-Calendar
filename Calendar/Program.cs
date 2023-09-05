string DFILE = "./dates_file";
Entry[] dates;

void add_date(){

}

void remove_date(){

}


//Fetch dates from the file upon load
void load_dates(){
    //Using streamreader we can read the file line by line easily
    using (StreamReader sr = new StreamReader(DFILE)){
        while (sr.Peek() >= 0){
            string currentDate = sr.ReadLine();
            string[] split = currentDate.Split(";");
        }
    }
}


//Creating data file if it doesn't yet exist
if(!File.Exists(DFILE)){
    
    using (FileStream fs = File.Create(DFILE));

}

load_dates();


