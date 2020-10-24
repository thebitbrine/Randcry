using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randcry
{
    class QualityTest
    {
        public QualityTest(byte[] Data)
        {
            this.Data = Data;
        }

        private readonly byte[] Data;

        public bool RunAllTests()
        {
            var Valid = true;
            if (!MeanValueTest()) { Valid = false; Log.Debug("Failed MeanValueTest"); }
            if (!ChiSquaredTest()) { Valid = false; Log.Debug("Failed ChiSqauredTest"); }

            return Valid;
        }

        public bool MeanValueTest()
        {
            var TestResult = Math.Round(Data.Select(x => { return (int)x; }).Average(), 1);
            Log.Debug($"Arithmetic mean value: {TestResult}");
            //return TestResult >= 127.4 && TestResult <= 127.6;
            return TestResult == 127.5;
        }

        public bool ChiSquaredTest()
        {
            var TestResult = new ChiSquared().IsRandom(Data, 255);
            Log.Debug($"Chi-Squared value: {TestResult}");
            return TestResult <= 235;
        }

    }
}
