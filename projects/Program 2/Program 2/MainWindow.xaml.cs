using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BabbleSample
{
    /// Babble framework
    /// Starter code for CS212 Babble assignment
    /// 
    /// Used 1st order statistics
    public partial class MainWindow : Window
    {
        private string input;               // input file
        public string[] words;             // input file broken into array of words
        private int wordCount = 200;        // number of words to babble
        public int number_of_words;
        public int number_of_keys;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            number_of_words = 0; number_of_keys = 0;  // sets the number of words and keys to 0 each time

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Sample"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                
              
                input = System.IO.File.ReadAllText(ofd.FileName);  // read file
                words = Regex.Split(input, @"\s+");       // split into array of words
                textBlock1.Text = "Loading file " + ofd.FileName;

            }
        }

        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + order);
                textBlock1.Text = "";
                
                Dictionary<string, ArrayList> hashTable = makeHashtable(); // when the order is chosen, then the hash table is made
                
                foreach(KeyValuePair<string, ArrayList> entry in hashTable) // prints out the different keys in the hash table and each corresponding word that goes with it.
                {
                    textBlock1.Text += entry.Key + "->"; // prints out the keys
                    foreach (string word in entry.Value) // for each element in the array of strings, the element/word is printed
                        textBlock1.Text += word + " ";

                    textBlock1.Text += "\n";
                    number_of_keys++;  // each time it gets an entry from the hash table, the number of keys goes up by one
                }

                number_of_words = words.Length;  // makes the number of words equal to the length of the array of words


                // outputs a message saying the number of total words and keys found.
                MessageBox.Show("Total number of words: " + number_of_words + "\n" + "Number of keys: " + number_of_keys); 

            }
        }

        // This function performs an action when the babble button is clicked. It outputs text using the dump function.
        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, ArrayList> hashTable = makeHashtable(); // makes a hashtable so the dump funciton can call it.

            for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                textBlock1.Text += " " + words[i];

                dump(hashTable); // executes the dump function, which outputs the lines of babble text
        }


        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);
        }

        // This function makes a hashtable based on the string "words", made from the words taken from the text file. 
        Dictionary<string, ArrayList> makeHashtable()
        {
            Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();

            for (int i = 0; i <= words.Length - 1; i++)
            {
                string starting_word = words[i];
                string second_word = "";

                if (i+1 > words.Length-1) // goes back to the beginning if the second word is out of the bounds in the list of words
                {
                    second_word = words[0];
                }
                else // if not, it sets up the second word
                {
                    second_word = words[i + 1];
                }

                if (!hashTable.ContainsKey(starting_word)) // if the hashtable doesn't contain a key, it adds it to the hashtable as a key
                    hashTable.Add(starting_word, new ArrayList());
                
                hashTable[starting_word].Add(second_word); // if the hashtable contains a key, it adds the word after the current word to an array corresponding to the key
          
            }
            return hashTable;
        }

        // This function outputs a randomized set of 200 words based on the hashtable. 
        void dump(Dictionary<string, ArrayList> hashTable)
        {

            textBlock1.Text = ""; // clears the text block
            
            string current_word = words[0]; // the current word starts on the first word of the file
            
            textBlock1.Text += current_word + " ";  // outputs the first word of the file 

            for (int i = 0; i < 199; i++)  // loops 199 times, outputting a total of 200 words by the end of the function. 
            {
                if (hashTable.ContainsKey(current_word))
                {
                        ArrayList myArrayList = (ArrayList)hashTable[current_word];  // makes an array from the current key

                        Random rnd = new Random();  // picks a random number
                        int rand_number = rnd.Next(0, myArrayList.Count);
                        
                        
                        current_word = myArrayList[rand_number].ToString();  // makes the next current word a random word in that array 
                        textBlock1.Text += current_word + " ";  // outputs the current word

                }
            }   
        }
    }
}

