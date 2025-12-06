namespace DelayedTotalRevenue.Models
{
    /// <summary>
    /// Represents the pieces of a revenue calculation for an ideal launch and a delayed launch,
    /// including per-piece totals and computed loss metrics.
    /// </summary>
    public sealed class CalculationResult
    {
        /// <summary>
        /// Gets the total revenue from the triangular phases (ramp-up and ramp-down) in the ideal launch scenario.
        /// Calculated as: <c>peakRevenue * triangleWeeks</c>. Units: currency × weeks.
        /// </summary>
        /// <value>The triangular portion of the ideal revenue.</value>
        public double IdealTriangle { get; init; }

        /// <summary>
        /// Gets the total revenue from the plateau phases in the ideal launch scenario.
        /// Calculated as: <c>peakRevenue * maturityWeeks</c>.
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double IdealPlateau { get; init; }

        /// <summary>
        /// Gets the total revenue from the plateau and triangular phases in the ideal launch scenario.
        /// Calculated as: <c>idealTriangleRevenue + idealPlateauRevenue</c>.
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double IdealTotal { get; init; }

        /// <summary>
        /// Gets the total revenue from the plateau and triangular phases in the ideal launch scenario.
        /// Calculated as: <c>delayedPlateauHeight = peakRevenue * (1 - (delayWeeks / triangleWeeks))</c> and
        /// <c>delayedTriangleRevenue = delayedPlateauHeight * (triangleWeeks - delayWeeks)</c>
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double DelayedTriangle { get; init; }

        /// <summary>
        /// Gets the total revenue from the plateau phases in the ideal launch scenario.
        /// Calculated as: <c>maturityWeeks * delayedPlateauHeight</c>.
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double DelayedPlateau { get; init; }

        /// <summary>
        /// Gets the total revenue from the plateau and triangular phases in the ideal launch scenario.
        /// Calculated as: <c>delayedTriangleRevenue + delayedPlateauRevenue + delayedExtraRevenue</c>, where
        /// <c>delayedExtraRevenue = delayWeeks * delayedPlateauHeight</c>
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double DelayedTotal { get; init; }

        /// <summary>
        /// Gets the absolute loss in revenue due to the delay.
        /// Calculated as: <c>idealTotal - delayedTotal</c>.
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double AbsoluteLoss { get; init; }

        /// <summary>
        /// Gets the percentage loss in revenue due to the delay.
        /// Calculated as: <c>absoluteLoss / idealTotal</c>.
        /// Units match the product of the input peak revenue and weeks (e.g. currency × weeks).
        /// </summary>
        public double PercentLoss { get; init; }
    }
}