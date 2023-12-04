using SQLiteTest.Data.Interfaces;
using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLiteTest.Data.Repository
{
    public class DBResultsRepository : IDatabaseResults
    {
        private DatabaseContext _context;
        public DBResultsRepository(DatabaseContext context) => _context = context;

        public void AddMunsterbergResult(DBMunsterbergResult result) => _context.MunsterbergResults.Add(result);

        public void AddShulteResult(DBShulteResult result) => _context.ShulteResults.Add(result);

        public void AddStrupResult(DBStrupResult result) => _context.StrupResults.Add(result);

        public User GetUser(User user)
        {
            if (user != null)
            {
                if (!_context.Users.Any(x => x.Name == user.Name && x.Group == user.Group))
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
                return _context.Users.First(i => i.Name == user.Name && i.Group == user.Group);
            }
            else return user;
        }
        public IEnumerable<DBMunsterbergResult> GetAllMunsterbergResults() => _context.MunsterbergResults;

        public IEnumerable<DBShulteResult> GetAllShulteResults() => _context.ShulteResults;

        public IEnumerable<DBStrupResult> GetAllStrupResults() => _context.StrupResults;

        public IEnumerable<User> GetAllUsers() => _context.Users;

        public void RemoveMunsterbergResult(DBMunsterbergResult result) => _context.MunsterbergResults.Remove(result);

        public void RemoveShulteResult(DBShulteResult result) => _context.ShulteResults.Remove(result);

        public void RemoveStrupResult(DBStrupResult result) => _context.StrupResults.Remove(result);

        public async void SaveToDatabase()
        {
            try
            {
                await Task.Run(() => _context.SaveChanges());
            }
            catch
            {
                MessageBox.Show("Ошибка сохранения в базу данных");
            }
        }
    }
}
