namespace DelayedTotalRevenue.Models
{
    public sealed class CalculationResult
    {
        public double IdealTriangle { get; init; }
        public double IdealPlateau { get; init; }
        public double IdealTotal { get; init; }

        public double DelayedTriangle { get; init; }
        public double DelayedPlateau { get; init; }
        public double DelayedTotal { get; init; }

        public double AbsoluteLoss { get; init; }
        public double PercentLoss { get; init; }
    }
}