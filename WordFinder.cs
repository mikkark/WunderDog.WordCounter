using System;
using System.Collections.Generic;
using System.Linq;

namespace WunderDog.WordFinder
{
    public class WordFinder
    {
        private readonly List<string> _possibleWords;
        private readonly List<List<char>> _lettersPlanes;
        private Dictionary<CustomPoint, char> _lettersIndexed;

        public WordFinder(List<string> possibleWords, List<List<char>> lettersPlanes)
        {
            _possibleWords = possibleWords;
            _lettersPlanes = lettersPlanes;

            FoundWords = new List<string>();
        }

        public IList<string> FoundWords { get; set; }

        public int FoundWordsCount => FoundWords.Count;

        private void Initialize()
        {
            _lettersIndexed = new Dictionary<CustomPoint, char>();

            for (int z = 0; z < _lettersPlanes.Count; z++)
            {
                var lettersPlane = _lettersPlanes[z];

                for (int i = 0, x = 0, y = -1; i < lettersPlane.Count; i++, x++)
                {
                    if (i % 4 == 0)
                    {
                        x = 0;
                        y++;
                    }

                    _lettersIndexed.Add(new CustomPoint(x, y, z), Convert.ToChar(lettersPlane[i].ToString().ToLower()));
                }
            }
        }

        public void FindWords()
        {
            Initialize();

            foreach (string word in _possibleWords)
            {
                FindWord(word);
            }
        }

        private void FindWord(string word)
        {
            var firstLetters = _lettersIndexed.Where(li => li.Value == word[0]);

            foreach (var firstLetter in firstLetters)
            {
                var chain = FindNeighbouringLetter(word, firstLetter, new List<CustomPoint>() { firstLetter.Key });

                if (chain.Count == word.Length)
                {
                    FoundWords.Add(word);

                   // Console.WriteLine("word found: " + word + ", with chain:");
                    foreach (var c in chain)
                    {
                     //   Console.WriteLine(c.ToString());
                    }
                }
                else
                {
                    //Console.WriteLine("word not found: " + word);
                }
            }
        }

        private IList<CustomPoint> FindNeighbouringLetter(string word,
            KeyValuePair<CustomPoint, char> currentLetter, List<CustomPoint> customPoints)
        {
            var neigbouringLetters = _lettersIndexed.Where(li => li.Value == word[customPoints.Count] &&
                                                                 li.Key.IsNeighbourOf(currentLetter.Key) &&
                                                                 customPoints.Contains(li.Key) == false);

            foreach (var neighbour in neigbouringLetters)
            {
                var newChain = new List<CustomPoint>();
                newChain.AddRange(customPoints);
                newChain.Add(neighbour.Key);

                if (newChain.Count == word.Length)
                {
                    return newChain;
                }
                else
                {
                    return FindNeighbouringLetter(word, neighbour, newChain);
                }
            }

            return customPoints;
        }
    }
}