using Randcry.Quality_Test;
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
            if (!MeanValueTest()) { Log.Debug("Failed MeanValueTest"); return false; }
            if (!ChiSquaredTest()) { Log.Debug("Failed ChiSqauredTest"); return false; }
            if (!EntropyTest()) { Log.Debug("Failed EntropyTest"); return false; }

            return true;
        }

        public bool EntropyTest()
        {
            var TestResult = Math.Round(new ShannonEntropy().Calculate(Data), 4);
            Log.Debug($"Entropy bits per byte: {TestResult}");
            return TestResult >= 7.9996;

        }

        public bool MeanValueTest()
        {
            var TestResult = Math.Round(new ArithmeticMean().Calculate(Data), 1);
            Log.Debug($"Arithmetic mean value: {TestResult}");
            //return TestResult >= 127.4 && TestResult <= 127.6;
            return TestResult == 127.5;
        }

        public bool ChiSquaredTest()
        {
            var TestResult = new ChiSquared().Calculate(Data, 256);
            Log.Debug($"Chi-Squared value: {TestResult}");
            return TestResult <= 235;
        }

    }
}
