class Tombala
{
     static char[] bag = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', //M is index 12
                         'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1'}; //Z is index 25
                                                                                                // '0' means the letter is removed, '1' is joker
     static int remaining_letters = 27; // 27 remaining letters (including joker) in the bag

     static char[,] player1 = new char[2, 4]; // default (when a letter is removed) = 0
     static char[,] player2 = new char[2, 4];
     static Random random = new Random();
     static char[] p1l1_min_and_max = new char[2];
     static char[] p1l2_min_and_max = new char[2];
     static char[] p2l1_min_and_max = new char[2];
     static char[] p2l2_min_and_max = new char[2];

     static char[,] Initialize() // use only once at the beginning of the game
     {
          char[] temp_alph = (char[])bag.Clone();
          char[,] init = new char[2, 4];
          for (byte i = 0; i < 4; i++) // letters from A to M , upper line
          {
               int upperline_letter = random.Next(13); // returns an index up to 12 (M)
               if (temp_alph[upperline_letter].Equals('0')) // the random index is an already drawn letter 
               {
                    i--;
                    continue;
               }
               init[0, i] = temp_alph[upperline_letter];
               temp_alph[upperline_letter] = '0';
          }
          for (byte i = 0; i < 4; i++) // letters from N to Z, bottom line
          {
               int bottomline_letter = random.Next(13, 26); // random between index 13 (N) and index 25 (Z)
               if (temp_alph[bottomline_letter].Equals('0'))
               {
                    i--;
                    continue;
               }
               init[1, i] = temp_alph[bottomline_letter];
               temp_alph[bottomline_letter] = '0';
          }
          return init;
     }

     static sbyte[] Search(char[,] card, char target)
     {                                                                // Linear Search has better time complexity
          for (sbyte i = 0; i < 2; i++)                               // than Binary Search with UNSORTED 8 elements
               for (sbyte j = 0; j < 4; j++)
                    if (card[i, j].Equals(target)) return [i, j];     // Returns the index of the letter, [-1, -1] if 
                                                                      // the player's card does not have the letter
          return [-1, -1];
     }

     static char DrawLetterFromBag() // draws a letter from the bag, might be Joker ('1')
     {
          bool newToken = false;
          int tokenIndex;
          do
          {
               tokenIndex = random.Next(27); // returns an index up to 26 [26th index is Joker('1')]
               if (!bag[tokenIndex].Equals('0')) newToken = true;
          } while (!newToken);
          char token = bag[tokenIndex];
          bag[tokenIndex] = '0';
          remaining_letters--;
          return token;
     }
     static void Display()
     {
          Console.Clear();
          Console.SetCursorPosition(0, 0);
          char[,] player = player1;
          for (int p = 0; p < 2; p++)
          {
               for (int i = 0; i < 2; i++)
               {
                    for (int j = 0; j < 4; j++)
                    {
                         if (!player[i, j].Equals('0')) Console.Write(player[i, j] + " ");
                    }
                    if (p == 0) Console.WriteLine();
                    else Console.SetCursorPosition(15, 1);
               }
               player = player2;
               Console.SetCursorPosition(15, 0);
          }
          Console.SetCursorPosition(0, 3);
     }

     static char[] FindMinAndMaxLetter(char[,] card, byte mode) // Upper line: Mode = 0
     {                                                          // Bottom line: Mode = 1   else is wrong use
                                                                // RETURNS LETTERS, not their index
          char min = card[mode, 0];
          char max = card[mode, 0];
          for (int j = 1; j < 4; j++)
          {
               if (card[mode, j] < min) min = card[mode, j];
               else if (card[mode, j] > max) max = card[mode, j];
          }
          return [min, max];
     }

     static char SetMaxIndexToZero(ref char[,] playercard) // When the Joker is drawn, delete Max from the
     {                                                    //  player's card.
                                                          // This also returns the deleted letter (neccessary
                                                          // in order to remove it from the "min-max" array, if
                                                          // exists)
          byte[] maxIndex = { 0, 0 };
          for (int i = 0; i < 2; i++)
               for (int j = 0; j < 4; j++)
               {
                    byte m = maxIndex[0], n = maxIndex[1];
                    if (playercard[i, j] > playercard[m, n])
                    {
                         maxIndex[0] = (byte)i;
                         maxIndex[1] = (byte)j;
                    }
               }
          char output = playercard[maxIndex[0], maxIndex[1]];
          playercard[maxIndex[0], maxIndex[1]] = '0';
          return output;
     }
     static void SetTargetToZeroOnMinMaxArray(char target, ref char[] upper, ref char[] lower)
     // If the drawn letter is an inital min-max, it should also be removed
     // from the min-max array
     {
          if (lower[1].Equals(target)) lower[1] = '0';
          else if (lower[0].Equals(target)) lower[0] = '0';
          else if (upper[1].Equals(target)) upper[1] = '0';
          else if (upper[0].Equals(target)) upper[0] = '0';
          // does nothing if the letter is not an initial min-max
     }
     static void SetTargetToZeroOnMinMaxArray(char targ) // if the target is the same letter
                                                         // (use if the drawn token is not Joker)
     {
          SetTargetToZeroOnMinMaxArray(targ, ref p1l1_min_and_max, ref p1l2_min_and_max);
          SetTargetToZeroOnMinMaxArray(targ, ref p2l1_min_and_max, ref p2l2_min_and_max);
          // does nothing if the letter is not an initial min-max
     }

     static int GivePoints(char token, int scoremultiplier)
     {
          if (token.Equals('A') || token.Equals('E') || token.Equals('I') ||
               token.Equals('O') || token.Equals('U'))
          { // 3 x score multiplier (vowel)
               return scoremultiplier * 3;
          }
          else
               return scoremultiplier * 2;
     }

     static void Main()
     {
          Console.Clear();
          bool firstChinko = false;
          //   vvv VARIABLES vvv

          int p1_l1 = 0, p1_l2 = 0, p2_l1 = 0, p2_l2 = 0; // Player1 Line1 .. etc.
                                                          //    p1_l1 == 4 : Çinko
                                                          //    p1_l1 + p1_l2 == 8 : Tombala

          int player1Score = 0, player2Score = 0;
          int multiplier = 30;
          bool p1l1extra = true, p1l2extra = true, p2l1extra = true, p2l2extra = true;

          



          //   ^^^ VARIABLES ^^^

          player1 = Initialize();
          player2 = Initialize(); // 8 letters on both arrays that satisfy the conditions
          p1l1_min_and_max = FindMinAndMaxLetter(player1, 0); // These hold the "min and max" 
          p1l2_min_and_max = FindMinAndMaxLetter(player1, 1); // letters on each row
          p2l1_min_and_max = FindMinAndMaxLetter(player2, 0); // [minLetter, maxLetter]
          p2l2_min_and_max = FindMinAndMaxLetter(player2, 1); // The function is only used at the beginning.
                                                              // When the min or max value is drawn from the bag,
                                                              // it is changed to '0' in these arrays. Since we only find 
                                                              // min and max once, changing it to '0' will not cause any problem
                                                              // with having the lowest ASCII code
           

          Console.SetCursorPosition(0, 3);
          Display();
          Console.Write("Press enter to start");

          Console.ReadLine();
          while (p1_l1 + p1_l2 < 8 && p2_l1 + p2_l2 < 8
                              && remaining_letters > 0) // Keep drawing new letter from the bag
                                                        // until someone makes "Tombala" or the bag is empty
          {
               Display();
               if (multiplier == 30)
               {
                    Console.SetCursorPosition(0, 3);
                    Console.Write("Press enter to draw a new letter");
               }

               char drawnToken = DrawLetterFromBag();

               Console.SetCursorPosition(0, 2);
               if(!drawnToken.Equals('1'))
                    Console.Write($"{31 - multiplier}. selected letter: {drawnToken}");
               else Console.Write($"{31 - multiplier}. selected letter: JOKER");
               int p1CurrScore = 0, p2CurrScore = 0;

               if (drawnToken.Equals('1')) // Joker is drawn, each player deletes the max letter on their cards
               {
                    char p1Max = SetMaxIndexToZero(ref player1); // Remove max letter from the players' card
                    char p2Max = SetMaxIndexToZero(ref player2); // and returns the letter (it is 
                                                                 // neccessary in order to remove it
                                                                 // from the "min-max" array)
                    SetTargetToZeroOnMinMaxArray(p1Max, ref p1l1_min_and_max, ref p1l2_min_and_max); // Removes it from the 
                    SetTargetToZeroOnMinMaxArray(p2Max, ref p2l1_min_and_max, ref p2l2_min_and_max);// "min-max" array (if exists)

                    if (p1Max < 'N') p1_l1++;  // counts removed letters on each row (4 = çinko)
                    else p1_l2++;

                    if (p2Max < 'N') p2_l1++;
                    else p2_l2++;
                    p1CurrScore = GivePoints(p1Max, multiplier);
                    p2CurrScore = GivePoints(p2Max, multiplier);

               }

               else // A regular letter is drawn
               {
                    sbyte[] letterIndexOnP1Card = Search(player1, drawnToken);
                    if (letterIndexOnP1Card[0] == -1) { } // player does not have the letter on their card
                                                          // -> do nothing this round

                    else // player has this letter on their card
                    {
                         p1CurrScore = GivePoints(drawnToken, multiplier);
                         player1[letterIndexOnP1Card[0], letterIndexOnP1Card[1]] = '0';
                         if(drawnToken < 'N') p1_l1++;
                         else p1_l2++;
                    }

                    sbyte[] letterIndexOnP2Card = Search(player2, drawnToken);
                    if (letterIndexOnP2Card[0] == -1) { }
                    else
                    {
                         p2CurrScore = GivePoints(drawnToken, multiplier);
                         player2[letterIndexOnP2Card[0], letterIndexOnP2Card[1]] = '0';
                         if(drawnToken < 'N') p2_l1++;
                         else p2_l2++;
                    }
                    SetTargetToZeroOnMinMaxArray(drawnToken); ; // if the letter is an inital min-max,
                                                                // delete it from the min-max array

               }
               player1Score += p1CurrScore;
               player2Score += p2CurrScore;
               Console.SetCursorPosition(0, 3);
               Console.Write("                                                            "); ; //deletes, unimportant
               Console.SetCursorPosition(0, 3);
               if (p1CurrScore != 0)
                    Console.WriteLine($"Player 1 gained {p1CurrScore} points");
               if (p2CurrScore != 0)
                    Console.WriteLine($"Player 2 gained {p2CurrScore} points");

               if(p1l1extra && p1l1_min_and_max[0].Equals('0') && p1l1_min_and_max[1].Equals('0'))
               {
                    player1Score += 100;
                    Console.WriteLine("Player 1 gained 100 points");
                    p1l1extra = false;
               }
               else if(p1l2_min_and_max[0].Equals('0') && p1l2_min_and_max[1].Equals('0') && p1l2extra)
               {
                    player1Score += 100;
                    Console.WriteLine("Player 1 gained 100 points");
                    p1l2extra = false;
               }
               if(p2l1extra && p2l1_min_and_max[0].Equals('0') && p2l1_min_and_max[1].Equals('0'))
               {
                    player2Score += 100;
                    Console.WriteLine("Player 2 gained 100 points");
                    p2l1extra = false;
               }
               else if(p2l2extra && p2l2_min_and_max[0].Equals('0') && p2l2_min_and_max[1].Equals('0'))
               {
                    player2Score += 100;
                    Console.WriteLine("Player 2 gained 100 points");
                    p2l2extra = false;
               }

               if (!firstChinko && (p1_l1 == 4 || p1_l2 == 4 || p2_l2 == 4 || p2_l1 == 4) )
               {
                    Console.WriteLine();
                    if ((p1_l1 == 4 || p1_l2 == 4) && (p2_l2 == 4 || p2_l1 == 4))
                    {
                         Console.Write("Tie on First Çinko. Nobody wins the prize");
                    }
                    else if (p1_l1 == 4 || p1_l2 == 4) 
                    {
                         Console.Write("First Çinko - Player1 wins the prize and gains 150 pts");
                         player1Score += 150;
                    }
                    else
                    {
                         Console.Write("First Çinko - Player2 wins the prize and gains 150 pts");
                         player2Score += 150;
                    }
                    firstChinko = true;
                    Console.WriteLine();
               }
               
               

               Console.SetCursorPosition(27, 0);
               Console.Write($"Player 1 score: {player1Score}");
               Console.SetCursorPosition(27, 1);
               Console.Write($"Player 2 score: {player2Score}");
               Console.SetCursorPosition(0,7);
               
               
               
               
               

               Console.ReadLine();

               multiplier--;

          }
          Console.Clear();
          Console.SetCursorPosition(0,0);
          if(p1_l1 + p1_l2 == 8 && p2_l1 + p2_l2 == 8)
          {
               if(player1Score > player2Score)
                    Console.WriteLine("Player1 has won by points");
               else if (player1Score < player2Score)
                    Console.WriteLine("Player2 has won by points");
               else Console.WriteLine("Tie!!");
          }
          else if (p1_l1 + p1_l2 == 8)
               Console.WriteLine("Player1 won by TOMBALA");
          else Console.WriteLine("Player2 won by TOMBALA");

          Console.WriteLine($"The game is over after {30 - multiplier} steps.");
          Console.WriteLine();
          Console.Write("Good Bye!");
          Console.ReadLine();

          
     }
}
