using Randcry.Quality_Test;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Randcry.Output
{
    class Analyzer
    {
        public void ContinuousAnalysis()
        {
            while (true)
            {
                var FileName = new Configs().GetOutputFileName();
                var Results = AnalyzeSample(FileName);
                Log.Information(
                    $"Sample: {Results.FileName}, " +
                    $"Length: {Results.SampleLength}, " +
                    $"ChiSquared: {Math.Round(Results.ChiSquaredValue, 2)}, " +
                    $"ArithmeticMean: {Math.Round(Results.ArithmeticMeanValue, 4)}, " +
                    $"Entropy: {Math.Round(Results.EntropyBitsPerByte, 6)}");
                Thread.Sleep(5555);
            }
        }
        public void AnalyzeAndPrint()
        {
            var FileName = new Configs().GetOutputFileName();
            var Results = AnalyzeSample(FileName);
            Log.Information(
                $"Sample: {Results.FileName}, " +
                $"Length: {Results.SampleLength}, " +
                $"ChiSquared: {Math.Round(Results.ChiSquaredValue, 2)}, " +
                $"ArithmeticMean: {Math.Round(Results.ArithmeticMeanValue, 4)}, " +
                $"Entropy: {Math.Round(Results.EntropyBitsPerByte, 6)}");

        }

        public AnalyzeResults AnalyzeSample(string SamplePath)
        {
            var RandBytes = File.ReadAllBytes(SamplePath);
            return new AnalyzeResults()
            {
                FileName = SamplePath,
                SampleLength = RandBytes.Length,
                ChiSquaredValue = new ChiSquared().Calculate(RandBytes, 255),
                ArithmeticMeanValue = new ArithmeticMean().Calculate(RandBytes),
                EntropyBitsPerByte = new ShannonEntropy().Calculate(RandBytes)
            };
        }

    }

    public class AnalyzeResults
    {
        public string FileName;
        public double ChiSquaredValue;
        public double ArithmeticMeanValue;
        public double EntropyBitsPerByte;
        public long SampleLength;
    }
}
