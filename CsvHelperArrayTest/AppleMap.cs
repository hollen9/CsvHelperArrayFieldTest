using CsvHelper.Configuration;
using CsvHelperArrayTest.Models;
using System.Collections.Generic;
using System.Globalization;

namespace CsvHelperArrayTest
{
    internal class AppleMap : ClassMap<Apple>
    {
        public AppleMap()
        {
            // Map(m => m.Name).Name("Name");
            // Map(m => m.Price).Name("Price");


            AutoMap(CultureInfo.InvariantCulture);

            Map(m => m.Notes).ConvertUsing(row =>
            {
                var colIdxices = new List<float?>();

                for (int i = 0; i < 3; i++)
                {
                    colIdxices.Add(row.GetField<float?>("Note" + (i + 1).ToString("d2"))/* ?? -99999*/);
                }

                return colIdxices;
                // return new List<string> { row.GetField<string>(1), row.GetField<string>(2), row.GetField<string>(3) };
            });

            Map(m => m.Pads).ConvertUsing(row =>
            {
                var colIdxices = new List<string>();

                for (int i = 0; i < 2; i++)
                {
                    colIdxices.Add(row.GetField<string>("Pad" + (i + 1).ToString("d2"))/* ?? -99999*/);
                }

                return colIdxices;
                // return new List<string> { row.GetField<string>(1), row.GetField<string>(2), row.GetField<string>(3) };
            });



        }
    }
}