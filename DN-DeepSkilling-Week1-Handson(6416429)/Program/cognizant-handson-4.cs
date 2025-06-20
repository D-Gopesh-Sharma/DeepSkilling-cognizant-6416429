using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FinancialForecastingRecursive
{
    public class FinancialData
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal GrowthRate { get; set; }

        public FinancialData(DateTime date, decimal value, decimal growthRate = 0)
        {
            Date = date;
            Value = value;
            GrowthRate = growthRate;
        }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd}: ${Value:F2} (Growth: {GrowthRate:P2})";
        }
    }

    public class RecursionExplainer
    {
        public static void ExplainRecursion()
        {
            Console.WriteLine("=== RECURSION CONCEPTS ===");
            Console.WriteLine("Recursion: A function that calls itself to solve smaller subproblems");
            Console.WriteLine("Base Case: Condition that stops the recursion");
            Console.WriteLine("Recursive Case: Function calls itself with modified parameters");
            Console.WriteLine("Stack Frame: Each recursive call creates a new stack frame");
            Console.WriteLine();
            Console.WriteLine("Benefits: Simplifies complex problems, elegant solutions");
            Console.WriteLine("Drawbacks: Stack overflow risk, potential performance issues");
            Console.WriteLine();
        }
    }

    public class FinancialForecaster
    {
        private Dictionary<string, decimal> memoCache = new Dictionary<string, decimal>();
        private long recursiveCallCount = 0;
        private long memoizedCallCount = 0;

        public decimal CalculateFutureValueRecursive(decimal initialValue, decimal growthRate, int periods)
        {
            recursiveCallCount++;
            
            if (periods == 0)
                return initialValue;
            
            return CalculateFutureValueRecursive(initialValue * (1 + growthRate), growthRate, periods - 1);
        }

        public decimal CalculateFutureValueMemoized(decimal initialValue, decimal growthRate, int periods)
        {
            string key = $"{initialValue}_{growthRate}_{periods}";
            
            if (memoCache.ContainsKey(key))
                return memoCache[key];

            memoizedCallCount++;
            
            decimal result;
            if (periods == 0)
            {
                result = initialValue;
            }
            else
            {
                result = CalculateFutureValueMemoized(initialValue * (1 + growthRate), growthRate, periods - 1);
            }
            
            memoCache[key] = result;
            return result;
        }

        public decimal CalculateCompoundInterestRecursive(decimal principal, decimal rate, int years)
        {
            if (years == 0)
                return principal;
            
            return CalculateCompoundInterestRecursive(principal * (1 + rate), rate, years - 1);
        }

        public decimal CalculateNPVRecursive(decimal[] cashFlows, decimal discountRate, int index = 0)
        {
            if (index >= cashFlows.Length)
                return 0;
            
            decimal presentValue = cashFlows[index] / (decimal)Math.Pow((double)(1 + discountRate), index);
            return presentValue + CalculateNPVRecursive(cashFlows, discountRate, index + 1);
        }

        public List<FinancialData> ForecastSeriesRecursive(decimal initialValue, decimal baseGrowthRate, 
            decimal volatility, int periods, Random? random = null)
        {
            random = random ?? new Random();
            var forecasts = new List<FinancialData>();
            
            ForecastSeriesRecursiveHelper(initialValue, baseGrowthRate, volatility, periods, 
                DateTime.Today, forecasts, random);
            
            return forecasts;
        }

        private void ForecastSeriesRecursiveHelper(decimal currentValue, decimal baseGrowthRate, 
            decimal volatility, int periodsLeft, DateTime currentDate, 
            List<FinancialData> results, Random random)
        {
            if (periodsLeft == 0)
                return;
            
            decimal adjustedGrowthRate = baseGrowthRate + (decimal)(random.NextDouble() - 0.5) * volatility;
            decimal nextValue = currentValue * (1 + adjustedGrowthRate);
            
            results.Add(new FinancialData(currentDate.AddMonths(results.Count + 1), nextValue, adjustedGrowthRate));
            
            ForecastSeriesRecursiveHelper(nextValue, baseGrowthRate, volatility, periodsLeft - 1, 
                currentDate, results, random);
        }

        public decimal CalculateFibonacciGrowthRecursive(decimal baseValue, int period)
        {
            if (period <= 1)
                return baseValue;
            
            if (period == 2)
                return baseValue * 1.1m;
            
            decimal fib1 = CalculateFibonacciGrowthRecursive(baseValue, period - 1);
            decimal fib2 = CalculateFibonacciGrowthRecursive(baseValue, period - 2);
            
            return fib1 + (fib2 * 0.1m);
        }

        public decimal CalculateFibonacciGrowthOptimized(decimal baseValue, int period)
        {
            Dictionary<int, decimal> memo = new Dictionary<int, decimal>();
            return CalculateFibonacciGrowthMemo(baseValue, period, memo);
        }

        private decimal CalculateFibonacciGrowthMemo(decimal baseValue, int period, Dictionary<int, decimal> memo)
        {
            if (memo.ContainsKey(period))
                return memo[period];
            
            decimal result;
            if (period <= 1)
                result = baseValue;
            else if (period == 2)
                result = baseValue * 1.1m;
            else
            {
                decimal fib1 = CalculateFibonacciGrowthMemo(baseValue, period - 1, memo);
                decimal fib2 = CalculateFibonacciGrowthMemo(baseValue, period - 2, memo);
                result = fib1 + (fib2 * 0.1m);
            }
            
            memo[period] = result;
            return result;
        }

        public void ResetCounters()
        {
            recursiveCallCount = 0;
            memoizedCallCount = 0;
            memoCache.Clear();
        }

        public long GetRecursiveCallCount() => recursiveCallCount;
        public long GetMemoizedCallCount() => memoizedCallCount;
    }

    public class PerformanceAnalyzer
    {
        public static void AnalyzeTimeComplexity()
        {
            Console.WriteLine("=== TIME COMPLEXITY ANALYSIS ===");
            Console.WriteLine("Basic Recursive Future Value: O(n) - linear time");
            Console.WriteLine("Memoized Version: O(n) - but with reduced constant factor");
            Console.WriteLine("Fibonacci-based Growth: O(2^n) - exponential without memoization");
            Console.WriteLine("Fibonacci with Memoization: O(n) - linear time");
            Console.WriteLine("NPV Calculation: O(n) - linear in number of cash flows");
            Console.WriteLine();
        }

        public static void CompareRecursiveVsIterative(FinancialForecaster forecaster)
        {
            Console.WriteLine("=== RECURSIVE VS ITERATIVE COMPARISON ===");
            
            decimal initialValue = 1000m;
            decimal growthRate = 0.05m;
            int[] testPeriods = { 5, 10, 15, 20 };
            
            Console.WriteLine("Periods | Recursive | Iterative | Difference");
            Console.WriteLine("--------|-----------|-----------|----------");
            
            foreach (int periods in testPeriods)
            {
                Stopwatch sw1 = Stopwatch.StartNew();
                decimal recursiveResult = forecaster.CalculateFutureValueRecursive(initialValue, growthRate, periods);
                sw1.Stop();
                
                Stopwatch sw2 = Stopwatch.StartNew();
                decimal iterativeResult = CalculateFutureValueIterative(initialValue, growthRate, periods);
                sw2.Stop();
                
                bool same = Math.Abs(recursiveResult - iterativeResult) < 0.01m;
                string status = same ? "Same" : "Different";
                
                Console.WriteLine($"{periods,7} | {sw1.ElapsedTicks,9} | {sw2.ElapsedTicks,9} | {status}");
            }
            Console.WriteLine();
        }

        private static decimal CalculateFutureValueIterative(decimal initialValue, decimal growthRate, int periods)
        {
            decimal result = initialValue;
            for (int i = 0; i < periods; i++)
            {
                result *= (1 + growthRate);
            }
            return result;
        }

        public static void TestFibonacciPerformance(FinancialForecaster forecaster)
        {
            Console.WriteLine("=== FIBONACCI PERFORMANCE TEST ===");
            Console.WriteLine("Period | Recursive (ms) | Optimized (ms) | Speedup");
            Console.WriteLine("-------|----------------|----------------|--------");
            
            int[] testPeriods = { 10, 15, 20, 25 };
            
            foreach (int period in testPeriods)
            {
                Stopwatch sw1 = Stopwatch.StartNew();
                decimal recursiveResult = forecaster.CalculateFibonacciGrowthRecursive(1000m, period);
                sw1.Stop();
                
                Stopwatch sw2 = Stopwatch.StartNew();
                decimal optimizedResult = forecaster.CalculateFibonacciGrowthOptimized(1000m, period);
                sw2.Stop();
                
                double speedup = sw1.ElapsedMilliseconds > 0 ? 
                    (double)sw1.ElapsedMilliseconds / sw2.ElapsedMilliseconds : 1.0;
                
                Console.WriteLine($"{period,6} | {sw1.ElapsedMilliseconds,14} | {sw2.ElapsedMilliseconds,14} | {speedup:F1}x");
            }
            Console.WriteLine();
        }
    }

    public class OptimizationTechniques
    {
        public static void ExplainOptimizations()
        {
            Console.WriteLine("=== OPTIMIZATION TECHNIQUES ===");
            Console.WriteLine("1. MEMOIZATION:");
            Console.WriteLine("   - Cache results of expensive recursive calls");
            Console.WriteLine("   - Trades space for time complexity");
            Console.WriteLine("   - Most effective for overlapping subproblems");
            Console.WriteLine();
            Console.WriteLine("2. TAIL RECURSION:");
            Console.WriteLine("   - Recursive call is the last operation");
            Console.WriteLine("   - Can be optimized to iterative by compiler");
            Console.WriteLine("   - Reduces stack frame usage");
            Console.WriteLine();
            Console.WriteLine("3. ITERATIVE CONVERSION:");
            Console.WriteLine("   - Convert recursive solution to loops");
            Console.WriteLine("   - Eliminates stack overflow risk");
            Console.WriteLine("   - Often more memory efficient");
            Console.WriteLine();
            Console.WriteLine("4. DYNAMIC PROGRAMMING:");
            Console.WriteLine("   - Bottom-up approach using tables");
            Console.WriteLine("   - Eliminates redundant calculations");
            Console.WriteLine("   - Optimal for problems with optimal substructure");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FINANCIAL FORECASTING WITH RECURSIVE ALGORITHMS");
            Console.WriteLine("===============================================");
            Console.WriteLine();

            try
            {
                RecursionExplainer.ExplainRecursion();
                
                FinancialForecaster forecaster = new FinancialForecaster();
                
                Console.WriteLine("=== BASIC RECURSIVE FORECASTING ===");
                decimal initialValue = 10000m;
                decimal growthRate = 0.08m;
                int periods = 10;
                
                decimal futureValue = forecaster.CalculateFutureValueRecursive(initialValue, growthRate, periods);
                Console.WriteLine($"Initial Value: ${initialValue:F2}");
                Console.WriteLine($"Growth Rate: {growthRate:P2} per period");
                Console.WriteLine($"Periods: {periods}");
                Console.WriteLine($"Future Value: ${futureValue:F2}");
                Console.WriteLine();
                
                Console.WriteLine("=== COMPOUND INTEREST CALCULATION ===");
                decimal compound = forecaster.CalculateCompoundInterestRecursive(5000m, 0.06m, 5);
                Console.WriteLine($"$5,000 at 6% for 5 years: ${compound:F2}");
                Console.WriteLine();
                
                Console.WriteLine("=== NET PRESENT VALUE CALCULATION ===");
                decimal[] cashFlows = { -1000m, 300m, 400m, 500m, 600m };
                decimal npv = forecaster.CalculateNPVRecursive(cashFlows, 0.10m);
                Console.WriteLine($"NPV of cash flows at 10% discount rate: ${npv:F2}");
                Console.WriteLine();
                
                Console.WriteLine("=== FORECASTING WITH VOLATILITY ===");
                var forecasts = forecaster.ForecastSeriesRecursive(1000m, 0.05m, 0.02m, 12, new Random(42));
                Console.WriteLine("12-month forecast with 5% base growth and 2% volatility:");
                foreach (var forecast in forecasts.Take(6))
                {
                    Console.WriteLine($"  {forecast}");
                }
                Console.WriteLine($"  ... and {forecasts.Count - 6} more periods");
                Console.WriteLine();
                
                PerformanceAnalyzer.AnalyzeTimeComplexity();
                PerformanceAnalyzer.CompareRecursiveVsIterative(forecaster);
                PerformanceAnalyzer.TestFibonacciPerformance(forecaster);
                OptimizationTechniques.ExplainOptimizations();
                
                Console.WriteLine("=== MEMOIZATION DEMONSTRATION ===");
                forecaster.ResetCounters();
                
                decimal result1 = forecaster.CalculateFutureValueRecursive(1000m, 0.05m, 15);
                long recursiveCalls = forecaster.GetRecursiveCallCount();
                
                forecaster.ResetCounters();
                decimal result2 = forecaster.CalculateFutureValueMemoized(1000m, 0.05m, 15);
                long memoizedCalls = forecaster.GetMemoizedCallCount();
                
                Console.WriteLine($"Recursive calls: {recursiveCalls}");
                Console.WriteLine($"Memoized calls: {memoizedCalls}");
                Console.WriteLine($"Results match: {Math.Abs(result1 - result2) < 0.01m}");
                Console.WriteLine();
                
                Console.WriteLine("=== PRACTICAL RECOMMENDATIONS ===");
                Console.WriteLine("1. Use iterative solutions for simple growth calculations");
                Console.WriteLine("2. Apply memoization for recursive algorithms with overlapping subproblems");
                Console.WriteLine("3. Consider tail recursion optimization where possible");
                Console.WriteLine("4. Implement stack depth limits to prevent overflow");
                Console.WriteLine("5. Use dynamic programming for complex financial models");
                Console.WriteLine("6. Cache frequently computed values in production systems");
                Console.WriteLine();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}