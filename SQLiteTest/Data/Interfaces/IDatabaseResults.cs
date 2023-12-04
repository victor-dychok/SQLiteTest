using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.Interfaces
{
    public interface IDatabaseResults
    {
        void SaveToDatabase();
        void AddStrupResult(DBStrupResult result);
        void RemoveStrupResult(DBStrupResult result);
        IEnumerable<DBStrupResult> GetAllStrupResults();
        void AddMunsterbergResult(DBMunsterbergResult result);
        void RemoveMunsterbergResult(DBMunsterbergResult result);
        IEnumerable<DBMunsterbergResult> GetAllMunsterbergResults();
        void AddShulteResult(DBShulteResult result);
        void RemoveShulteResult(DBShulteResult result);
        IEnumerable<DBShulteResult> GetAllShulteResults();

        User GetUser(User user);
        IEnumerable<User> GetAllUsers();
    }
}
