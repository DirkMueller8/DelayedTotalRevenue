namespace DelayedTotalRevenue
{
    using System;
    using System.Globalization;
    using DelayedTotalRevenue.Services;

    /// <summary>
    /// Entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var ci = CultureInfo.InvariantCulture;
            var calculator = new DelayCalculator();

            Console.WriteLine("=== Market Delay Calculator ===");

            do
            {
                // minimal example flow; keep console code simple and small
                Console.Write("Enter ideal ramp-up time in weeks ('e' for exit): ");
                var entry = Console.ReadLine();
                if (entry is "e" or "E")
                {
                    break;
                }

                double triangleWeeks = double.Parse(entry ?? "0", ci);

                Console.Write("Enter maturity phase length in weeks: ");
                double maturityWeeks = double.Parse(Console.ReadLine() ?? "0", ci);

                Console.Write("Enter peak revenue (per week): ");
                double peakRevenue = double.Parse(Console.ReadLine() ?? "0", ci);

                Console.Write("Enter development delay in weeks: ");
                double delayWeeks = double.Parse(Console.ReadLine() ?? "0", ci);

                var result = calculator.Calculate(triangleWeeks, maturityWeeks, peakRevenue, delayWeeks);

                Console.WriteLine($"\nIdeal total: {result.IdealTotal:F2}");
                Console.WriteLine($"Delayed total: {result.DelayedTotal:F2}");
                Console.WriteLine($"Absolute loss: {result.AbsoluteLoss:F2}");
                Console.WriteLine($"Percent loss: {result.PercentLoss:F2}%");

                // --- Recall calculation menu ---
                Console.Write("\nCalculate recall loss during maturity phase? (y/n): ");
                var recallChoice = Console.ReadLine();
                if (recallChoice is "y" or "Y")
                {
                    Console.Write("Enter number of weeks product was not sold (recall): ");
                    double recallWeeks = double.Parse(Console.ReadLine() ?? "0", ci);

                    var recallResult = calculator.CalculateRecallLoss(triangleWeeks, maturityWeeks, peakRevenue, recallWeeks);

                    Console.WriteLine($"\n--- Recall Impact ---");
                    Console.WriteLine($"Ideal total (before recall): {recallResult.IdealTotal:F2}");
                    Console.WriteLine($"Recall weeks: {recallResult.RecallWeeks:F2}");
                    Console.WriteLine($"Recall loss: {recallResult.RecallLoss:F2}");
                    Console.WriteLine($"Adjusted total (after recall): {recallResult.AdjustedTotal:F2}");
                    Console.WriteLine($"Percent loss from recall: {recallResult.PercentLoss:F2}%");
                }

                Console.WriteLine();
            }
            while (true);
        }
    }
}
