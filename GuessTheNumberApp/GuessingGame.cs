using System;
using System.Collections.Generic;

namespace GuessTheNumberApp
{
    public class GuessingGame
    {
        private int numNeedToGuess;
        private int turns;
        public int Score { get; private set; } = 100;
        public GuessingGame()
        {
            turns = 10;
            numNeedToGuess = GetRandomNumber();
        }

        public bool CheckValidAnswer(int answer) => answer >= 1 && answer <= 100;

        public bool CheckTheAnswer(int value, out bool isTooHigh)
        {
            if (value == numNeedToGuess)
            {
                isTooHigh = false;
                int remainingTurns = GetRemainingTurns();
                Score += remainingTurns * 10;
                return true;
            }
            else
            {
                isTooHigh = value > numNeedToGuess;
                Score -= 10;
                turns--;
                return false;
            }
        }

        public int GetRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 100);
            return randomNumber;
        }
        public void AddScore(int score)
        {
            Score += score;
        }
        
        public int GetRemainingTurns() => turns;
        public int NumNeedToGuess => numNeedToGuess;

    }
}