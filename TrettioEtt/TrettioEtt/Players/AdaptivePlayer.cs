using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrettioEtt.Players
{
    /// <summary>
    /// 
    /// </summary>
    class AdaptivePlayer : Player  // Denna spelare är testfiende för att pröva idéer.
    {
        List<int> enemyScoreList = new List<int>();
        int numberOfGames = 0;
        //Lägg gärna till egna variabler här

        public AdaptivePlayer() //Skriv samma namn här
        {
            Name = "AdaptivePlayer"; //Skriv in samma namn här
        }

        /// <summary>
        /// Bestämmer om spelaren ska knacka när den kallas.
        /// </summary>
        /// <param name="round">
        /// Mängden rundor som har gått.
        /// </param>
        /// <param name="cardsLeft">
        /// Mängden kort kvar i kortleken.
        /// </param>
        /// <returns></returns>
        public override bool Knacka(int round, int cardsLeft) //Returnerar true om spelaren skall knacka, annars false
        {

            if (round < 3)
            {
                enemyScore = 0;
            }

            // Om medelvärdet av fiendens score är större än spelarens score knackar den ej och vice versa.
            if (Game.Score(this) < AverageEnemyScore(enemyScoreList))
            {
                return false;
            }
            else if (Game.Score(this) >= AverageEnemyScore(enemyScoreList))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool TaUppKort(Card card) // Returnerar true om spelaren skall ta upp korten på skräphögen (card), annars false för att dra kort från leken.
        {
            if (card.Value == 11 || (card.Value >= 8 && card.Suit == BestSuit))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override Card KastaKort()  // Returnerar det kort som skall kastas av de fyra som finns på handen
        {
            Game.Score(this);
            Card worstCard = Hand.First();
            for (int i = 1; i < Hand.Count; i++)
            {
                if (Hand[i].Value < worstCard.Value) 
                {
                    worstCard = Hand[i];
                }
            }
            return worstCard;
        }

        public override void SpelSlut(bool wonTheGame) // Anropas när ett spel tar slut. Wongames++ får ej ändras!
        {
            if (wonTheGame)
            {
                Wongames++;
            }
            if (enemyScore != 0)
            {
                // när SpelSlut kallas läggs fiendens score till i enemyScoreList vilket är en lista av int-variabler
                enemyScoreList.Add(enemyScore);
            }
            numberOfGames++;
        }

        private int CardValue(Card card) // Hjälpmetod som kan användas för att värdera hur bra ett kort är
        {
            int cardValue = card.Value;
            // Om ett kort har samma suit som BestSuit ökas dens returnerade värde med 5.
            if (card.Suit == BestSuit)
            {
                cardValue += 5;
            }
            return cardValue;
        }

        /// <summary>
        /// I denna metod skickas det in en lista an int-variabler motsvarande motståndarens resultat, sedan returnerar den medelvärdet.
        /// </summary>
        /// <param name="score">
        /// En lista av int-variabler som metoden räknar ut medelvärdet på.
        /// </param>
        /// <returns></returns>
        static float AverageEnemyScore(List<int> score)
        {
            int antal = score.Count();
            int summa = 0;
            for (int i = 0; i < antal; i++)
            {
                summa += score[i];
            }
            if (antal == 0)
            { antal++; }
            int enemyScore = summa / antal;
            return enemyScore;
        }
        // Lägg gärna till egna hjälpmetoder här
    }

}
