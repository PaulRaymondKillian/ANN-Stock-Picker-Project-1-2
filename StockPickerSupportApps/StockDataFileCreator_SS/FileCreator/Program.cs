/////////////////////////////////////////////////////////////////////////////
// FileCreator Program
// Written by Matt Boreen and Paul Killian
// Description: collect relevant data from StockData database, organize it,
//              and output as a RAW_DATA file and a Header file.
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Collections;
using System.Timers;
using System.IO;

namespace FileCreator
{
    internal class YahooStockQuoteGetterOfGoodness
    {
        private static string startDateString = "";
        private static string endDateString = "";
        private static DateTime endDate;
        private static string stockString = "";
        private static string filePathString = "";
        private static string connectionString = "";
        private static Hashtable htStocks = new Hashtable();
        private static int volumeNormalizer = 1;
        private static string[] parameterString;

        private static void Main(string[] args)
        {
            Console.BufferHeight = 10000;
            Console.BufferWidth = 130;
            Console.WindowHeight = 50;
            Console.WindowWidth = 130;
            Console.Title = "File Creator";

            //Read connection string from file
            StreamReader sr;
            sr = File.OpenText("ConnectionString.txt");
            connectionString = sr.ReadLine();
            sr.Close();
            if (args.Count() == 5)
            {
                //startDateString = args[0];
                //stockString = args[1];
                //htStocks = ReturnParmStocks(stockString);
                //filePathString = args[2];
                //volumeNormalizer = Convert.ToInt32(args[3]);
                //parameterString = args[4].Split(',');
                //Console.WriteLine("Start Date String: " + startDateString);
                //Console.WriteLine("Stocks: ");
                //for (int i = 0; i < htStocks.Count; i++)
                //    Console.WriteLine(" " + htStocks[i]);
                //Console.WriteLine("Output FilePath: " + filePathString);
                stockString = args[0];  //Comma separated stock string
                htStocks = ReturnParmStocks(stockString);
                filePathString = args[1];
                parameterString = args[2].Split(',');
                startDateString = args[3];
                endDateString = args[4];    //Should have verification to check to make sure endDateString is a valid date (not future)
                Console.WriteLine("Start Date String: " + startDateString);
                Console.WriteLine("Stocks: ");
                for (int i = 0; i < htStocks.Count; i++)
                    Console.WriteLine(" " + htStocks[i]);
                Console.WriteLine("Output FilePath: " + filePathString);
            }
            else if (args.Count() == 4)
            { 
                stockString = args[0];  //Comma separated stock string
                htStocks = ReturnParmStocks(stockString);
                filePathString = args[1];
                parameterString = args[2].Split(',');
                startDateString = args[3];
 
                Console.WriteLine("Start Date String: " + startDateString);
                Console.WriteLine("Stocks: ");
                for (int i = 0; i < htStocks.Count; i++)
                    Console.WriteLine(" " + htStocks[i]);
                Console.WriteLine("Output FilePath: " + filePathString);

            }
            else if (args.Count() != 0)
            {
                Console.WriteLine("Invalid Number of Input Parameters, ignoring them...");
            }

            CollectData();

        }

        public static void CollectData()
        {
            try
            {
                StockDataOrganizer.StockDataOrganizer StockDataOrg = new StockDataOrganizer.StockDataOrganizer();

                //Set Stock Data Output Flags
                for (int i = 0; i < parameterString.Count(); i++)
                {
                    if (parameterString[i] == "1")
                        StockDataOrg.outputOpen = true;

                    if (parameterString[i] == "2")
                        StockDataOrg.outputClose = true;

                    if (parameterString[i] == "3")
                        StockDataOrg.outputHigh = true;

                    if (parameterString[i] == "4")
                        StockDataOrg.outputLow = true;

                    if (parameterString[i] == "5")
                        StockDataOrg.outputVolume = true;

                    if (parameterString[i] == "6")
                        StockDataOrg.outputAdjClose = true;
                }

                if (!StockDataOrg.outputOpen || !StockDataOrg.outputClose)
                {
                    Console.WriteLine("Open and Closed Parameters are Required!");
                    string a = Console.ReadLine();
                    Environment.Exit(1);
                }

                //Hashtable ht = new Hashtable();

                //Open database connection.
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                if (startDateString == "")
                {
                    Console.Write("Enter starting date: ");
                    startDateString = Console.ReadLine();
                }

                if (stockString == "")
                {
                    Console.Write("Enter Stocks (Separated by commas): ");
                    stockString = Console.ReadLine();
                    htStocks = ReturnParmStocks(stockString);
                }

                if (filePathString == "")
                {
                    Console.Write("Enter FileName: ");
                    filePathString = Console.ReadLine();
                }

                if (endDateString == "")
                    endDate = DateTime.Now.AddDays(-1);
                else
                    endDate = Convert.ToDateTime(endDateString);

                Hashtable htStockIDs = ReturnStockIDs(startDateString, htStocks);
                if (VerifyStockCompanies(startDateString, htStockIDs))
                {  
                    //Console.Write("Enter ending date: ");
                    //string endDateString = Console.ReadLine();

                    DateTime startDate = Convert.ToDateTime(startDateString);
                    DateTime verificationStartDate = Convert.ToDateTime(startDateString);
                    //DateTime endDate;
                    //DateTime endDate = Convert.ToDateTime(endDateString);
                    


                    //StreamWriter sw = new StreamWriter(filePathString);
                    Console.WriteLine("Creating data text file...");

                    DateTime origDate = startDate;
                    while (endDate >= startDate)
                    {
                        Console.WriteLine("Adding data for " + startDate + ".");
                        for (int i = 0; i < htStockIDs.Count; i++)
                        {
                            //Grab all of the stocks that we're going to collect data on.
                            SqlCommand selectStocksCommand = conn.CreateCommand();
                            selectStocksCommand.CommandText = "select * from historydata " +
                            "where historydata.stockid in (select stockid from historydata " +
                            "inner join stock on historydata.stockid = stock.id " +
                            "inner join industry on stock.industry = industry.id " +
                            "where stock.id = '" + htStockIDs[i] +"'" +
                            " and historydata.[datetime] = '" + origDate + "') and historydata.[datetime] = '" +
                            startDate + "' order by historydata.stockid asc";
                            SqlDataReader stocksReader = selectStocksCommand.ExecuteReader();
                            while (stocksReader.Read())
                            {
                                string stockid = stocksReader["StockID"].ToString();
                                string datetime = stocksReader["DateTime"].ToString();
                                string open = stocksReader["Open"].ToString();
                                string high = stocksReader["High"].ToString();
                                string low = stocksReader["Low"].ToString();
                                string close = stocksReader["Close"].ToString();
                                string volume = (Convert.ToDouble(stocksReader["Volume"].ToString())/volumeNormalizer).ToString();
                                string adjClose = stocksReader["Adjusted Close"].ToString();
                                StockDataOrg.Add(stockid, datetime, open, high, low, close, volume, adjClose);
                            }

                            stocksReader.Close();
                        }
                            startDate = startDate.AddDays(1);
                    }
                    conn.Close();
                    //DateTime Now = new DateTime();
                    //Now = DateTime.Now;

                    //bool Verified = StockDataOrg.VerifyStockData(verificationStartDate, Now.AddDays(-1));
                    bool Verified = StockDataOrg.VerifyStockData(verificationStartDate, Convert.ToDateTime  (endDateString));
                    if (Verified == true)
                    { 
                        Console.WriteLine("Data is Verified!");
                        Console.WriteLine("Writing Output File");
                        StockDataOrg.WriteFile(filePathString, verificationStartDate, endDate);
                    }
                    else
                    { Console.WriteLine("Data is NOT Verified!"); }

                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
            }

            

        }

        public static String ReturnDateString(DateTime pDateTime)
        {
            return pDateTime.Month + "/" + pDateTime.Day + "/" + pDateTime.Year;
        }


        public static Boolean VerifyStockCompanies(string StartDate, Hashtable htStockIDs)
        {
            Console.WriteLine("Verifying Stock Data...");
            bool NoError = true;

            Console.WriteLine(htStockIDs.Count + " of the " + htStockIDs.Count + " Stock IDs were found existing on " + StartDate);
            if (htStockIDs.Count > 0)
            {
                Console.WriteLine("Verifying Individual Stock Data is Homogenous...");
                int StockRowCount = 0;
                int StockRowCount2 = 0;
                for(int i=0; i<htStockIDs.Count; i++)
                {
                    if (i == 0)
                    {
                        StockRowCount = CountRowsForStockID(htStockIDs[i].ToString(), StartDate);
                        Console.WriteLine("Stock: " + htStockIDs[i] + " has " + StockRowCount + " rows.");
                    }
                    else
                    {
                        StockRowCount2 = CountRowsForStockID(htStockIDs[i].ToString(), StartDate);
                        if (StockRowCount == StockRowCount2)
                        {
                            Console.WriteLine("Stock: " + htStockIDs[i] + " has " + StockRowCount + " rows.");
                        }
                        else
                        { 
                            NoError = false;
                            Console.WriteLine("Stock: " + htStockIDs[i] + " DOES NOT HAVE " + StockRowCount + " rows - Has " + StockRowCount2 + " rows!; Error, cannot continue.");
                        }
                    }
                }
            }
            else
            { NoError = false; }

            if (NoError == false)
            {
                Console.WriteLine("Stock data not homogenous, continue anyway? (y/n): ");
                string input = Console.ReadLine();
                if( input.Equals("y") || input.Equals("Y"))
                    NoError = true;
            }

            return NoError;
            //return true;    //Do this for now for testing purposes
        }

        public static int CountRowsForStockID(string StockID, string StartDate)
        {
            int RowCount = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            SqlCommand selectStocksCommand = conn.CreateCommand();
            selectStocksCommand.CommandText = "select COUNT(*) as CountRows from historydata where StockId = " + StockID + " and DateTime >= '" + StartDate + "'";
            SqlDataReader stocksReader = selectStocksCommand.ExecuteReader();
            if (stocksReader.HasRows)
            {
                stocksReader.Read();
                RowCount = Convert.ToInt32(stocksReader["CountRows"]);
            }
            return RowCount;
        }

        public static Hashtable ReturnStockIDs(string StartDate, Hashtable htStocks)
        {
            Hashtable htStockIDs = new Hashtable();

            //Open database connection.
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            for (int i = 0; i < htStocks.Count; i++)
            {
                //Grab all of the stock IDs that we're going to collect data on.
                SqlCommand selectStocksCommand = conn.CreateCommand();
                selectStocksCommand.CommandText = "select ID from Stock where Symbol='" + htStocks[i] + "'";
                SqlDataReader stocksReader = selectStocksCommand.ExecuteReader();

                if (stocksReader.HasRows)
                {
                    stocksReader.Read();
                    htStockIDs.Add(i, Convert.ToInt32(stocksReader["ID"]));
                }
                else
                {
                    Console.WriteLine("Error occurred in ReturnStockIDs(StartDate,Industry) - stocksReader has no rows");
                    throw new Exception("Error Ahhh!!!!");
                }
            }

            return htStockIDs;
        }

        public static Hashtable ReturnParmStocks(string Stocks)
        {
            string[] StockList = Stocks.Split(',');
            Hashtable htStockList = new Hashtable();

            for (int counter = 0; counter < StockList.Count(); counter++)
                htStockList.Add(counter, StockList[counter]);

            return htStockList;
        }
    }

}
