using Cybtans.Automation;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Automation.Components
{
    public class Table<THeader, TCell> : TestComponent, IEnumerable<TableRow<TCell>>
    {
        [TestElement("thead > tr > th")]
        public List<THeader> Headers { get; set; }

        [TestElement("tbody > tr")]
        public List<TableRow<TCell>> Rows { get; set; }

        public TCell this[int row, int column]
        {
            get
            {
                return Rows[row][column];
            }            
        }

        public int RowsCount => Rows?.Count ?? 0;

        public int ColumnsCount => Headers?.Count ?? 0;

        public bool IsEmpty => Rows == null || Rows.Count == 0;

        public IEnumerator<TableRow<TCell>> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.GetEnumerator();
        }
    }

    public class TableRow<TCell> : TestComponent, IEnumerable<TCell>
    {
        [TestElement("td")]
        public List<TCell> Cells { get; set; }

        public TCell this[int idx] => Cells[idx];

        public int Count => Cells?.Count ?? 0;

        IEnumerator<TCell> IEnumerable<TCell>.GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Cells.GetEnumerator();
        }      
    }

    public class BasicTable : Table<IWebElement, IWebElement>
    {

    }
}
