using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaravuselavuStandard.Models
{
    [Table("category")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
