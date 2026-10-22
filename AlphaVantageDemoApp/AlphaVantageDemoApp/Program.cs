/** File Header
 *Property of Killian Intelligence Systems
 *Author: Paul Killian
 *Date: 3/26/2020
 
 *Description: A simple example of retrieving Stock data information from AlphaVantage
 * https://www.alphavantage.co/documentation/
**/

using System;
using System.Net;
using System.Collections.Generic;

namespace AlphaVantageDemoApp
{
    //INTRADAY URL EXAMPLE: https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={pStockSymbol}&interval=5min&apikey=demo
    //DAILY URL EXAMPLE: https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={pStockSymbol}&outputsize=compact&apikey=demo

    public struct StockDataDay
    {
        public string Symbol;
        public string Date;
        public string Open;
        public string High;
        public string Low;
        public string Close;
        public string Volume;
    }//Struct - StockDay

    class Program
    {
        static public string _APIKey = "NAMJN0O7OARDDWZF";
     

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string StockSymbol = "WEC";



            /////// THIS IS A GOOD PLACE TO STOP
            ///NEXT STEPS:
            ///     1. DO MULTIPLE STOCK SYMBOLS FROM A LIST OF STRINGS
            ///     2. EXPORT THE STOCK DATA FOR THOSE STOCKS TO A TEXT FILE
            ///     3. READ IN THE STOCK SYMBOLS FROM A TEXT FILE
            ///     4. INCLUDE OTHER PARAMETERS SUCH AS DATE RANGES AND MORE.
            ///     5. WRITE INTERFACE TO READ IN THE STOCK DATA EXPORT FILE CREATED BY THIS APP SO IT CAN BE INPUTTED BY AN ANN
            //List<string> StockSymbols = new List<string>();

            //StockSymbols.Add("MFST");
            //StockSymbols.Add("WEC");
            //StockSymbols.Add("AA");



            string RawStockData = RetrieveStockDataAlphaVantage_Method1(StockSymbol);
            List<string[]> RawStockDataByDay = ParseRawStockDataByDay(RawStockData);
            List<StockDataDay> StockDataByDay = RefineRawStockDataByDay(RawStockDataByDay, StockSymbol);

            foreach(StockDataDay StockDay in StockDataByDay)
            {
                Console.WriteLine(StockToString(StockDay));
            }


            Console.WriteLine("Bye!");

        }//Main

        #region Stock Data Retrieval Methods for Alpha Vantage

        static string RetrieveStockDataAlphaVantage_Method1(string pStockSymbol = "")
        {
            string results = "";

            using (WebClient client = new WebClient())
            {
                results = client.DownloadString($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={pStockSymbol}&outputsize=compact&apikey={_APIKey}");
            }//using

            return results;

        }//StockDataAlphaVantage_Method1

        #endregion

        #region Raw Stock Data Parsing Methods from Alpha Vantage

        static List<string[]> ParseRawStockDataByDay(string pRawStockData)
        {
            List<string[]> results = new List<string[]>();

            string[] RawStockDataSplit = pRawStockData.Split("\n");

            for (int i = 9; i < RawStockDataSplit.Length - 3; i = i + 7)
            {
                string[] currentRawStockDataDay = new string[6];

                for (int j = 0; j < 6; j++)
                {
                    currentRawStockDataDay[j] = RawStockDataSplit[i + j];
                }//for

                results.Add(currentRawStockDataDay);

            }//for

            return results;
        }//RawStockDataByDay

        #endregion
       
        #region Refine Raw Stock Data from Alpha Vantage

        static List<StockDataDay> RefineRawStockDataByDay(List<string[]> pParsedRawStockDataByDay, string pStockSymbol)
        {
            List<StockDataDay> results = new List<StockDataDay>();

            for(int i=0; i<pParsedRawStockDataByDay.Count; i++)
            {
                StockDataDay currentStockDataDay = new StockDataDay();
                string[] currentRawStockDataByDay = pParsedRawStockDataByDay[i];

                //Set the Stock symbol
                currentStockDataDay.Symbol = pStockSymbol;

                //Parse the Date string
                currentStockDataDay.Date = ParseOutDate(currentRawStockDataByDay[0]);

                //Parse the Open string
                currentStockDataDay.Open = ParseOutOpen(currentRawStockDataByDay[1]);

                //Parse the High string
                currentStockDataDay.High = ParseOutHigh(currentRawStockDataByDay[2]);

                //Parse the Low string
                currentStockDataDay.Low = ParseOutLow(currentRawStockDataByDay[3]);

                //Parse the Close string
                currentStockDataDay.Close = ParseOutClose(currentRawStockDataByDay[4]);

                //Parse the Volume string
                currentStockDataDay.Volume = ParseOutVolume(currentRawStockDataByDay[5]);

                results.Add(currentStockDataDay);



                ////Parse the Date string
                //currentStockDataDay.Date = ParseOutDate(pParsedRawStockDataByDay[i][0]);

                ////Parse the Open string
                //currentStockDataDay.Open = ParseOutOpen(pParsedRawStockDataByDay[i][1]);

                ////Parse the High string
                //currentStockDataDay.High = ParseOutHigh(pParsedRawStockDataByDay[i][2]);

                ////Parse the Low string
                //currentStockDataDay.Low = ParseOutLow(pParsedRawStockDataByDay[i][3]);

                ////Parse the Close string
                //currentStockDataDay.Close = ParseOutClose(pParsedRawStockDataByDay[i][4]);

                ////Parse the Volume string
                //currentStockDataDay.Volume = ParseOutVolume(pParsedRawStockDataByDay[i][5]);

            }//for


            return results;
        }
        #endregion

        #region ParseOutRawStockDataIndividualDatum

        static string RemoveSpecialCharacters(string pRawString)
        {
            string result = pRawString;

            result = result.Replace("\\", "");
            result = result.Replace(@"\", "");
            result = result.Replace(@":", "");
            result = result.Replace(@"{", "");
            result = result.Replace(@" ", "");
            result = result.Replace(@":", "");
            result = result.Replace(@"""", "");
            result = result.Replace(@",", "");

            return result;
        }//RemoveSpecialCharacters

        static string ParseOutDate(string pRawDate)
        {
            string Date = "N/A";

            Date = RemoveSpecialCharacters(pRawDate);

            return Date;
        }//ParseOutDate

        static string ParseOutOpen(string pRawOpen)
        {
            string Open = "N/A";

            Open = RemoveSpecialCharacters(pRawOpen);
            Open = Open.Replace("1.open", "");

            return Open;
        }//ParseOutOpen

        static string ParseOutHigh(string pRawHigh)
        {
            string High = "N/A";

            High = RemoveSpecialCharacters(pRawHigh);
            High = High.Replace("2.high", "");

            return High;
        }//ParseOutHigh

        static string ParseOutLow(string pRawLow)
        {
            string Low = "N/A";

            Low = RemoveSpecialCharacters(pRawLow);
            Low = Low.Replace("3.low", "");

            return Low;
        }//ParseOutLow

        static string ParseOutClose(string pRawClose)
        {
            string Close = "N/A";

            Close = RemoveSpecialCharacters(pRawClose);
            Close = Close.Replace("4.close", "");

            return Close;
        }//ParseOutClose

        static string ParseOutVolume(string pRawVolume)
        {
            string Volume = "N/A";

            Volume = RemoveSpecialCharacters(pRawVolume);
            Volume = Volume.Replace("5.volume", "");

            return Volume;
        }//ParseOutVolume


        #endregion

        #region Misc Methods
        static string StockToString(StockDataDay pStockDataDay)
        {
            string result = "";

            result = pStockDataDay.Symbol + " - " + pStockDataDay.Date + ": Open=" + pStockDataDay.Open + "; High=" + pStockDataDay.High + "; Low=" + pStockDataDay.Low + "; Close=" + pStockDataDay.Close + "; Volume=" + pStockDataDay.Volume;

            return result;
            
        }//StockToString
        #endregion
    }
}
