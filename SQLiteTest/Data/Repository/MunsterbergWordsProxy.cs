using SQLiteTest.Data.Interfaces;
using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.Repository
{
    public class MunsterbergWordsProxy : IMunsterbergWords
    {
        private DatabaseContext _context;
        private List<string> _words;
        public MunsterbergWordsProxy(DatabaseContext appContext)
        {
            _context = appContext;
            _words = new List<string>();
        }

        private List<string> setWords()
        {
            var list = new List<string>
            {
                "солнце",
                "район",
                "новость",
                "факт",
                "экзамен",
                "прокурор",
                "теория",
                "хоккей",
                "телевизор",
                "память",
                "восприятие",
                "любовь",
                "спектакль",
                "радость",
                "народ",
                "репортаж",
                "конкурс",
                "личность",
                "плавание",
                "комедия",
                "отчаяние",
                "лаборатория",
                "основание",
                "жизнь",
                "море",
                "путешествия",
                "велосипед",
                "утро",
                "кружка",
                "металл",
                "заяц",
                "папка"
            };

            foreach (var word in list)
            {
                _context.MunsterbergWords.Add(new MunsterbergWords { Word = word });
            }
            _context.SaveChanges();
            return list;
        }
        public List<string> GetWords()
        {
            var list = _context.MunsterbergWords.ToList();
            if (list.Count != 0)
            {
                foreach (MunsterbergWords word in list)
                {
                    _words.Add(word.Word);
                }
            }
            else
            {
                _words = setWords();
            }
            return _words;
        }
    }
}
