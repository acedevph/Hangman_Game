using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangmanGame
{
    class Program
    {
        // ─── Word Bank ──────────────────────────────────────────────────────────────
        static readonly Dictionary<string, List<string>> WordBank = new()
        {
            ["Animals"] = new()
            {
                "elephant", "crocodile", "butterfly", "giraffe", "penguin",
                "chimpanzee", "dolphin", "kangaroo", "armadillo", "chameleon"
            },
            ["Countries"] = new()
            {
                "philippines", "australia", "brazil", "switzerland", "indonesia",
                "argentina", "portugal", "bangladesh", "madagascar", "mozambique"
            },
            ["Technology"] = new()
            {
                "algorithm", "compiler", "database", "encryption", "framework",
                "interface", "keyboard", "processor", "recursion", "bandwidth"
            },
            ["Sports"] = new()
            {
                "basketball", "volleyball", "badminton", "swimming", "gymnastics",
                "archery", "wrestling", "triathlon", "bobsled", "lacrosse"
            },
            ["Food"] = new()
            {
                "spaghetti", "croissant", "avocado", "dumplings", "blueberry",
                "quesadilla", "cheesecake", "asparagus", "bruschetta", "marmalade"
            }
        };

        // ─── Hangman ASCII Art (0 = fresh, 6 = dead) ────────────────────────────────
        static readonly string[] HangmanStages =
        {
            // 0 – Empty gallows
            @"
  +---+
  |   |
      |
      |
      |
      |
=========",
            // 1 – Head
            @"
  +---+
  |   |
  O   |
      |
      |
      |
=========",
            // 2 – Head + body
            @"
  +---+
  |   |
  O   |
  |   |
      |
      |
=========",
            // 3 – Head + body + left arm
            @"
  +---+
  |   |
  O   |
 /|   |
      |
      |
=========",
            // 4 – Head + body + both arms
            @"
  +---+
  |   |
  O   |
 /|\  |
      |
      |
=========",
            // 5 – Head + body + both arms + left leg
            @"
  +---+
  |   |
  O   |
 /|\  |
 /    |
      |
=========",
            // 6 – Full hangman (game over)
            @"
  +---+
  |   |
  O   |
 /|\  |
 / \  |
      |
========="
        };

        static readonly int MaxWrongGuesses = HangmanStages.Length - 1; // 6

        // ─── Colour helpers ──────────────────────────────────────────────────────────
        static void WriteColour(string text, ConsoleColor colour, bool newLine = true)
        {
            Console.ForegroundColor = colour;
            if (newLine) Console.WriteLine(text);
            else         Console.Write(text);
            Console.ResetColor();
        }

        static void WriteTitle(string text)
        {
            Console.WriteLine();
            WriteColour(text, ConsoleColor.Cyan);
            WriteColour(new string('─', text.Length), ConsoleColor.DarkCyan);
        }

        // ─── Entry point ─────────────────────────────────────────────────────────────
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            ShowWelcome();

            bool playAgain = true;
            int wins = 0, losses = 0;

            while (playAgain)
            {
                var (category, word) = PickWord();
                bool won = PlayRound(category, word);

                if (won) wins++; else losses++;

                Console.WriteLine();
                WriteColour($"  📊  Session score  ─  Wins: {wins}   Losses: {losses}",
                            ConsoleColor.DarkYellow);
                Console.WriteLine();

                WriteColour("  Play again? (Y / N): ", ConsoleColor.White, newLine: false);
                string? again = Console.ReadLine()?.Trim().ToUpper();
                playAgain = again == "Y";
            }

            Console.WriteLine();
            WriteColour("  Thanks for playing Hangman! Goodbye 👋", ConsoleColor.Green);
            Console.WriteLine();
        }

        // ─── Welcome screen ──────────────────────────────────────────────────────────
        static void ShowWelcome()
        {
            Console.Clear();
            WriteColour(@"
  ██╗  ██╗ █████╗ ███╗   ██╗ ██████╗ ███╗   ███╗ █████╗ ███╗   ██╗
  ██║  ██║██╔══██╗████╗  ██║██╔════╝ ████╗ ████║██╔══██╗████╗  ██║
  ███████║███████║██╔██╗ ██║██║  ███╗██╔████╔██║███████║██╔██╗ ██║
  ██╔══██║██╔══██║██║╚██╗██║██║   ██║██║╚██╔╝██║██╔══██║██║╚██╗██║
  ██║  ██║██║  ██║██║ ╚████║╚██████╔╝██║ ╚═╝ ██║██║  ██║██║ ╚████║
  ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝", ConsoleColor.Yellow);

            Console.WriteLine();
            WriteColour("  Guess the hidden word before the man is hanged!", ConsoleColor.Gray);
            WriteColour($"  You have {MaxWrongGuesses} wrong guesses per round.", ConsoleColor.Gray);
            Console.WriteLine();
            WriteColour("  Categories available:", ConsoleColor.DarkGray);
            foreach (var cat in WordBank.Keys)
                WriteColour($"    • {cat}", ConsoleColor.DarkGray);
            Console.WriteLine();
        }

        // ─── Word selection ──────────────────────────────────────────────────────────
        static (string category, string word) PickWord()
        {
            var categories = WordBank.Keys.ToList();

            WriteTitle("  Choose a Category");
            for (int i = 0; i < categories.Count; i++)
                WriteColour($"  [{i + 1}]  {categories[i]}", ConsoleColor.White);
            WriteColour("  [0]  Random", ConsoleColor.DarkGray);
            Console.WriteLine();

            int choice = -1;
            while (choice < 0 || choice > categories.Count)
            {
                WriteColour("  Your choice: ", ConsoleColor.White, newLine: false);
                string? input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out choice) || choice < 0 || choice > categories.Count)
                {
                    WriteColour("  ⚠ Invalid choice. Please try again.", ConsoleColor.Red);
                    choice = -1;
                }
            }

            var rng = new Random();
            string category = choice == 0
                ? categories[rng.Next(categories.Count)]
                : categories[choice - 1];

            var wordList = WordBank[category];
            string word = wordList[rng.Next(wordList.Count)];

            return (category, word);
        }

        // ─── Core game round ─────────────────────────────────────────────────────────
        static bool PlayRound(string category, string word)
        {
            var guessed   = new HashSet<char>();   // all letters tried
            var correct   = new HashSet<char>();   // correct letters
            int wrongCount = 0;

            while (wrongCount < MaxWrongGuesses)
            {
                Console.Clear();
                RenderBoard(category, word, guessed, correct, wrongCount);

                // Win check
                if (word.All(c => correct.Contains(c)))
                {
                    Console.WriteLine();
                    WriteColour("  ✅  Excellent! You guessed the word!", ConsoleColor.Green);
                    WriteColour($"  The word was: {word.ToUpper()}", ConsoleColor.Green);
                    return true;
                }

                // Prompt
                Console.WriteLine();
                WriteColour("  Enter a letter: ", ConsoleColor.White, newLine: false);
                string? raw = Console.ReadLine()?.Trim().ToLower();

                // Validate
                if (string.IsNullOrEmpty(raw) || raw.Length != 1 || !char.IsLetter(raw[0]))
                {
                    WriteColour("  ⚠ Please enter a single letter.", ConsoleColor.Red);
                    System.Threading.Thread.Sleep(900);
                    continue;
                }

                char letter = raw[0];

                if (guessed.Contains(letter))
                {
                    WriteColour($"  ⚠ You already guessed '{letter}'. Try another.", ConsoleColor.Yellow);
                    System.Threading.Thread.Sleep(900);
                    continue;
                }

                guessed.Add(letter);

                if (word.Contains(letter))
                {
                    correct.Add(letter);
                    WriteColour($"  ✔ '{letter}' is in the word!", ConsoleColor.Green);
                }
                else
                {
                    wrongCount++;
                    WriteColour($"  ✘ '{letter}' is NOT in the word. " +
                                $"({MaxWrongGuesses - wrongCount} guess(es) left)", ConsoleColor.Red);
                }

                System.Threading.Thread.Sleep(700);
            }

            // Lose
            Console.Clear();
            RenderBoard(category, word, guessed, correct, wrongCount);
            Console.WriteLine();
            WriteColour("  ☠  Game over! The man has been hanged.", ConsoleColor.Red);
            WriteColour($"  The word was: {word.ToUpper()}", ConsoleColor.Yellow);
            return false;
        }

        // ─── Render the full game board ──────────────────────────────────────────────
        static void RenderBoard(string category, string word,
                                HashSet<char> guessed, HashSet<char> correct,
                                int wrongCount)
        {
            // Hangman figure
            WriteColour(HangmanStages[wrongCount],
                        wrongCount < 3 ? ConsoleColor.DarkGray :
                        wrongCount < 5 ? ConsoleColor.Yellow   : ConsoleColor.Red);

            Console.WriteLine();

            // Category
            WriteColour($"  Category : {category}", ConsoleColor.Cyan);

            // Wrong count
            WriteColour($"  Wrong    : {wrongCount} / {MaxWrongGuesses}", ConsoleColor.DarkYellow);

            // Word display  (e.g.  _ _ A _ _ M A N )
            Console.WriteLine();
            Console.Write("  ");
            foreach (char c in word)
            {
                if (correct.Contains(c))
                    WriteColour($"{char.ToUpper(c)} ", ConsoleColor.Green, newLine: false);
                else
                    WriteColour("_ ", ConsoleColor.White, newLine: false);
            }
            Console.WriteLine();
            Console.WriteLine();

            // Wrong letters
            var wrongLetters = guessed.Except(correct).OrderBy(c => c).ToList();
            if (wrongLetters.Count > 0)
            {
                Console.Write("  Wrong letters: ");
                WriteColour(string.Join("  ", wrongLetters).ToUpper(), ConsoleColor.Red, newLine: false);
                Console.WriteLine();
            }

            // Correct letters
            if (correct.Count > 0)
            {
                Console.Write("  Found letters: ");
                WriteColour(string.Join("  ", correct.OrderBy(c => c)).ToUpper(),
                            ConsoleColor.Green, newLine: false);
                Console.WriteLine();
            }
        }
    }
}