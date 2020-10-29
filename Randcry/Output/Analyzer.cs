using Randcry.Quality_Test;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using AForge.Video.DirectShow;
using Randcry.Extensions;
using static Randcry.Extensions.ByteArray;

namespace Randcry.Output
{
    class Analyzer
    {
        public readonly string RandFile;
        public Analyzer(string RandFile)
        {
            this.RandFile = RandFile;
        }
        public void ContinuousAnalysis(VideoCaptureDevice Device, int Delay = 60000)
        {
            Log.Debug($"Started continuous analysis for: {Device.SourceObject.GetHashCode():X}");
            Thread.Sleep(10000);
            while (true)
            {
                var Results = AnalyzeSample(new Configs().GetOutputFilePath(Device));
                if (Results != null)
                {
                    PrintAnalysisReport(Results);
                    Thread.Sleep(Delay);
                }
                else
                {
                    Thread.Sleep(1111);
                }
            }
        }

        public void PrintAnalysisReport(AnalysisResults Results)
        {
            Log.Information("");
            Log.Information("==================ANALYSIS REPORT==================");
            Log.Information($"Size: {Results.SampleLength.GetSize()}");
            Log.Information($"Entropy: {Math.Round(Results.EntropyBitsPerByte, 6)}");
            Log.Information($"ChiSquared: {Math.Round(Results.ChiSquaredValue, 2)}");
            Log.Information($"ArithmeticMean: {Math.Round(Results.ArithmeticMeanValue, 4)}");
            Log.Information($"SerialCorrelationCoefficient: {Math.Round(Results.SerialCorrelationCoefficient, 5)}");
            Log.Information($"Sample: {Results.FileName}");
            Log.Information("===================================================");
            Log.Information("");
        }

        public AnalysisResults AnalyzeSample(string SamplePath)
        {
            byte[] RandBytes = null;
            try
            {
                RandBytes = SamplePath.ReadBytes();
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred while analyzing sample: {SamplePath}", ex);
            }
            if (RandBytes == null || RandBytes.Length == 0)
                return null;

            return new AnalysisResults()
            {
                FileName = SamplePath,
                SampleLength = RandBytes.Length,
                ChiSquaredValue = new ChiSquared().Calculate(RandBytes, 256),
                SerialCorrelationCoefficient = new SerialCorrelation().Calculate(RandBytes),
                ArithmeticMeanValue = new ArithmeticMean().Calculate(RandBytes),
                EntropyBitsPerByte = new ShannonEntropy().Calculate(RandBytes)
            };

        }

    }

    public class AnalysisResults
    {
        public string FileName;
        public double ChiSquaredValue;
        public double SerialCorrelationCoefficient;
        public double ArithmeticMeanValue;
        public double EntropyBitsPerByte;
        public long SampleLength;
    }
}
