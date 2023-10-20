
//a simple, interactive dice game 

//variables used to control game execution 
string GAME_STATE = "DIFFICULTY";
string DIFFICULTY = "";
int DICE = 0;
int CEILING = 0;
int WINS = 0;
int TOTAL_GAMES = 0;


void clearGame(){
    DIFFICULTY = "";
    DICE = 0;
    CEILING = 0;
    GAME_STATE = "DIFFICULTY";
    return;
}

Console.Clear();
formatter.intro();
//code varies based upon game state; the while loop is used to run the game continiously after each user input
while(GAME_STATE != "CRITICAL_FAILURE"){
    switch(GAME_STATE){
        
        //Getting user input for difficulty setting
        case "DIFFICULTY":

            Console.Clear();
            Console.WriteLine($"You've won {WINS} out of {TOTAL_GAMES} games");
            //Assigning difficulty and error handler
            DIFFICULTY=formatter.difficulty();

            if(DIFFICULTY=="ERROR"){
                Console.Clear();
                Console.WriteLine("Something has gone horribly wrong! Press enter to exit the application.");
                Console.ReadLine();
                GAME_STATE = "CRITICAL_FAILURE";
                break;
            }

            //advancing game state to move on to selecting the number of dice
            GAME_STATE = "DICE_SELECTION";
            continue;
        
        //Getting user input for the dice to use
        case "DICE_SELECTION":

            Console.Clear();
            var output_tuple = formatter.selectDice();

            //placing output into game's state
            DICE = output_tuple.dice;
            CEILING = output_tuple.ceiling;
            GAME_STATE = "ROLLING";
            continue;

        //Handler for the game and rolling dice            
        case "ROLLING":

            Console.Clear();

            //calculating neccessary values for determining if the player won
            var rolled_dice = roller.RollDice(CEILING, DICE);
            int score = roller.calculateScore(rolled_dice);
            int winningScore = roller.winningScore(CEILING, DICE, DIFFICULTY);
            string formatted_dice = "";
            var i = 0;
            bool victory = false;
            TOTAL_GAMES++;

            //formatting output and determining if player won or lost
            while(i < DICE){
                if( i == DICE-1){
                    formatted_dice = formatted_dice + $"and {rolled_dice[i] }";
                }else{
                    formatted_dice = formatted_dice + $"{rolled_dice[i]}, ";
                }
                i++;
            }

            if(score >= winningScore){
                victory=true;
                WINS++;
            }

            Console.WriteLine($"You rolled {formatted_dice} for a total of {score}, and had to roll at or above {winningScore}");
            if(victory==true){
                Console.WriteLine("Congratulations, you won!");
            }else{
                Console.WriteLine("You lost!");
            }
            
            Console.WriteLine("Press enter to play again.");
            clearGame();
            Console.ReadLine();
            continue;

        default:
            Console.Clear();
            Console.WriteLine("Something has gone horribly wrong! Press enter to exit the application.");
            Console.ReadLine();
            GAME_STATE = "CRITICAL_FAILURE";
            break;
    }


}



