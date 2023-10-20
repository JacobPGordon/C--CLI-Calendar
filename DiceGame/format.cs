//methods to display output to the console
public class formatter{
    //displays the games rules and options
    public static void intro(){
        Console.WriteLine("Welcome to dice roller!");
        Console.WriteLine("You get to choose a number of die to roll and their size, and have to roll above a certain number to win.");
        Console.WriteLine("Press Enter to Continue");
        Console.ReadLine();

    }

    //used to get a difficulty setting from the player and then return it as a plain string
    //cursor help from https://stackoverflow.com/questions/5027301/c-sharp-clear-console-last-item-and-replace-new-console-animation and
    //https://stackoverflow.com/questions/8946808/can-console-clear-be-used-to-only-clear-a-line-instead-of-whole-console
    public static string difficulty(){
        Console.WriteLine("Choose a difficulty by typing in 'EASY', 'MEDIUM', or 'HARD'");
        int done = 0;
        //this loop is used to keep the difficulty selection process running until the user gives a valid input
        while (done==0){
            var difficulty = Console.ReadLine();
            switch(difficulty){
            
                case null:
                    Console.WriteLine("Invalid input! Press Enter to retry.");
                    Console.ReadLine();
                    
                    Console.SetCursorPosition(0, Console.CursorTop-1);
                    Console.Write("");
                    continue;

                case "EASY":
                    return "EASY";
                    
                case "MEDIUM":
                    return "MEDIUM";
                    
                case "HARD":
                    return "HARD";

                 case (_):
                    Console.WriteLine("Invalid input! Press Enter to retry.");
                    Console.ReadLine();
                    
                    Console.SetCursorPosition(0, Console.CursorTop-2);
                    Console.Write(new String(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop-1);
                    Console.Write(new String(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    continue;
                
             
            }
        }

        return "ERROR";
    
    }

    //this is used to select the number of dice and how large you want them to be
    public static (int dice, int ceiling) selectDice(){
        Console.WriteLine("Select the number of dice you want to roll (maximum ten)!");

        //variables for the output and intermediate results 
        int done = 0;
        int dice = 0;
        int ceiling = 0;
        

        while(done == 0){

            var output = Console.ReadLine();
            int numberOutput = 0;

            // if it's an integer, we go to further handling; otherwise, pass an error message and wait for a new input
            if(int.TryParse(output, out numberOutput)){
                switch(numberOutput){
                    
                    //If the input is less than 1
                    case int a when a<1:
                        Console.WriteLine("You can't have negative or no dice! Press Enter to retry.");
                        Console.ReadLine();
                    
                        Console.SetCursorPosition(0, Console.CursorTop-1);
                        Console.Write("");
                        continue;
                    
                    case int a when a>10:
                        Console.WriteLine("You can't have more than ten dice! Press Enter to retry.");
                        Console.ReadLine();
                    
                        Console.SetCursorPosition(0, Console.CursorTop-2);
                        Console.Write(new String(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop-1);
                        Console.Write(new String(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        continue;

                    case (_):
                        dice = numberOutput;
                        done = 1;
                        continue;



                }


            }else{

                Console.WriteLine("That wasn't a number! Press Enter to retry.");
                Console.ReadLine();

                Console.SetCursorPosition(0, Console.CursorTop-2);
                Console.Write(new String(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop-1);
                Console.Write(new String(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                continue;
            }

        }

        //Asking user for size of dice
        done = 0;
        Console.Clear();
        Console.WriteLine("Select how large you want the dice to be (4 through 20 faces)");
        while(done==0){
            var output = Console.ReadLine();
            int numberOutput = 0;
            
            //checking for integer
            if(int.TryParse(output, out numberOutput)){
                
                switch(numberOutput){

                    case int a when a<4:
                        Console.WriteLine("Not enough faces! Press Enter to retry.");
                        Console.ReadLine();
                    
                        Console.SetCursorPosition(0, Console.CursorTop-2);
                        Console.Write(new String(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop-1);
                        Console.Write(new String(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        continue;
                    
                    case int a when a>20:
                        Console.WriteLine("You can't have more than twenty faces! Press Enter to retry.");
                        Console.ReadLine();

                        Console.SetCursorPosition(0, Console.CursorTop-2);
                        Console.Write(new String(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop-1);
                        Console.Write(new String(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        continue;

                    case (_):
                        ceiling=numberOutput;
                        done = 1;
                        continue;

                }


            }else{

                Console.WriteLine("That wasn't a number! Press Enter to retry.");
                Console.ReadLine();

                Console.SetCursorPosition(0, Console.CursorTop-2);
                Console.Write(new String(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop-1);
                Console.Write(new String(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                continue;

            }

        }

    return(dice,ceiling);
    
    }

}

