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
    public class MunsterbergDBToExcel : IDataToExcel
    {
        private Workbook _workbook = new Workbook();
        private List<DBMunsterbergResult> _soutceList;
        public MunsterbergDBToExcel(List<DBMunsterbergResult> sourceList)
        {
            _soutceList = sourceList;
        }
        public void SaveToExcel(string pass)
        {
            Worksheet worksheet = _workbook.Worksheets[0];

            worksheet.Cells.ImportCustomObjects(_soutceList,
            new string[] { "NumderOfAllWords",
                            "NumberOfCorrectWords",
                            "NumberOfMisstakenWords",
                            "AllTime"
                         },
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
