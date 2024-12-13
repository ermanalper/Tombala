class Tombala
{
     static char[] bag = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', //M is index 12
                              'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1'}; //Z is index 25
                              // '0' means the letter is removed, '1' is joker

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

     static bool Search(char[] card, char target)
     {
          char[] sortedCard = (char[])card.Clone();
          Array.Sort(sortedCard);
          int lp = 0, rp = card.Length;
          while(lp < rp)
          {
               int mp = lp + (rp - lp) / 2;
               if (sortedCard[mp].Equals(target)) return true;
               else
               {
                    if (target < sortedCard[mp]) rp = mp - 1;
                    else lp = mp + 1;
               }
          }
          // if while loop ends, it means lp = rp.
          if (sortedCard[lp].Equals(target)) return true;
          return false;
     }

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


     static void Main()
     {
          player1 = Initialize(); 
          player2 = Initialize(); // 8 letters on both arrays that satisfy the conditions
          Display();
          
     }
}