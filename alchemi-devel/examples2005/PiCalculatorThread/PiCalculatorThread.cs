using System;
using System.Threading;
using System.Reflection;
using System.Text;
using Alchemi.Core;
using Alchemi.Core.Owner;

namespace Alchemi.Examples.PiCalculator
{
    [Serializable]
    public class PiCalculatorThread : GThread
    {
        private int _StartDigitNum;
        private int _NumDigits;
        private string _Result;

        public int StartDigitNum
        {
            get { return _StartDigitNum ; }
        }

        public int NumDigits
        {
            get { return _NumDigits; }
        }

        public string Result
        {
            get { return _Result; }
        }

        public PiCalculatorThread(int startDigitNum, int numDigits)
        {
            _StartDigitNum = startDigitNum;
            _NumDigits = numDigits;
        }

        public override void Start()
        {
            StringBuilder temp = new StringBuilder();

            PlouffeBellard pb = new PlouffeBellard();
            for (int i = 0; i <= Math.Ceiling((double)_NumDigits / 9); i++)
            {
                temp.Append(pb.CalculatePiDigits(_StartDigitNum + (i * 9)));
            }

            _Result = temp.ToString().Substring(0, _NumDigits);

			for (int i = 0; i < int.MaxValue; i++);
        }
    }
}


