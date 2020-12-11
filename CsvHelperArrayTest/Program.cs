using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelperArrayTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelperArrayTest
{
    public class NANListNullableSingleConverter : IEnumerableConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value is List<float?> listFloats)
            {
                if (value == null)
                {
                    return base.ConvertToString(value, row, memberMapData);
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listFloats.Count; i++)
                {
                    var nf = listFloats[i];
                    if (nf.HasValue)
                    {
                        sb.Append(nf.Value);
                    }
                    if (i < listFloats.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                var sbR = sb.ToString();
                return sbR;
            }
            else
            {
                return base.ConvertToString(value, row, memberMapData);
            }
        }
    }
    
    public class NANSingleConverter : CsvHelper.TypeConversion.SingleConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            //if (value == null)
            //{
            //    return "";
            //}
            return base.ConvertToString(value, row, memberMapData);
        }

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string trimmed = text.Trim();
            if (trimmed == string.Empty || trimmed.ToUpper() == "NAN")
            {
                return null;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Apple> apple_records;

            //using (var reader = new StreamReader(@"apple.csv"))
            using (var reader = new StreamReader(@"apple-dup.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.TypeConverterCache.AddConverter<float?>(new NANSingleConverter());
                csv.Configuration.RegisterClassMap<AppleMap>();
                apple_records = csv.GetRecords<Apple>().ToList();
                Console.WriteLine(apple_records.Count);
            }

            using (var writer = new StreamWriter(@"apple-dup.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.TypeConverterCache.AddConverter<float?>(new NANSingleConverter());
                // csv.Configuration.TypeConverterCache.AddConverter<float?>(new NANSingleConverter());
                csv.Configuration.TypeConverterCache.AddConverter<List<float?>>(new NANListNullableSingleConverter());
                csv.Configuration.TypeConverterOptionsCache.AddOptions<List<float?>>(new TypeConverterOptions 
                {
                    
                });

                csv.Configuration.RegisterClassMap<AppleMap>();
                csv.Configuration.HasHeaderRecord = false;

                // 讓特定欄位 有 " "
                csv.Configuration.ShouldQuote = (field, context) =>
                {
                    return false;
                    //return
                    //    context.Record.Count == 0 &&
                    //    context.HasHeaderBeenWritten;
                };

                csv.WriteConvertedField("Name");
                csv.WriteConvertedField("Price");
                
                for (int i = 1; i <= 3; i++)
                {
                    string id = i.ToString("d2");
                    csv.WriteConvertedField($"Note{id}");
                }
                for (int i = 1; i <= 2; i++)
                {
                    string id = i.ToString("d2");
                    csv.WriteConvertedField($"Pad{id}");
                }
                csv.Context.HasHeaderBeenWritten = true;
                csv.NextRecord();

                csv.WriteRecords<Apple>(apple_records);
            }

        }
    }
}
