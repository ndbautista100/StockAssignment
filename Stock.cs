using System.Collections.Generic;
using System.Text;
using System.Threading;
using System;

namespace StockAssignment
{

    //-----------------------------------------------------------------------------------
    public class Stock
    {
        // create an event for notifications
        public event EventHandler<StockNotification> StockEvent;
        //create an event for file writing
        public event EventHandler<EventArgs> FileStockEvent;
        //Name of our stock.
        private string _name;
        //Starting value of the stock.
        private int _initialValue;
        //Max change of the stock that is possible.
        private int _maxChange;
        //Threshold value where we notify subscribers to the event.
        private int _threshold;
        //Amount of changes the stock goes through.
        private int _numChanges;
        //Current value of the stock.
        private int _currentValue;
        private readonly Thread _thread;
        // initialize auto properties
        public string StockName { get; set; }
        public int InitialValue { get; set;}
        public int CurrentValue { get; set; }
        public int MaxChange { get; set; }
        public int Threshold { get; set; }
        public int NumChanges { get; set; }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stock class that contains all the information and changes of the stock
        /// </summary>
        /// <param name="name">Stock name</param>
        /// <param name="startingValue">Starting stock value</param>
        /// <param name="maxChange">The max value change of the stock</param>
        /// <param name="threshold">The range for the stock</param>
        public Stock(string name, int startingValue, int maxChange, int threshold)
        {
            StockName = name;
            InitialValue = startingValue;
            CurrentValue = startingValue;
            MaxChange = maxChange;
            Threshold = threshold;
            this._thread = new Thread(new ThreadStart(Activate));
            _thread.Start();
        }

        //each new thread will call this method which will wait 500 milsec and then call another function
        public void Activate()
        {
            for (int i = 0; i < 25; i++)
            {
                Thread.Sleep(500); // 1/2 second
                ChangeStockValue();
            }
        }

        // This method will invoke the event listeners by raising them when a stock moves past the designated threshold for a notification
        public void ChangeStockValue()
        {
            var rand = new Random();
            CurrentValue += Convert.ToInt32(rand.NextDouble() * (MaxChange - (MaxChange * -1)) + (MaxChange * -1));
            NumChanges++;
            var newStock = new StockNotification(_name, _currentValue, _numChanges);
            if (Math.Abs((CurrentValue - InitialValue)) > Threshold)
            { //RAISE THE EVENT
                StockEvent?.Invoke(this, newStock );
                FileStockEvent?.Invoke(this, EventArgs.Empty);
            }
        }


    }
}
