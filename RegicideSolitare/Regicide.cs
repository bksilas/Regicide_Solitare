using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegicideSolitare
{
    /* Regicide Solitare 
     * Regicide Class
     * Silas U. Barr
     * Based off "Regicide" by Badgers form Mars
     * Jan 19 - 21, 2026
    */

    internal class Regicide
    {
        
        //Varibles 
        int ReDeals = 0;
        int TurnCount = 0;
        Card Enemy;
        int HP = 10; int Type = -1;
        int Attack = 10, CurAttack = 10;
        bool Phase = true;

        List<Card>  TavernDeck = new List<Card>();
        List<Card> DiscardDeck = new List<Card>();
        Stack<Card> CastleDeck = new Stack<Card>();
        List<Card> Hand = new List<Card>();
        Boolean[] Selected = new Boolean[8];

        bool Testing = false;

        public Regicide() 
        {
        }

        public void test()
        {
            Testing = true;
            //foreach (Card card in CastleDeck) { Console.WriteLine(card); }

            MenuNavigation();
            Type = -1;
            //for (int i = 0; i < 1; i++) { Selected[i] = true;}

            //Hand.Add(new Card(4, 1));
            //Hand.Add(new Card(5, 1));
            //Console.WriteLine(CheckCombos());
        }

        public void Setup()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;


            //Tavern Deck
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    TavernDeck.Add(new Card(j, i));
                }
            }
            


            //Castle Deck 
            for (int j = 20; j > 5; j-=5)
            {
                List<Card> tempcards = new List<Card>();
                for (int k = 0; k < 4; k++)
                {
                    tempcards.Add(new Card(j, k,true));
                }
                Card.Suffle(tempcards);
                CastleDeck.Push(tempcards[0]);
                CastleDeck.Push(tempcards[1]);
                CastleDeck.Push(tempcards[2]);
                CastleDeck.Push(tempcards[3]);
            }

            Card.Suffle(TavernDeck);
        }

        public void Run()
        {
            Setup();
            GetNextEnemy();
            Draw(8);
            MenuNavigation();
        }

        public void MenuNavigation(int mState = 0)//Menu State
        {
            /*States
             * 0 - Defult Menu
             * 1 - Selection/UnSelction
             * 2 - Play
             * 3 - Redraw
             * 4 - Exit Menu
             * 5 - Win
             * 6 - Exit
            */
            do
            {
                PrintScreen(mState);
                int input = 0;

                //Input Checks
                try { input = int.Parse(Console.ReadLine()); }
                catch { PrintStatement(0); continue; } // Is Digit Check

                switch (mState)
                {
                    case 0:
                        {
                            if (input < 1 || input > 4) { PrintStatement(1); } //Is Correct Digit Check
                            else { mState = input; } //All SubMenus
                            break;
                        }
                    case 1:
                        {
                            if (input < 0 || input > 9 || input == 8) { PrintStatement(1); }  //Is Correct Digit Check
                            else
                            {
                                if (input == 9) { mState = 0; }
                                else if (Hand.Count > input) { Selected[input] = !Selected[input]; }
                                else { PrintStatement(2); }
                            }
                            break;
                        }
                    case 2:
                        {
                            if (input != 1 && input != 2) { PrintStatement(1); continue; } //Is Correct Digit Check
                            else
                            {
                                if (input == 1) { if (AttackAndDefenseLogic() == -1) { mState = 5; }; } //Actual game 
                                mState = 0;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (input != 1 && input != 2) { PrintStatement(1); continue; } //Is Correct Digit Check
                            else
                            {
                                if (input == 1) { Redraw(); }
                                mState = 0;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (input != 1 && input != 2) { PrintStatement(1); continue; } //Is Correct Digit Check
                            else
                            {
                                if (input == 1) { mState = 6; } //Ends the Game
                                else { mState = 0; }
                            }
                            break;
                        }
                    case 5:
                        {
                            if (input != 1 && input != 2) { PrintStatement(1); continue; } //Is Correct Digit Check
                            else
                            {
                                if (input == 2) { mState = 6;  } //Ends the Game
                                else { mState = 0; }
                            }
                            break;
                        }
                }
            
            } while (mState != 6);
            System.Environment.Exit(0);

        }

        public void PrintScreen(int menu = 0)
        {
            if (!Testing) { Console.Clear(); }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t\t\tRegicide\n");
            Console.ForegroundColor = ConsoleColor.Black;


            //Exit Screen
            if (menu == 4) { Console.WriteLine("Are you sure you want to Exit\n| 1: Yes\n| 2: No"); }
            else if (menu == 3) { Console.WriteLine("Are you sure you want to Redraw your Hand\n| 1: Yes\n| 2: No"); } 
            else if (menu == 2) { Console.WriteLine("Are you sure you want to Play these Cards\n| 1: Yes\n| 2: No"); }
            else if(menu == 5) { Console.WriteLine("CONGRATS, you have killed the Royal Family\nWould you like to Play again\n| 1: Yes\n| 2: No"); }
            else
            {
                Console.Write($"Turn: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{TurnCount,-37}"); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Redraws: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{ReDeals,2}\n"); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"Deck: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{TavernDeck.Count,-37}"); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Discard: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{DiscardDeck.Count,2}\n"); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"Curent Enemy: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{Enemy,-22}"); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Attack: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{CurAttack,-3} "); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("HP: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{HP,2}\n"); Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Phase: "); Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write($"{(Phase?"Attack": "Defense") }\n\n"); Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine($"Hand:");
                for (int i = 0; i < 8; i++)
                {
                    if (i < Hand.Count) { Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write(Hand[i]); Console.ForegroundColor = ConsoleColor.Black; }
                    else { Console.Write("empty"); }
                    if (i != 7) { Console.Write(", "); }
                    else Console.Write("\n");
                }

                for (int i = 0; i < 8; i++)
                {
                    Console.Write($"{i}: ");
                    if (Selected[i]) { Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write(" *  "); Console.ForegroundColor = ConsoleColor.Black; }
                    else Console.Write("    ");
                }

                Console.WriteLine("\n\nMenu");
                switch (menu)
                {
                    case 0: Console.Write("| 1: Select\n| 2: Play\n| 3: Redraw\n| 4: Exit"); break;
                    case 1: Console.Write("| 0-7: Choose Index(s) to Select/UnSelect\n| 9: Exit"); break;
                }

                Console.WriteLine("\n");
            }
        }

        public void PrintStatement(int mess)
        {
            if (!Testing) { Console.Clear(); };
            switch (mess)
            {
                case 0: { Console.WriteLine("Please Enter a Number");  break; }
                case 1: {  Console.WriteLine("Please Enter a Valid Number"); break; }
                case 2: { Console.WriteLine("You Can Not Select an Empty Postion");break; }
                case 3: { Console.WriteLine("Not a Valid Combo"); break;}
                case 4: { Console.WriteLine("Not Enough Defense"); break; }
                case 5: { Console.WriteLine($"Congrats You Killed {Enemy}");break; }
                case 6: { Console.WriteLine("Congrats you have killed the Royal Family and Won the Game");break; }
            }
            Console.ReadLine();
        }

        public int AttackAndDefenseLogic()//true if succeded
        {
            int amt = CheckCombos();
            string effs = "";
            List<Card> cardBuffer = new List<Card>();

            if(Phase)
            {
                if (amt < 0)
                {
                    PrintStatement(3);
                    return 0;
                }

                for (int i = 0; i < 8; i++) 
                { 
                    if (Selected[i]) 
                    {
                        switch (Hand[i].Suit)
                        {
                            case SUIT.Diamond: { effs += "d"; break; }
                            case SUIT.Club: { effs += "c"; break; }
                            case SUIT.Spade: { effs += "s"; break; }
                            case SUIT.Heart: { effs += "h"; break; }
                        }
                    } 
                }
            }
            else
            {
                if(-amt < CurAttack) { PrintStatement(4); return 0; }
            }

            for (int i = 7; i > -1; i--) 
            {
                if (Selected[i]) { cardBuffer.Add(Hand[i]); Hand.RemoveAt(i);  }
                CurAttack = Attack;
            }

            if (Phase) 
            {
                int result = EffectsAndAttacks(amt, effs);
                if (result == 1) { Phase = !Phase; }
                else if (result == -1) { return -1; }
            }
            DiscardDeck.AddRange(cardBuffer); 

            ClearSelected();

            Phase = !Phase;
            return 1;
        }

        public int CheckCombos()
        {
            int cRank = -1,aces = 0,amt =0;

            for (int i = 0; i < 8; i++) 
            {
                if (Selected[i]) 
                {
                    int t = Hand[i].Rank; 

                    if(t != 1 && cRank == -1) { cRank = t; }//Setting a NonAce Rank to Track
                    else if(t == 1) {  aces++; }//Tracking Aces
                    amt += t; 
                }
            }

            if( cRank == -1 && aces > 0) { cRank = 1; }//Checks if all Aces


            //Checks if Combo Valid
            // 1 Rank Combos have to Use Ranks <= 5
            // 2 Rank Combos amt <= 10
            // 3 Rank Combos Cannot include Aces
            // 4 Rank Combo Cards have to be the Same Rank
            // 5 Aces can be used with Any Rank

            if (amt != cRank)//Otherwise its 1 Selected Card
            {
                if(aces > 0)
                {
                    if(amt > cRank * 2) { amt = -amt; }// 3
                }
                else
                {
                    if(cRank > 5) { amt = -amt; } // 1
                    else if (amt > 10) {  amt = -amt; } // 2
                    else if(amt % cRank != 0) { amt = -amt; }//4
                }
            }
            else if (!Phase) { amt = -amt; }

             return amt; 
        }

        public void Heal(int amt)
        {
            if (amt > DiscardDeck.Count) { amt  = DiscardDeck.Count; }

            Card.Suffle(DiscardDeck);

            for(int i = amt; i > 0; i--) { TavernDeck.Add(DiscardDeck[0]); DiscardDeck.RemoveAt(0); }
        }

        public void Draw(int amt)
        {
            if (amt > 8 - Hand.Count) { amt = 8 - Hand.Count; }

            for (int i = amt; i > 0; i--) { Hand.Add(TavernDeck[0]); TavernDeck.RemoveAt(0); }
        }

        public int EffectsAndAttacks(int amt, string eff)//Kill 1 Win -1 Survive 0
        {
            //Effects
            if (eff.Contains("d")&&Type != 0) { Draw(amt); }
            if (eff.Contains("h")&& Type != 2) { Heal(amt); }
            if (eff.Contains("c")&&Type != 1) { HP -= amt; }
            if(eff.Contains("s")&& Type != 3) { CurAttack -= amt; }

            //Attack
            HP -= amt;
            if (HP <= 0) 
            {
                if(CastleDeck.Count > 0)
                {
                    KillEnemy();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }

        public bool Redraw()
        {
            DiscardDeck.AddRange(Hand);
            Hand.Clear();
            Draw(8);
            ReDeals++;
            ClearSelected();
            return true;
        }

        public void ClearSelected()
        {
            for(int i  = 0; i < Selected.Length; i++) { Selected[i] = false; }
        }

        public void KillEnemy()
        {
            PrintStatement(5);
            DiscardDeck.Add(Enemy);
            GetNextEnemy();
        }

        public void GetNextEnemy()
        {
            Enemy = CastleDeck.Pop();

            switch(Enemy.Suit)
            {
                    case SUIT.Club: Type = 1;break;
                    case SUIT.Diamond: Type = 0;break;
                    case SUIT.Heart: Type = 2;break;
                    case  SUIT.Spade: Type = 3;break;
            }

            switch (Enemy.Rank)
            {
                    case 10: Attack = 10; HP = 20; break;
                    case 15: Attack = 15; HP = 30; break;
                    case 20: Attack = 20; HP = 40; break;
            }
        }
    }
}
