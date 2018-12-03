﻿//-----------------------------------------------------------
//File:   HighScoresManager.cs
//Desc:   Creates a static list of highscores and holds methods
//        to write them to a file and read them back into the list
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Royale_Platformer.Model.HighScores
{
    // Manages a list of highscores and contains methods to manipulate it
    public class HighScoresManager
    {
        private static List<HighScore> highScores = new List<HighScore>(); // stores a list HighScore objects
        public HighScoresManager()
        {
        }

        // Checks to see if score is a high score <score>
        // Returns true if score is a highscore and false if not a highscore
        public static bool CheckScore(int score)
        {
            highScores = ReadScores(); // read scores into list from file so new score can be compared

            if (highScores.Count == 0) // score is a highscore if it's the first score
                return true;
            else if (highScores.Count > 0 && highScores.Count < 10) // score is a highscore if there is not already ten highscores in the list
                return true;
            else // compare player's score with other scores to determine if it high enough to be a highscore
            {
                foreach (HighScore item in highScores)
                {
                    if (score > item.GetScore())
                        return true;
                }
            }
            return false; // if score does not meet previous criteria, it is not a highscore
        }

        // Adds a player's name <name> and score <score> to the highScores list from greatest to least 
        public static void AddHighScore(string playerName, int playerScore)
        {
            highScores.Add(new HighScore(playerName, playerScore)); // add name and score to list as a HighScore object

            // OrderBy function found at "https://stackoverflow.com/questions/16620135/sort-a-list-of-objects-by-the-value-of-a-property/16620159"
            highScores = highScores.OrderByDescending(x => x.GetScore()).ToList(); // order the list from greatest ot least

            if (highScores.Count > 10) // remove lowest score from the list so list only holds 10 objects
            {
                highScores.RemoveRange(10, highScores.Count - 10);
            }
            WriteScores(); // write scores to file for future reference
        }

        // Writes the highScores list to a file
        public static void WriteScores()
        {
            string PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // find path at which to create scores.txt

            string infoList = "";
            foreach (HighScore score in highScores) // prepare string with highscores info 
            {
                infoList += $"{score.GetName()},{score.GetScore()}\r\n";
            }
            File.WriteAllText(Path.Combine(PATH, "scores.txt"), infoList); // write info to file with path PATH
        }

        // Reads names and scores from a file and returns a list
        public static List<HighScore> ReadScores()
        {
            List<HighScore> scores = new List<HighScore>(); // create list to hold highscore info

            string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "scores.txt"); // find path at which to find scores.txt

            if (File.Exists(PATH)) // checks if PATH exists
            {
                foreach (var score in File.ReadLines(PATH)) // iterate through lines in scores.txt
                {
                    string[] items = score.Split(','); // split each line into a name and score and store in temporary array
                    scores.Add(new HighScore(items[0], Convert.ToInt32(items[1]))); // add highscore info for each player to scores list
                }
            }
            return scores; // return completed list of HighScore objects
        }

        // Returns names of players held in the list instance variable
        public static List<HighScore> GetHighScores()
        {
            return highScores;
        }

        // Provides ability to clear highScores list for testing purposes
        public static void ClearHighScores()
        {
            highScores.Clear();
        }
    }
}


