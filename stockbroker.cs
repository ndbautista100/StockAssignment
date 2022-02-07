using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace StockAssignment
{
    class StockBroker
    {
        public string BrokerName { get; set; }
        public List<Stock> stocks = new List<Stock>();
        public static ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();
        //readonly string docPath = @"C:\Users\Documents\CECS 475\Lab3_output.txt";
        readonly string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
       "Lab1_output.txt");
        public string path = @"D:\c#\MyTest.txt";
        public string titles = "Broker".PadRight(10) + "Stock".PadRight(15) +
        "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time";

        public StockBroker(string brokerName)
        {
            BrokerName = brokerName;
            Console.WriteLine(titles);
        }

        public void AddStock(Stock stock)
        {
            stocks.Add(stock);
            stock.StockEvent += EventHandler;
            stock.FileStockEvent += TextFileEventHandler;
        }

       

        void EventHandler(Object sender, StockNotification e)
        {
            try
            { //LOCK Mechanism
                
                Stock newStock = (Stock)sender;
                string statement = BrokerName.PadRight(15) + newStock.StockName.PadRight(15) + newStock.CurrentValue.ToString().PadRight(10) + newStock.NumChanges.ToString().PadRight(10) + DateTime.Now.ToString();

                Console.WriteLine(statement);
                //string statement;
                //!NOTE!: Check out C#events, pg.4
                // Display the output to the console windows
                Console.WriteLine(BrokerName.PadRight(16));
               /*______________________________________________);
                //Display the output to the file
                using (StreamWriter outputFile
               = ________________________________________________)
                {
                    ________________________________________________
                }*/
                //RELEASE the lock
                
            }
            finally
            {
            }
        }

        void TextFileEventHandler(Object sender, EventArgs e)
        {
            try
            {
                
                Stock newStock = (Stock)sender;
                string statement = BrokerName.PadRight(15) + newStock.StockName.PadRight(15) + newStock.CurrentValue.ToString().PadRight(10) + newStock.NumChanges.ToString().PadRight(10) + DateTime.Now.ToString();
                myLock.EnterWriteLock();
                if (!File.Exists(path))
                {
                   

                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(statement);
                    }

                }
                else if (File.Exists(path))
                {
                    using (TextWriter tw = File.AppendText(path))
                    {
                        tw.WriteLine(statement);
                    }
                }
            }
            finally
            {
                myLock.ExitWriteLock();
            }
        }
    }
    
}
