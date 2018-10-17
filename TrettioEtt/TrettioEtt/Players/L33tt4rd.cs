using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrettioEtt.Players
{
    class L33tt4rd : Player  // HuvudSpelaren
    {
        
        //Lägg gärna till egna variabler här
        public int EnemyScore()
        {
            return 0;
        }

        public L33tt4rd() //Skriv samma namn här
        {
            Name = "L33tt4rd"; //Skriv in samma namn här
        }

        /// <summary>
        /// Returnerar true om spelaren skall knacka, annars false.
        /// </summary>
        /// <param name="round">
        /// Mängden rundor som gått i ett spel.
        /// </param>
        /// <param name="cardsLeft">
        /// Hur många kort som finns kvar i kortleken.
        /// </param>
        /// <returns>
        /// Returnerar om den ska knacka eller inte.
        /// </returns>
        public override bool Knacka(int round, int cardsLeft) 
        {
            // kollar hur många kort som är kvar och anpassar knackningsbenägenheten.
            if (cardsLeft > 40)
            {
                if (Game.Score(this) > 20) return true;
                else return false;
            }
            else if(cardsLeft > 30)
            {
                if (Game.Score(this) > 22) return true;
                else return false;
            }
            else if (cardsLeft > 20)
            {
                if (Game.Score(this) > 24) return true;
                else return false;
            }
            else if (cardsLeft > 10)
            {
                if (Game.Score(this) > 26) return true;
                else return false;
            }
            else if (cardsLeft > 5)
            {
                if (Game.Score(this) > 28) return true;
                else return false;
            }
            else
            {
                if (Game.Score(this) > 30) return true;
                else return false;
            }
        }

        public override bool TaUppKort(Card card) // Returnerar true om spelaren skall ta upp korten på skräphögen (card), annars false för att dra kort från leken.
        {
            Card worstCard = Hand.FirstOrDefault();
            for (int i = 1; i < Hand.Count; i++)
            {
                // Om kortet är ett ess eller medium poäng av bestSuit kastas det ej.
                if (CardValue(Hand[i]) < CardValue(worstCard) && worstCard.Value != 11)
                {
                    worstCard = Hand[i];
                }
            }

            // Om kortet har värdet 9 eller högre av samma färg eller är ett ess plockas det upp
            if (card.Value == 11 || (CardValue(card) > CardValue(worstCard) && card.Value > 8) || (card.Value >= 8 && card.Suit == BestSuit))
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
                // Om kortet är ett ess eller medium poäng av bestSuit kastas det ej.
                if (CardValue(Hand[i]) < CardValue(worstCard) && worstCard.Value != 11)
                {
                    worstCard = Hand[i];
                }
            }
            return worstCard;
            //return Hand.OrderBy(c => c.Value).First();
        }

        public override void SpelSlut(bool wonTheGame) // Anropas när ett spel tar slut. Wongames++ får ej ändras!
        {
            if (wonTheGame)
            {
                Wongames++;
            }
        }

        /// <summary>
        /// Används för att beräkna värdet av ett kort.
        /// </summary>
        /// <param name="card">
        /// Kortet vars värde beräknas.
        /// </param>
        /// <returns></returns>
        private int CardValue(Card card) // Hjälpmetod som kan användas för att värdera hur bra ett kort är
        {
            int cardValue = card.Value;

            if (card.Suit == BestSuit)
            {
                cardValue += 9;
            }
            return cardValue;
        }

        

        // Lägg gärna till egna hjälpmetoder här
    }
}
