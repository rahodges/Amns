using System;
using System.Collections.Generic;
using System.Text;

namespace Amns.GreyFox.Statistics
{
    class SpanItem
    {
        DateTime start;
        DateTime end;

        double val;

        public double Value { get { return val; } set { val = value; } }

        public SpanItem(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }

        public void Inc()
        {
            val += 1;
        }

        public void Inc(double value)
        {
            val += value;
        }

        public void Inc(DateTime date, double val)
        {
        }
    }
}
