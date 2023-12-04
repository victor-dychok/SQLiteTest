using SQLiteTest.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.TestModels
{
    internal class MunsterbergModel
    {
        private const int NumberOfCollumns = 35;

        private List<string> _answerList;

        private int _numberOfWords;
        private List<string> _words;
        private char[] _symbols;
        private Random _random;
        IMunsterbergWords _wordsRepository;


        public MunsterbergModel(int numberOfWords, int numberOfSymbols, IMunsterbergWords munsterbergWords)
        {
            _numberOfSymbols = numberOfSymbols;
            _numberOfWords = numberOfWords;
            _random = new Random();
            _answerList = new List<string>();
            _wordsRepository = munsterbergWords;

            GeneratedSymbolsArray();
        }

        public int NumberOfWords { get; set; }

        private int _numberOfSymbols;

        public int NumberOfSymbols { get; set; }

        public List<string> GetTestAnswer()
        {
            return _answerList;
        }

        public int GetCollumsNumber()
        {
            return NumberOfCollumns;
        }

        public int GetRowNumber()
        {
            return _numberOfSymbols / NumberOfCollumns + 1;
        }

        public char[] GetSymbolsArray()
        {
            return _symbols;
        }

        private void GeneratedSymbolsArray()
        {
            SetWords(_wordsRepository.GetWords());
            _symbols = new char[_numberOfSymbols];
            int dif = _numberOfSymbols / (_numberOfWords + 1);
            int minNextWordIndex = 0;
            int maxNextWordIndex = dif;
            int wordIndex = 0;

            int nextWordIndex = _random.Next(minNextWordIndex, maxNextWordIndex);
            for (int i = 0; i < _numberOfSymbols; ++i)
            {
                _symbols[i] = (char)_random.Next('а', 'я');
            }

            for (int i = 0; i < _numberOfSymbols; i++)
            {
                if (i == nextWordIndex
                    && i + _words[wordIndex].Length < _numberOfSymbols
                    && wordIndex < _numberOfWords)
                {
                    for (int j = 0; j < _words[wordIndex].Length && i < _numberOfSymbols; i++, j++)
                    {
                        _symbols[i] = _words[wordIndex][j];
                    }
                    _answerList.Add(_words[wordIndex]);
                    minNextWordIndex = i + 1;
                    maxNextWordIndex += dif;
                    ++wordIndex;
                    nextWordIndex = _random.Next(minNextWordIndex, maxNextWordIndex);
                }
            }
        }

        public void SetWords(List<string> words)
        {
            _words = words;
        }
    }
}