using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace RegicideSolitare
{
    /* Regicide Solitare 
     * Card Class
     * Silas U. Barr
     * Based off "Regicide" by Badgers form Mars
     * Jan 19 - 21, 2026
    */

    enum SUIT
    {
        Club, Diamond, Heart, Spade
    }

    internal class Card
    {
        static Random ran = new Random();

        public int Rank;
        public SUIT Suit;
        public bool Royal;

        public Card(int rank, int suit,bool royal = false)
        {
            Royal = royal;
            Rank = rank;
            switch (suit)
            {
                case 1: Suit = SUIT.Club; break;
                case 0: Suit = SUIT.Diamond;break;
                case 2: Suit = SUIT.Heart; break;
                case 3: Suit = SUIT.Spade; break;
            }
        }

        public override string ToString()
        {
            /*[(Rank)(Suit)]
             * [
             * 
             * [
             */
           


            //String Rank
            string sr = string.Empty;
            string ss = string.Empty;
            if (Royal||Rank==1)
            {
                switch(Rank) 
                {
                    case 1: sr = "A";break;
                    case 10: sr = "J";break;
                    case 15: sr = "Q";break;
                    case 20: sr = "K";break;
                }
            }
            else if(Rank == 10) { sr = "T"; }
            else { sr = Rank.ToString(); }

            switch(Suit)
            {
                case SUIT.Spade: ss = "S"; break;
                case SUIT.Club: ss = "C"; break;
                case SUIT.Diamond: ss = "D"; break;
                case SUIT.Heart: ss = "H"; break;
            }

            return $"[{sr} {ss}]";
        }

        public int getSuitInt()
        {
            switch (Suit)
            {
                case SUIT.Spade: return 3;
                case SUIT.Club: return 1;
                case SUIT.Diamond: return 0;
                  case SUIT.Heart: return 2;
                    default : return -1;
            }
        }

        public static void Suffle(List<Card> list)
        {
            
            List<Card> ushuff = new List<Card>();
            for (int i = 0; i < list.Count; i++)
            {
                ushuff.Add(new Card(list[i].Rank, list[i].getSuitInt(), list[i].Royal));
            }
            
            for (int i = ushuff.Count-1; i >= 0 ;i--)
            {
                int r = ran.Next(ushuff.Count);
                list[i] = ushuff[r];
                ushuff.RemoveAt(r);
            }

        }
    }
}
