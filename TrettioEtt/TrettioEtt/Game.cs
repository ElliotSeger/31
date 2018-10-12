﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrettioEtt
{
    class Game
    {

        List<Card> CardDeck = new List<Card>();
        List<Card> DiscardPile = new List<Card>();
        public bool Lastround;
        int Cardnumber;
        public int Printlevel;
        public int Discardnumber;
        public Player Player1 { private get; set; }
        public Player Player2 { private get; set; }
        Random RNG = new Random();
        public int NbrOfRounds;

        public Game()
        {

        }

        public void initialize()
        {
            Lastround = false;
            Cardnumber = -1;
            Discardnumber = 52;
            CardDeck = new List<Card>();
            DiscardPile = new List<Card>();
            Player1.Hand = new List<Card>();
            Player2.Hand = new List<Card>();

            int id;
            int suit;
            for (int i = 0; i < 52; i++)
            {
                id = i % 13 + 1;
                suit = i % 4;
                CardDeck.Add(new Card(id, (Suit)suit));
            }
            Shuffle();
            for (int i = 0; i < 3; i++)
            {
                Player1.Hand.Add(DrawCard());
                Player2.Hand.Add(DrawCard());
            }
            Discard(DrawCard());
        }

        public void printHand(Player player)
        {
            Console.SetCursorPosition(0, player.PrintPosition);
            Console.WriteLine(player.Name + " har ");
            for (int i = 0; i < player.Hand.Count; i++)
            {
                player.Hand[i].PrintCard();
                Console.WriteLine();
            }
        }

        private int playARound(Player player, Player otherPlayer)
        {
            if (Printlevel > 1)
            {
                printHand(player);
                Console.SetCursorPosition(4, 6);
                Console.Write("På skräphögen ligger ");
                DiscardPile.Last().PrintCard();

            }
            otherPlayer.OpponentsLatestCard = null;
            if (NbrOfRounds > 1 && player.Knacka(NbrOfRounds) && !Lastround)
            {
                if (Printlevel > 1)
                {
                    Console.SetCursorPosition(20, player.PrintPosition + 2);
                    Console.Write(player.Name + " knackar!");
                }
                return Score(player);
            }
            else if (player.TaUppKort(DiscardPile.Last()))
            {

                player.Hand.Add(PickDiscarded());
                otherPlayer.OpponentsLatestCard = player.Hand.Last();
                if (Printlevel > 1)
                {
                    Console.SetCursorPosition(20, player.PrintPosition + 2);
                    Console.Write(player.Name + " plockar ");
                    player.Hand.Last().PrintCard();
                    Console.Write(" från skräphögen.");
                }
            }
            else
            {
                player.Hand.Add(DrawCard());
                if (Printlevel > 1)
                {
                    Console.SetCursorPosition(20, player.PrintPosition + 2);
                    Console.Write(player.Name + " drar ");
                    player.Hand.Last().PrintCard();
                }
            }
            Card discardcard = player.KastaKort();

            UpdateHand(player, discardcard);
            if (Printlevel > 1)
            {
                Console.SetCursorPosition(20, player.PrintPosition + 3);
                Console.Write(player.Name + " kastar bort ");
                discardcard.PrintCard();
                Console.Write("       Tryck ENTER");
                Console.ReadLine();
                Console.Clear();
                printHand(player);

            }

            Discard(discardcard);
            if (Score(player) == 31)
            {
                return 31;
            }
            else
            {
                return 0;
            }
        }

        private void UpdateHand(Player player, Card discardcard)
        {
            player.Hand.Remove(discardcard);

        }

        public int Score(Player player) //Reurnerar spelarens poäng av bästa färg. Uppdaterar player.bestsuit.
        {
            int[] suitScore = new int[4];
            if (player.Hand[0].Value == 11 && player.Hand[1].Value == 11 && player.Hand[2].Value == 11)
            {
                return 31;
            }

            for (int i = 0; i < player.Hand.Count; i++)
            {
                if (player.Hand[i] != null)
                    suitScore[(int)player.Hand[i].Suit] += player.Hand[i].Value;
            }
            int max = 0;

            for (int i = 0; i < 4; i++)
            {
                if (suitScore[i] > max)
                {
                    max = suitScore[i];
                    player.BestSuit = (Suit)i;
                }
            }
            return max;

        }

        public int SuitScore(List<Card> hand, Suit suit) //Returnerar handens poäng av en viss färg
        {
            int sum = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] != null && hand[i].Suit == suit)
                {
                    sum += hand[i].Value;
                }

            }
            return sum;


        }

        public int HandScore(List<Card> hand, Card excluded) //Returnerar handens poäng av bästa färg. Undantar ett kort från beräkningen (null för att ta med alla kort)
        {
            int[] suitScore = new int[4];
            int aces = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] != null && hand[i] != excluded)
                {
                    suitScore[(int)hand[i].Suit] += hand[i].Value;
                    if (hand[i].Value == 11)
                    {
                        aces++;
                    }
                }

            }
            if (aces == 3)
                return 31;
            int max = 0;

            for (int i = 0; i < 4; i++)
            {
                if (suitScore[i] > max)
                {
                    max = suitScore[i];
                }
            }
            return max;
        }

        public void PlayAGame(bool player1starts)
        {
            NbrOfRounds = 0;
            Player playerInTurn, playerNotInTurn, temp;
            if (player1starts)
            {
                playerInTurn = Player1;
                playerNotInTurn = Player2;
            }
            else
            {
                playerInTurn = Player2;
                playerNotInTurn = Player1;
            }
            while (Cardnumber < 51 && NbrOfRounds < 100)
            {
                NbrOfRounds++;
                int result = playARound(playerInTurn, playerNotInTurn);
                if (result == 31)
                {
                    if (Printlevel > 1)
                        printHand(playerNotInTurn);

                    playerInTurn.SpelSlut(true);
                    playerNotInTurn.SpelSlut(false);
                    if (Printlevel > 0)
                    {
                        Console.SetCursorPosition(15, playerInTurn.PrintPosition + 5);
                        Console.Write(playerInTurn.Name + " fick 31 och vann spelet!");
                        Console.ReadLine();
                    }
                    break;
                }
                else if (result > 0)
                {
                    Lastround = true;
                    playerNotInTurn.lastTurn = true;
                    playARound(playerNotInTurn, playerInTurn);
                    playerNotInTurn.lastTurn = false;
                    if (Printlevel > 1)
                    {
                        printHand(playerInTurn);
                        printHand(playerNotInTurn);
                    }


                    if (Printlevel > 0)
                    {
                        Console.SetCursorPosition(15, playerInTurn.PrintPosition + 5);
                        Console.Write(playerInTurn.Name + " knackade och har " + Score(playerInTurn) + " poäng");
                        Console.SetCursorPosition(15, playerNotInTurn.PrintPosition + 5);
                        Console.Write(playerNotInTurn.Name + " har " + Score(playerNotInTurn) + " poäng");
                    }
                    if (Score(playerInTurn) > Score(playerNotInTurn))
                    {
                        playerNotInTurn.enemyScore = Score(playerInTurn);
                        playerInTurn.SpelSlut(true);
                        playerNotInTurn.SpelSlut(false);
                        if (Printlevel > 0)
                        {
                            Console.SetCursorPosition(15, playerInTurn.PrintPosition + 6);
                            Console.WriteLine(playerInTurn.Name + " vann!");
                            Console.ReadLine();
                        }
                        break;
                    }
                    else
                    {
                        playerNotInTurn.enemyScore = Score(playerInTurn);
                        playerInTurn.SpelSlut(false);
                        playerNotInTurn.SpelSlut(true);
                        if (Printlevel > 0)
                        {
                            Console.SetCursorPosition(15, playerNotInTurn.PrintPosition + 6);
                            Console.WriteLine(playerNotInTurn.Name + " vann!");
                            Console.ReadLine();
                        }
                        break;
                    }
                }
                else
                {
                    temp = playerNotInTurn;
                    playerNotInTurn = playerInTurn;
                    playerInTurn = temp;
                }

            }
            if (Cardnumber >= 51 || NbrOfRounds >= 100)
            {
                if (Printlevel > 0)
                {
                    Console.SetCursorPosition(0, 20);
                    Console.WriteLine("Korten tog slut utan att någon spelare vann.");
                    Console.ReadLine();
                }
                playerInTurn.SpelSlut(false);
                playerNotInTurn.SpelSlut(false);

            }

        }

        private void Discard(Card card)
        {
            Discardnumber--;
            DiscardPile.Add(card);
        }

        private Card DrawCard()
        {
            Cardnumber++;
            Card card = CardDeck.First();
            CardDeck.RemoveAt(0);
            return card;
        }

        private Card PickDiscarded()
        {
            Card card = DiscardPile.Last();
            Discardnumber++;
            return card;
        }

        private void Shuffle()
        {
            for (int i = 0; i < 200; i++)
            {
                switchCards();
            }
        }

        private void switchCards()
        {
            int card1 = RNG.Next(CardDeck.Count);
            int card2 = RNG.Next(CardDeck.Count);
            Card temp = CardDeck[card1];
            CardDeck[card1] = CardDeck[card2];
            CardDeck[card2] = temp;
        }
    }
}
