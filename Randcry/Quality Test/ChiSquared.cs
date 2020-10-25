using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Randcry
{
    class ChiSquared

	{   // Calculates the chi-square value for N positive integers less than r
		// Source: "Algorithms in C" - Robert Sedgewick - pp. 517
		// NB: Sedgewick recommends: "...to be sure, the test should be tried a few times,
		// since it could be wrong in about one out of ten times."
		public double Calculate(byte[] randomNums, int r)
		{
			//Calculate the number of samples - N
			int N = randomNums.Length;

			//According to Sedgewick: "This is valid if N is greater than about 10r"
			if (N <= 10 * r)
				return double.MaxValue;

			double N_r = N / r;
			double chi_square = 0;
			Hashtable HT;

			//PART A: Get frequency of randoms
			HT = RandomFrequency(randomNums);

			//PART B: Calculate chi-square - this approach is in Sedgewick
			double f;
			foreach (DictionaryEntry Item in HT)
			{
				f = (int)Item.Value - N_r;
				chi_square += Math.Pow(f, 2);
			}
			chi_square /= N_r;
			return chi_square;
		}

		//Gets the frequency of occurrence of a randomly generated array of integers
		//Output: A hashtable, key being the random number and value its frequency
		private Hashtable RandomFrequency(byte[] randomNums)
		{
			Hashtable HT = new Hashtable();
			int N = randomNums.Length;

			for (int i = 0; i <= N - 1; i++)
			{
				if (HT.ContainsKey(randomNums[i]))
					HT[randomNums[i]] = (int)HT[randomNums[i]] + 1;
				else
					HT[randomNums[i]] = 1;
			}

			return HT;
		}
	}
}
