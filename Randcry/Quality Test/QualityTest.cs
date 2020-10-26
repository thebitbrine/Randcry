﻿using Randcry.Quality_Test;
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
            if (!SerialCorrelationTest()) { Log.Debug("Failed SerialCorrelationTest"); return false; }
            if (!ChiSquaredTest()) { Log.Debug("Failed ChiSquaredTest"); return false; }
            if (!EntropyTest()) { Log.Debug("Failed EntropyTest"); return false; }

            return true;
        }

        public bool EntropyTest()
        {
            var TestResult = Math.Round(new ShannonEntropy().Calculate(Data), 4);
            Log.Debug($"Entropy bits per byte: {TestResult}");
            return TestResult >= 7.9985;

        }

        public bool MeanValueTest()
        {
            var TestResult = Math.Round(new ArithmeticMean().Calculate(Data), 1);
            Log.Debug($"Arithmetic mean value: {TestResult}");
            return Math.Abs(TestResult - 127.5) < 0.2;
        }

        public bool ChiSquaredTest()
        {
            var TestResult = new ChiSquared().Calculate(Data, 256);
            Log.Debug($"Chi-Squared value: {TestResult}");
            return TestResult <= 235;
        }

        public bool SerialCorrelationTest()
        {
            var TestResult = Math.Round(new SerialCorrelation().Calculate(Data), 2);
            Log.Debug($"Serial correlation coefficient: {TestResult}");
            return TestResult < 0.01 && TestResult > -0.01;

        }

    }
}
