using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace Task2_Cloud.Models
{
    public class Prod
    {
        public string prodName { get; set; }
        public string prodDesc { get; set; }
        public double prodPrice { get; set; }
        public string FilePath { get; set; }
        public string PartitionKey { get; internal set; }
        public string RowKey { get; internal set; }
    }
}