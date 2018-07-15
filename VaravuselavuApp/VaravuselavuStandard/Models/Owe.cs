using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Models
{
    [Table("owe")]
    public class Owe
    {
        public int OweId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int ExpensesId { get; set; } 
    }
}
