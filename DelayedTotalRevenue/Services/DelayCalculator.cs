namespace DelayedTotalRevenue.Services
{
    using System;
    using DelayedTotalRevenue.Models;

    /// <summary>
    /// Calculates the revenue impact of a delayed product launch.
    public class DelayCalculator
    {
        /// <summary>
        /// Calculates revenue pieces for an ideal launch and for a launch delayed by <paramref name="delayWeeks"/>,
        /// and returns aggregated loss metrics.
        /// </summary>
        /// <param name="triangleWeeks">Duration in weeks of the ramp-up/ramp-down triangular phases. Must be &gt; 0.</param>
        /// <param name="maturityWeeks">Duration in weeks of the plateau (mature) phase. Must be &gt;= 0.</param>
        /// <param name="peakRevenue">Peak revenue per week at full maturity (units: currency per week). Must be &gt;= 0.</param>
        /// <param name="delayWeeks">Development delay in weeks. Must be &gt;= 0 and &lt; <paramref name="triangleWeeks"/> and &lt;= <paramref name="maturityWeeks"/> per the current validation rules.</param>
        /// <returns>
        /// A <see cref="CalculationResult"/> containing:
        /// <list type="bullet">
        /// <item>ideal and delayed triangle and plateau revenues,</item>
        /// <item>total revenues for ideal and delayed scenarios, and</item>
        /// <item>absolute and percent loss due to the delay.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more input arguments are outside the allowed ranges.</exception>
        /// <remarks>
        /// Formulas used (summary):
        /// <c>idealTriangle = peakRevenue * triangleWeeks</c>,
        /// <c>idealPlateau = peakRevenue * maturityWeeks</c>,
        /// <c>delayedPlateauHeight = peakRevenue * (1 - (delayWeeks / triangleWeeks))</c>,
        /// and delayed totals are computed piecewise from that height.
        /// </remarks>
        public CalculationResult Calculate(double triangleWeeks, double maturityWeeks, double peakRevenue, double delayWeeks)
        {
            ValidateInputs(triangleWeeks, maturityWeeks, peakRevenue, delayWeeks);

            var (idealTriangle, idealPlateau, idealTotal) = ComputeIdealPieces(triangleWeeks, maturityWeeks, peakRevenue);
            var (delayedTriangle, delayedPlateau, delayedExtra, delayedTotal, delayedPlateauHeight) =
                ComputeDelayedPieces(triangleWeeks, maturityWeeks, peakRevenue, delayWeeks);

            var (absoluteLoss, percentLoss) = ComputeLosses(idealTotal, delayedTotal);

            return new CalculationResult
            {
                IdealTriangle = idealTriangle,
                IdealPlateau = idealPlateau,
                IdealTotal = idealTotal,
                DelayedTriangle = delayedTriangle,
                DelayedPlateau = delayedPlateau,
                DelayedTotal = delayedTotal,
                AbsoluteLoss = absoluteLoss,
                PercentLoss = percentLoss,
            };
        }

        // Preserve argument validation in a single small method
        private static void ValidateInputs(double triangleWeeks, double maturityWeeks, double peakRevenue, double delayWeeks)
        {
            if (triangleWeeks <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(triangleWeeks));
            }

            if (maturityWeeks < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maturityWeeks));
            }

            if (peakRevenue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(peakRevenue));
            }

            if (delayWeeks < 0 || delayWeeks >= triangleWeeks)
            {
                throw new ArgumentOutOfRangeException(nameof(delayWeeks));
            }

            if (delayWeeks > maturityWeeks)
            {
                throw new ArgumentOutOfRangeException(nameof(delayWeeks));
            }
        }

        // Returns (triangleArea, plateauArea, total)
        private static (double triangle, double plateau, double total) ComputeIdealPieces(double triangleWeeks, double maturityWeeks, double peakRevenue)
        {
            double idealTriangleRevenue = triangleWeeks * peakRevenue; // area of full triangle (ramp-up + ramp-down)
            double idealPlateauRevenue = maturityWeeks * peakRevenue;
            double idealTotalRevenue = idealTriangleRevenue + idealPlateauRevenue;

            return (idealTriangleRevenue, idealPlateauRevenue, idealTotalRevenue);
        }

        // Returns (delayedTriangleArea, delayedPlateauArea, delayedExtraRect, delayedTotal, delayedPlateauHeight)
        private static (double triangle, double plateau, double extra, double total, double plateauHeight) ComputeDelayedPieces(
            double triangleWeeks, double maturityWeeks, double peakRevenue, double delayWeeks)
        {
            double delayedPlateauHeight = peakRevenue * (1 - (delayWeeks / triangleWeeks)); // height of delayed plateau
            double delayedTriangleRevenue = delayedPlateauHeight * (triangleWeeks - delayWeeks); // area of both delayed triangle portions
            double delayedPlateauRevenue = maturityWeeks * delayedPlateauHeight;
            double delayedExtraRevenue = delayWeeks * delayedPlateauHeight; // additional rectangle before hitting decline curve
            double delayedTotalRevenue = delayedTriangleRevenue + delayedPlateauRevenue + delayedExtraRevenue;

            return (delayedTriangleRevenue, delayedPlateauRevenue, delayedExtraRevenue, delayedTotalRevenue, delayedPlateauHeight);
        }

        // Returns (absoluteLoss, percentLoss)
        private static (double absolute, double percent) ComputeLosses(double idealTotal, double delayedTotal)
        {
            double absoluteLoss = idealTotal - delayedTotal;
            double percentLoss = idealTotal > 0 ? (absoluteLoss / idealTotal) * 100.0 : 0.0;

            return (absoluteLoss, percentLoss);
        }
    }
}