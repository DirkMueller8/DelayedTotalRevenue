namespace DelayedTotalRevenue.Models
{
    /// <summary>
    /// Represents the revenue loss caused by a product recall during the maturity (plateau) phase.
    /// </summary>
    public sealed class RecallResult
    {
        /// <summary>
        /// Gets the original ideal total revenue (before recall).
        /// </summary>
        public double IdealTotal { get; init; }

        /// <summary>
        /// Gets the number of weeks the product was not sold due to the recall.
        /// </summary>
        public double RecallWeeks { get; init; }

        /// <summary>
        /// Gets the revenue lost during the recall period.
        /// Calculated as: <c>recallWeeks * peakRevenue</c>.
        /// </summary>
        public double RecallLoss { get; init; }

        /// <summary>
        /// Gets the adjusted total revenue after the recall loss.
        /// Calculated as: <c>idealTotal - recallLoss</c>.
        /// </summary>
        public double AdjustedTotal { get; init; }

        /// <summary>
        /// Gets the percentage of revenue lost due to the recall.
        /// Calculated as: <c>(recallLoss / idealTotal) * 100</c>.
        /// </summary>
        public double PercentLoss { get; init; }
    }
}