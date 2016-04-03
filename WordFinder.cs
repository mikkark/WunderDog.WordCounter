using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WunderDog.WordFinder
{
    public class WordFinder
    {
        private readonly List<string> _possibleWords;
        private readonly List<List<char>> _lettersPlanes;
        private readonly bool _parallel;
        private Dictionary<char, List<CustomPoint>> _letterPositions;

        public WordFinder(List<string> possibleWords, List<List<char>> lettersPlanes, bool parallel)
        {
            _possibleWords = possibleWords;
            _lettersPlanes = lettersPlanes;
            _parallel = parallel;

            FoundWords = new ConcurrentBag<string>();
        }

        public ConcurrentBag<string> FoundWords { get; set; }

        public int FoundWordsCount => FoundWords.Count();

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

            if (_parallel)
            {
                Parallel.ForEach(_possibleWords.Where(w => _letterPositions.ContainsKey(w[0])), FindWord);
            }
            else
            {
                foreach (var word in _possibleWords.Where(w => _letterPositions.ContainsKey(w[0])))
                {
                    FindWord(word);
                }
            }

        }

        private void FindWord(string word)
        {
            List<CustomPoint> firstLetters;

            if (_letterPositions.TryGetValue(word[0], out firstLetters))
            {
                foreach (var firstLetter in firstLetters)
                {
                    FollowChain(word, firstLetter, new List<CustomPoint>());
                }
            }
        }

        private void FollowChain(string word,
            CustomPoint currentLetter, List<CustomPoint> customPoints)
        {
            customPoints.Add(currentLetter);

            if (customPoints.Count == word.Length)
            {
                FoundWords.Add(word);

                Debug.WriteLine("found word " + word + " in chain:");
                foreach (var link in customPoints)
                {
                    Debug.WriteLine(link.ToString());
                }

                return;
            }

            List<CustomPoint> positions;
            if (_letterPositions.TryGetValue(word[customPoints.Count], out positions) == false)
            {
                Debug.WriteLine("word not found " + word + " in chain:");
                foreach (var link in customPoints)
                {
                    Debug.WriteLine(link.ToString());
                }

                return;
            };

            var neigbouringLetters = positions.Where(pos => pos.IsNeighbourOf(currentLetter) &&
                                                                 customPoints.Contains(pos) == false);

            foreach (var neighbour in neigbouringLetters)
            {
                var newChain = new List<CustomPoint>();
                newChain.AddRange(customPoints);

                FollowChain(word, neighbour, newChain);
            }
        }
    }
}