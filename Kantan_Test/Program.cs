using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace Kantan_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string currDir = Path.GetDirectoryName(System.Environment.CurrentDirectory);
            string outputFile = currDir + @"/consumption-output.csv";
            string inputFile;

            //Get filepath of input data file
            try
            {
                inputFile = args[0];
            }
            catch
            {
                Console.WriteLine("Please pass the filepath of the input file as an argument");
                return;
            }

            List<Consumption> monthlyData = new List<Consumption>();
            Consumption prevRow = null;
            int prevTotal = 0;

            try
            {
                Console.WriteLine("Attempting to read from file...");
                //Read data from file, aggregate monthly in-place into List<Consumption>
                using (var reader = new StreamReader(inputFile))
                {
                    reader.ReadLine(); //Skip header

                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        var columns = row.Split(',');

                        Consumption currRow = new Consumption(columns[0], columns[1]);
                        prevRow ??= currRow;
                        
                        //Check for a change of row
                        if (prevRow.month != currRow.month)
                        {
                            int newPrevTotal = prevRow.value;
                            prevRow.value -= prevTotal;
                            monthlyData.Add(prevRow);
                            prevTotal = newPrevTotal;
                        }

                        prevRow = currRow;
                    }

                    prevRow.value -= prevTotal;
                    monthlyData.Add(prevRow);
                    Console.WriteLine("File read & monthly aggregation complete");
                }
            }
            catch
            {
                Console.WriteLine("There was an error with the input file argument. Please check the filepath and try again");
                return;
            }


            Console.WriteLine("Writing to output file...");

            //Write date from List<Consumption> into ouput csv
            using (var file = File.CreateText(outputFile))
            {
                file.WriteLine("Month Year,Consumption");
                
                foreach (Consumption row in monthlyData)
                {
                    file.WriteLine(row.month + "," + row.value);
                }
            }

            Console.WriteLine("Operation Complete!");
            Console.WriteLine("Output file: " + outputFile);
        }
    }

}
