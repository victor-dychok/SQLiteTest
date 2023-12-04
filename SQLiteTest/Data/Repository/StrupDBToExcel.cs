using Aspose.Cells;
using SQLiteTest.Data.Interfaces;
using SQLiteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.Data.Repository
{
    public class StrupDBToExcel : IDataToExcel
    {
        private Workbook _workbook = new Workbook();
        private List<DBStrupResult> _soutceList;
        public StrupDBToExcel(List<DBStrupResult> sourceList)
        {
            _soutceList = sourceList;
        }
        public void SaveToExcel(string pass)
        {
            Worksheet worksheet = _workbook.Worksheets[0];

            worksheet.Cells.ImportCustomObjects(_soutceList,
            new string[] { "NumberOfCorrectItem",
                            "NumberOfMisstake",
                            "MinTimePt1",
                            "MinTimePt2",
                            "MaxTimePt1",
                            "MaxTimePt2",
                            "AverageTimePt1",
                            "AverageTimePt2",
                            "DelayTime"},
            true,
            0,
            0,
            _soutceList.Count,
            true,
            null,
            false);
            _workbook.Save(pass);
        }
    }
}
