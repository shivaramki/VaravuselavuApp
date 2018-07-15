using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Models
{
    [Table("expenses")]
    public class Expenses
    {
        public int Id { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public int AddedBy { get; set; }
        public int PaidBy { get; set; }
        public string Comment { get; set; }
        public int[] DivideAmong { get; set; }
        public decimal Amount { get; set; }
        public int Category { get; set; }
    }
}
