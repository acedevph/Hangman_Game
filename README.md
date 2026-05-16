#  Hangman Game
> A text-based C# console game where players guess hidden words across multiple categories before the man is hanged.

---

##  Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Categories & Word Bank](#categories--word-bank)
- [How to Run](#how-to-run)
- [How to Play](#how-to-play)
- [Game Screenshots](#game-screenshots)
- [Project Structure](#project-structure)
- [Concepts Used](#concepts-used)
- [Team](#team)

---

## Overview

The **Hangman Game** is a fully text-based C# console application developed as a programming exercise in string manipulation, conditional logic, and data structures. Players choose a word category, then guess the hidden word one letter at a time — with a 7-stage ASCII gallows that builds progressively with every wrong guess.

---

## Features

###  Functional
| Feature | Description |
|---|---|
| Category Selection | Choose from 5 themed categories or pick Random |
| Random Word Drawing | One word randomly drawn per round from a pool of 10 |
| ASCII Hangman Art | 7-stage gallows that updates with each wrong guess |
| Input Validation | Rejects non-letters, multi-char inputs, and repeat guesses |
| Colour-Coded Feedback | Green = correct, Red = wrong, Yellow = warnings |
| Session Score Tracker | Tracks cumulative wins and losses across rounds |
| Play Again Loop | Seamlessly replay without restarting the program |

###  Non-Functional
- **Performance** — All logic runs in O(n) time; zero perceptible lag
- **Portability** — Targets .NET 6+ and runs on Windows, macOS, and Linux
- **Maintainability** — Word bank stored in a single `Dictionary` constant for easy editing
- **Reliability** — Input guards prevent crashes regardless of player input

---

## Categories & Word Bank

| Category | Sample Words |
|---|---|
|  Animals | elephant, crocodile, butterfly, kangaroo… |
|  Countries | philippines, australia, switzerland, madagascar… |
|  Technology | algorithm, compiler, encryption, recursion… |
|  Sports | basketball, badminton, gymnastics, triathlon… |
|  Food | spaghetti, croissant, quesadilla, bruschetta… |

> 10 words per category · 50 total playable words

---

## How to Run

### Requirements
- [.NET 6 SDK](https://dotnet.microsoft.com/download) or later

### Option A — New project (recommended)
```bash
dotnet new console -n HangmanGame
cd HangmanGame
# Replace the generated Program.cs with Hangman.cs
cp ../Hangman.cs Program.cs
dotnet run
```

### Option B — Run directly
```bash
mkdir HangmanGame && cd HangmanGame
dotnet new console
# Paste or copy Hangman.cs content into Program.cs
dotnet run
```

---

## How to Play

```
1. Launch the program — the welcome screen lists all categories.
2. Enter the number of your chosen category (or 0 for Random).
3. A word is secretly selected — you see only underscores ( _ _ _ _ ).
4. Type one letter per turn and press Enter.
5. Correct letters are revealed in green on the word display.
6. Wrong letters add a body part to the hangman (6 wrong = game over).
7. Win by revealing all letters before the man is fully hanged!
8. After each round, choose Y to play again or N to exit.
```

---

## Game Screenshots

**Welcome Screen**


**Mid-Game Board**


**Game Over**


---

## Project Structure

```
HangmanGame/
│
├── Program.cs          ← All game logic (single-file console app)
│
└── HangmanGame.csproj  ← .NET project file (auto-generated)
```

### Key Methods

| Method | Responsibility |
|---|---|
| `Main()` | Entry point; manages the play-again loop and session score |
| `ShowWelcome()` | Renders the ASCII title banner and category list |
| `PickWord()` | Displays category menu and returns a random word |
| `PlayRound()` | Core game loop — handles guesses, win/loss detection |
| `RenderBoard()` | Redraws the full game state each turn |
| `WriteColour()` | Reusable helper for colour-coded console output |

---

## Concepts Used

| Concept | Where Applied |
|---|---|
| `Dictionary<string, List<string>>` | Word bank grouped by category |
| `HashSet<char>` | O(1) lookup for guessed and correct letters |
| `string.All()` / `string.Contains()` | Win detection and letter checking |
| `Console.ForegroundColor` | Colour-coded player feedback |
| LINQ (`.OrderBy`, `.Except`, `.ToList`) | Sorting and filtering guessed letters |
| Conditional statements | Win/loss logic and input validation |
| `System.Random` | Random category and word selection |

---

## Team

| Name | Role | Contribution |
|---|---|---|
| Sean Jeremy S. Sulayao (acedevph) | Lead Programmer | Game architecture, all C# code, ASCII art, colour UI |
| Nicole Baluyot | Researcher | Word bank curation, project documentation |

---

## License

This project was created for educational purposes as part of a C# programming course.  
Feel free to fork, expand, and improve it!

---
