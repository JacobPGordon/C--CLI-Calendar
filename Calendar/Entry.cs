

public class Entry{
    //A datetime representation of the date entered for this item
    public DateTime date {get; set;}
    //The actual text of the item
    public string desc {get; set;}
    //The number of minutes that the program should wait for to repeat its warning
    public int repeat {get; set;}
    //used for quick reference; cosntructed by hashing the description plus a string representation of the date 
    public byte[] id {get; set;}

    


}