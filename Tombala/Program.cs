class Tombala
{
     static char[] bag = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', //M is index 12
                         'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1'}; //Z is index 25
                              // '0' means the letter is removed, '1' is joker
     static int remaining_letters = 27; // 27 remaining letters (including joker) in the bag

     static char[] player1 = new char[8]; // default (when a letter is removed) = 0
     static char[] player2 = new char[8];
     static Random random = new Random();

     static char[] Initialize() // use only once at the beginning of the game
     {
          char[] temp_alph = bag;
          char[] init = new char[8];
          for (byte i = 0; i < 4; i++) // letters from A to M , upper line
          {
               int upperline_letter = random.Next(13); // returns an index up to 12 (M)
               if (temp_alph[upperline_letter].Equals('0')) // the random index is an already drawn letter 
               {
                    i--;
                    continue;
               }
               init[i] = temp_alph[upperline_letter];
               temp_alph[upperline_letter] = '0';
          }
          for (byte i = 4; i < 8; i++) // letters from N to Z, bottom line
          {
               int bottomline_letter = random.Next(13, 26); // random between index 13 (N) and index 25 (Z)
               if (temp_alph[bottomline_letter].Equals('0'))
               {
                    i--;
                    continue;
               }
               init[i] = temp_alph[bottomline_letter];
               temp_alph[bottomline_letter] = '0';
          }
          return init;
     }

     static bool Search(char[] card, char target)      // Note: Since the array is only 8 elements long,                              
     {                                                 //       lineer search has a better time complexity
          foreach(char c in card)                      //       than binary search, which sorts the array
               if (c.Equals(target)) return true;      //       before searching.
          return false;                                //       Lineer Search: O(n)   --> 8 ops. max
     }                                                 //       Binary Search: O(n log(n)) + O(log(n))  --> 27 ops max

     static char DrawLetterFromBag() // draws a letter from the bag, might be Joker ('1')
     {
          bool newToken = false;
          int tokenIndex;
          do{
               tokenIndex = random.Next(27); // returns an index up to 26 [26th index is Joker('1')]
               if (!bag[tokenIndex].Equals('0')) newToken = true;
          }while(!newToken);
          char token = bag[tokenIndex];
          bag[tokenIndex] = '0';
          remaining_letters--;
          return token;
     }
     static void Display()
     {
          Console.Clear();
          for(byte c = 0; c < 4; c++)
          {
               if (player1[c].Equals('0')) Console.Write(" ");
               else Console.Write(player1[c] + " ");
          }
          Console.WriteLine();
          for(byte c = 4; c < 8; c++)
          {
               if (player1[c].Equals('0')) Console.Write(" ");
               else Console.Write(player1[c] + " ");
          }
          Console.SetCursorPosition(14, 0);
          for(byte c = 0; c < 4; c++)
          {
               if (player2[c].Equals('0')) Console.Write(" ");
               else Console.Write(player2[c] + " ");
          }
          Console.SetCursorPosition(14, 1);
          for(byte c = 4; c < 8; c++)
          {
               if (player2[c].Equals('0')) Console.Write(" ");
               else Console.Write(player2[c] + " ");
          }
     }

     static char[] FindMinAndMaxLetter(char[]card, byte lower, byte upper)
     {
          char maxChar = card[lower];                                 // Returns [min, max].
          char minChar = card[lower];                                 // Run for EACH ROW.
          for(byte i = Convert.ToByte(lower + 1) ; i < upper; i++)    // For upper row: lower = 0, upper = 4
          {                                                           // Bottom row: lower = 4, upper = 8
               if(card[i] < minChar) minChar = card[i];
               else if(card[i] > maxChar) maxChar = card[i];
          }
          return [minChar, maxChar];
     }




     static void Main()
     {
          //   vvv VARIABLES vvv

          int p1_l1 = 0, p1_l2 = 0, p2_l1 = 0, p2_l2 = 0; // Player1 Line1 .. etc.
                                                       //    p1_l1 == 4 : Çinko
                                                       //    p1_l1 + p1_l2 == 8 : Tombala

          int player1Score = 0, player2Score = 0;
          int multiplier = 30;

          char[] p1l1_min_and_max = FindMinAndMaxLetter(player1, 0, 4); // These hold the "min and max" 
          char[] p1l2_min_and_max = FindMinAndMaxLetter(player1, 4, 8); // letters on each row
          char[] p2l1_min_and_max = FindMinAndMaxLetter(player2, 0, 4); // [minLetter, maxLetter]
          char[] p2l2_min_and_max = FindMinAndMaxLetter(player2, 4, 8); // The function is only used at the beginning.
                                                       // When the min or max value is drawn from the bag,
                                                       // it is changed to '0' in these arrays. Since we only find 
                                                       // min and max once, changing it to '0' will not cause any problem
                                                       // with having the lowest ASCII code




          //   ^^^ VARIABLES ^^^

          player1 = Initialize(); 
          player2 = Initialize(); // 8 letters on both arrays that satisfy the conditions
          Display();

          while(p1_l1 + p1_l2 < 8 && p2_l1 + p2_l2 < 8
                              && remaining_letters > 0) // Keep drawing new letter from the bag
                                                       // until someone makes "Tombala" or the bag is empty
          {

          }
     }
}