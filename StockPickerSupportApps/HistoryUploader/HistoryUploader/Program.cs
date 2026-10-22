//////////////////////////////////////////////////////////////////
// HistoryUploader Program
// Written by Matt Boreen, slightly modified by Paul Killian
// Description: This program requests data from Yahoo and imports relevant stock data
//              to the StockData database.
//////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Collections;
using System.Timers;
using System.IO;

namespace HistoryUploader
{
    internal class YahooStockQuoteGetterOfGoodness
    {
        private static string industryName = "";
        private static string connectionString = "";

        private static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                industryName = args[0];
            }

            CollectData();
        }

        public static void CollectData()
        {
            try
            {
                //Read connection string from file
                StreamReader sr;
                sr = File.OpenText("ConnectionString.txt");
                connectionString = sr.ReadLine();
                sr.Close();


                Hashtable ht = new Hashtable();

                //Open database connection.
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                StreamWriter sw = new StreamWriter(System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Year + ".txt");

                if (industryName == "")
                {
                    Console.Write("Enter the stock industry to get history data for: ");
                    industryName = Console.ReadLine();
                }

                Console.WriteLine("Adding " + industryName + " stock data to database...");

                //Grab all of the stocks that we're going to collect data on.
                SqlCommand selectStocksCommand = conn.CreateCommand();
                selectStocksCommand.CommandText = "select stock.id, stock.name, stock.symbol from stock " +
                    "inner join industry on stock.industry = industry.id where industry.name = '" + industryName + "'";
                SqlDataReader stocksReader = selectStocksCommand.ExecuteReader();
                string ticker = "";
                int stockID = 0;
                if (stocksReader.HasRows)
                {
                    try
                    {
                        while (stocksReader.Read())
                        {
                            ticker = stocksReader["Symbol"].ToString();
                            stockID = Convert.ToInt32(stocksReader["ID"]);
                            Console.WriteLine("Processing data for " + stocksReader["Name"].ToString() + ".");
                            sw.WriteLine("Processing data for " + stocksReader["Name"].ToString() + ".");

                            //Call Yahoo and get back our data.
                            string url = string.Format(" http://ichart.finance.yahoo.com/table.csv?s={0}&g=d", ticker);
                            WebClient web = new WebClient();
                            string data = web.DownloadString(url);
                            

                            //Parse the parameter values into a Hashtable
                            Hashtable hashTable = Parameters.Parse(data);
                           
                            for (int i = 0; i < hashTable.Count; i=i+7)
                            {
                                SqlCommand insertCommand = conn.CreateCommand();
                                insertCommand.CommandText = "if not exists (select * from historydata where stockid = '"
                                    + stockID + "' and [datetime] = '" + hashTable[i] + "') " +
                                    "insert into historydata (stockid, [datetime], [open], high, low, " +
                                "[close], volume, [adjusted close]) " +
                                "values ('" + stockID + "', '" + hashTable[i] + "', '" + hashTable[i+1] +
                                "', '" + hashTable[i+2] + "', '" + hashTable[i+3] + "', '" + hashTable[i+4] +
                                "', '" + hashTable[i+5] + "', '" + hashTable[i+6] + "')";

                                int numRows = insertCommand.ExecuteNonQuery();
                                if (numRows.Equals(1))
                                {
                                    sw.WriteLine("---Inserting '" + hashTable[i] + "' with 'open' = '"
                                        + hashTable[i+1] + ", 'high' = '" + hashTable[i+2] + ", 'low' = '"
                                        + hashTable[i+3] + ", 'close' = " + hashTable[i+4] + ", 'volume' = '"
                                        + hashTable[i+5] + ", 'adjusted close' = " + hashTable[i+6] + "'.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("!!!!ERROR - " + ticker + ": " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("No values were found in the database.");
                }
                stocksReader.Close();
                sw.Close();
                conn.Close();
                Console.WriteLine("Completed writing to text file.");
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }

    public class Parameters
    {
        public static Hashtable Parse(string data)
        {       
            Hashtable hTable = new Hashtable();
            data = data.Replace("Date,Open,High,Low,Close,Volume,Adj Close\n", "");
            data = data.Replace("\n", ",");
            string[] fields = data.Replace("\"", "").Split(',');
            for (int i = 0; i < fields.Length-1; i++)
            {
                hTable.Add(i, fields[i]);
            }
            return hTable;
        }
    }
}
