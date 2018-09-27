using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrettioEtt
{


    class Program
    {

        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            Game game = new Game();

            List<Player> players = new List<Player>();

            players.Add(new L33tt4rd());
            players.Add(new BasicPlayer());
            int[] p = new int[2];
            Console.WriteLine("Vilka två spelare skall mötas?");
            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine(i + ": {0}", players[i].Name);
                do
                {
                    while (!int.TryParse(Console.ReadLine(), out p[i]))
                    {
                        Console.WriteLine("Felaktig symbol. Du måste ange ett numeriskt värde...");
                    }
                    if (p[i] >= players.Count)
                    {
                        Console.WriteLine($"Ogiltig spelare angiven {p[1]}. Välj ett värde mellan 0 och {players.Count -1}");
                    }
                } while (p[i] >= players.Count);
                
            }
                
            Player player1 = players[p[0]];
            Player player2 = players[p[1]];
            player1.Game = game;
            player1.PrintPosition = 0;
            player2.Game = game;
            player2.PrintPosition = 9;
            game.Player1 = player1;
            game.Player2 = player2;
            Console.WriteLine("Hur många spel skall spelas?");
            int numberOfGames = int.Parse(Console.ReadLine());
            Console.WriteLine("Skriva ut första spelet? (y/n)");
            string print = Console.ReadLine();
            Console.Clear();
            if (print == "y")
                game.Printlevel = 2;
            else
                game.Printlevel = 0;
            game.initialize();
            game.PlayAGame(true);
            Console.Clear();
            bool player1starts = true;

            for (int i = 1; i < numberOfGames; i++)
            {
                game.Printlevel = 0;
                player1starts = !player1starts;
                game.initialize();
                game.PlayAGame(player1starts);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, 3);
                Console.Write(player1.Name + ":");
                Console.ForegroundColor = ConsoleColor.Green;

                Console.SetCursorPosition((player1.Wongames * 100 / numberOfGames) + 15, 3);
                Console.Write("█");
                Console.SetCursorPosition((player1.Wongames * 100 / numberOfGames) + 16, 3);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(player1.Wongames);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, 5);
                Console.Write(player2.Name + ":");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((player2.Wongames * 100 / numberOfGames) + 15, 5);
                Console.Write("█");
                Console.SetCursorPosition((player2.Wongames * 100 / numberOfGames) + 16, 5);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(player2.Wongames);

            }
            Console.ReadLine();

        }

    }











}