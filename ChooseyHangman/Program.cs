using System;
using System.Collections.Generic;
using System.Linq;

namespace ChooseyHangman
{
    class Program
    {
        private static void Main()
        {
            var gameState = new GameState(PromptForWord());

            while (!gameState.IsFinished)
            {
                RenderStage(gameState);
                gameState.Guesses.Add(char.ToUpper(PromptForLetter()));
            }

            RenderStage(gameState);
            Console.WriteLine($"You {(gameState.IsWon ? "won" : "lost")}! The word was {gameState.Word}.");

            Console.Write("Play again? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                Main();
            }
        }

        private static void RenderStage(GameState gameState)
        {
            Console.Clear();
            Console.WriteLine(gameState.Stages[gameState.WrongGuesses.Count()]);
            Console.WriteLine($"Word ({gameState.Word.Length} letters): {gameState.WordDisplay}");
            Console.WriteLine($"Wrong guesses: {string.Join(" ", gameState.WrongGuesses)}");
        }

        private static string PromptForWord()
        {
            Console.Write("Enter your word: ");
            var foreColor = Console.ForegroundColor;
            var backColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Black;
            string word = Console.ReadLine();
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            return word;
        }

        private static char PromptForLetter()
        {
            string input;
            do
            {
                Console.Write("Enter single letter guess: ");
                input = Console.ReadLine();
            } while (input.Length != 1);
            return input.First();
        }
    }

    class GameState
    {
        public readonly string[] Stages =
        {
            @"



            _____________",
            @"

                |
                |
            ____|________",
            @"  
                ____
                |
                |
            ____|________",
            @"  
                ____
                |   O
                |
            ____|________",
            @"  
                ____
                |   O
                |   |
            ____|________",
            @"  
                ____
                |   O
                |  /|
            ____|________",
            @"  
                ____
                |   O
                |  /|\
            ____|________",
            @"  
                ____
                |   O
                |  /|\
            ____|__/_____",
            @"  
                ____
                |   O
                |  /|\
            ____|__/_\___"
        };

        public string Word { get; }
        public HashSet<char> Guesses { get; } = new HashSet<char>();
        public IEnumerable<char> WrongGuesses => Guesses.Where(guess => !Word.Contains(guess));
        public string WordDisplay => string.Join("", Word.Select(c => Guesses.Contains(c) ? c : '_'));
        public bool IsWon => WordDisplay == Word;
        public bool IsLost => WrongGuesses.Count() >= Stages.Length - 1;
        public bool IsFinished => IsWon || IsLost;

        public GameState(string word)
        {
            Word = word.ToUpper();
        }
    }
}
