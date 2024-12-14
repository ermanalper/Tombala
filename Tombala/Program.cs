﻿class Tombala
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

     static sbyte Search(char[] card, char target)      // Note: Since the array is only 8 elements long,                              
     {                                                  //       lineer search has a better time complexity
          for (sbyte i = 0; i < 8; i++)                  //       than binary search, which sorts the array
               if (card[i].Equals(target)) return i;    //       before searching.
          return -1;                                    //       Lineer Search: O(n)   --> 8 ops. max
     }                                                  //       Binary Search: O(n log(n)) + O(log(n))  --> 27 ops max
                                                        //       RETURNS THE INDEX OF THE TARGET, -1 IF IT DOES NOT EXIST
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
          Console.SetCursorPosition(0, 4);
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
     }

     static void Main()
     {
          //   vvv VARIABLES vvv

          int p1_l1 = 0, p1_l2 = 0, p2_l1 = 0, p2_l2 = 0; // Player1 Line1 .. etc.
                                                          //    p1_l1 == 4 : Çinko
                                                          //    p1_l1 + p1_l2 == 8 : Tombala

          int player1Score = 0, player2Score = 0;
          int multiplier = 30;

          p1l1_min_and_max = FindMinAndMaxLetter(player1, 0); // These hold the "min and max" 
          p1l2_min_and_max = FindMinAndMaxLetter(player1, 1); // letters on each row
          p2l1_min_and_max = FindMinAndMaxLetter(player2, 0); // [minLetter, maxLetter]
          p2l2_min_and_max = FindMinAndMaxLetter(player2, 1); // The function is only used at the beginning.
                                                             // When the min or max value is drawn from the bag,
                                                             // it is changed to '0' in these arrays. Since we only find 
                                                             // min and max once, changing it to '0' will not cause any problem
                                                             // with having the lowest ASCII code
          int currScore; // vowel: 3 ; constanst: 2



          //   ^^^ VARIABLES ^^^

          player1 = Initialize();
          player2 = Initialize(); // 8 letters on both arrays that satisfy the conditions
          Display();

          while (p1_l1 + p1_l2 < 8 && p2_l1 + p2_l2 < 8
                              && remaining_letters > 0) // Keep drawing new letter from the bag
                                                        // until someone makes "Tombala" or the bag is empty
          {
               Console.Write("Press enter to draw a new letter");
               Console.ReadLine();
               char drawnToken = DrawLetterFromBag();

               if (drawnToken.Equals('1')) // Joker is drawn, each player deletes the max letter on their cards
               {
                    char p1Max = SetMaxIndexToZero(ref player1); // Remove max letter from the players' card
                    char p2Max = SetMaxIndexToZero(ref player2); // and returns the letter (it is 
                                                                 // neccessary in order to remove it
                                                                 // from the "min-max" array)
                    SetTargetToZeroOnMinMaxArray(p1Max, ref p1l1_min_and_max, ref p1l2_min_and_max); // Removes it from the 
                    SetTargetToZeroOnMinMaxArray(p2Max, ref p2l1_min_and_max, ref p2l2_min_and_max);// "min-max" array (if exists)

                    if (p1Max < 'N') p1_l1++;  // counts removed letters on each row (4 = çinko)
                    else p2_l2++;

                    if (p2Max < 'N') p2_l1++;
                    else p2_l2++;
               }

               else // A regular letter is drawn
               {

               }



          }
     }
}
/* temp: if (drawnToken.Equals('A') || drawnToken.Equals('E') || drawnToken.Equals('I') ||
                    drawnToken.Equals('O') || drawnToken.Equals('U'))   */