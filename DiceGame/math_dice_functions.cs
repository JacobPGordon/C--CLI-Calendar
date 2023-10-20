//these methods are used to roll the dice and calculate various things, such as the score


public class roller{
//returns an array consisting of the outcome of rolling a die with a range of zero and {ceiling+1} {count} times 
public static int[] RollDice(int ceiling, int count){
    //Initializing the Random object for rolling 
    Random die = new Random();
    //initializing output array
    int[] output = new int[count];
    int i = 0;
    //rolling the die and adding the results to the array
    while(i < count){
        
        output[i] = (die.Next(1,ceiling+1));
        i++;

    }

    return output;
}

//used to calculate and return the game's score
public static int calculateScore(int[] rolls){

    //The score is calculated by going through the array of rolls and adding them to a holder value; we also initialize a dictioanry to track if numbers
    //have occured several times
    int score = 0;
    Dictionary<int, int> multiples = new Dictionary<int,int>();
    
    //iterating through array to get basic score and to populate multiples dictionary
    foreach (int i in rolls){
        score+=i;
        if (multiples.ContainsKey(i)){
            multiples[i] = multiples[i] + 1;
        }else{
            multiples.Add(i,1);
        }
    }

    //after that we can iterate through the dictionary to get the score with the double and triple bonus
    foreach (var entry in multiples){
        if (entry.Value ==2){
            score+=2;
        }
        if (entry.Value >2){
            score+=6;
        }
    } 
    return score;


}

//this is used to dynamically calculate the score needed to win based on dice amount, size, and chosen difficulty
//difficulty settings: easy is 75 percent chance to win, medium is 50 percent, hard is 25 percent
public static int winningScore(int ceiling, int count, string difficulty){
    //calculating max score possible with given dice
    int maxScore = ceiling * count;
    //we multiply to get  
    switch(difficulty){
        case "EASY":
            return (int)((double)maxScore * 0.25);

        case "MEDIUM":
            return (int)((double)maxScore * 0.5);

        case "HARD":
            return (int)((double)maxScore * 0.75); 

    }
    return 0;
}

}
