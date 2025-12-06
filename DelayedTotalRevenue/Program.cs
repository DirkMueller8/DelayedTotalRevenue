namespace DelayedTotalRevenue
{
    using System;
    using System.Globalization;
    using DelayedTotalRevenue.Services;


    public static class Program
    {
        static void Main()
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
            } while (true);
        }
    }
}
