using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Query
{
    public class ExpensesView
    {
        public int Id { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public int PaidBy { get; set; }
        public string Comment { get; set; }
        public int[] DivideAmong { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public string[] DivideAmongNames { get; set; }
    }
}
