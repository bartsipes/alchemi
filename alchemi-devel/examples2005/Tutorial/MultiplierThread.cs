using System;
using System.Collections.Generic;
using System.Text;

using Alchemi.Core.Owner;

namespace Alchemi.Examples.Tutorial
{
    [Serializable]
    class MultiplierThread : GThread
    {
        private int _A, _B, _Result;

        public int Result
        {
            get { return _Result; }
        }

        public MultiplierThread(int a, int b)
        {
            _A = a;
            _B = b;
        }

        public override void Start()
        {
            if (Id == 0) { int x = 5 / Id; } // divide by zero
            _Result = _A * _B * _A;
        }
    }
}
