using System;
using System.Collections.Generic;
using System.Linq;

namespace WunderDog.WordFinder
{
    public class WordFinder
    {
        private readonly List<string> _possibleWords;
        private readonly List<List<char>> _lettersPlanes;
        private Dictionary<char, List<CustomPoint>> _letterPositions;

        public WordFinder(List<string> possibleWords, List<List<char>> lettersPlanes)
        {
            _possibleWords = possibleWords;
            _lettersPlanes = lettersPlanes;

            FoundWords = new List<string>();
        }

        public IList<string> FoundWords { get; set; }

        public int FoundWordsCount => FoundWords.Distinct().Count();

        private void Initialize()
        {
            _letterPositions = new Dictionary<char, List<CustomPoint>>();

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

                    char c = Convert.ToChar(lettersPlane[i].ToString().ToLower());

                    CustomPoint point = new CustomPoint(x, y, z);

                    if (!_letterPositions.ContainsKey(c))
                    {
                        _letterPositions.Add(c, new List<CustomPoint>() { point });
                    }
                    else
                    {
                        _letterPositions[c].Add(point);
                    }
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
            List<CustomPoint> firstLetters;
            
            if (_letterPositions.TryGetValue(word[0], out firstLetters))
            {
                foreach (var firstLetter in firstLetters)
                {
                    var chain = FindNeighbouringLetter(word, firstLetter,
                        new List<CustomPoint>() { firstLetter });

                    if (chain.Count == word.Length)
                    {
                        FoundWords.Add(word);
                    }
                }
            }
        }

        private IList<CustomPoint> FindNeighbouringLetter(string word,
            CustomPoint currentLetter, List<CustomPoint> customPoints)
        {
            List<CustomPoint> positions;
            if (_letterPositions.TryGetValue(word[customPoints.Count], out positions) == false)
            {
                return customPoints;
            };

            var neigbouringLetters = positions.Where(pos => pos.IsNeighbourOf(currentLetter) &&
                                                                 customPoints.Contains(pos) == false);

            foreach (var neighbour in neigbouringLetters)
            {
                var newChain = new List<CustomPoint>();
                newChain.AddRange(customPoints);
                newChain.Add(neighbour);

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