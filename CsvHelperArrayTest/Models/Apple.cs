using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelperArrayTest.Models
{
    public class Apple
    {
        public string Name { get; set; }
        public int Price { get; set; }


        [Ignore] public List<float?> Notes { get; set; }
        [Ignore] public List<string> Pads { get; set; }

    }
}
