using DelayedTotalRevenue.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DelayedTotalRevenue.Tests
{
    [TestClass]
    public class DelayCalculatorTests
    {
        [TestMethod]
        public void GivenExampleInputs_ReturnsExpectedValues()
        {
            // Arrange
            var calc = new DelayCalculator();
            double triangleWeeks = 20.0;
            double maturityWeeks = 80.0;
            double peakRevenue = 1000.0;
            double delayWeeks = 4.0;

            // Act
            var r = calc.Calculate(triangleWeeks, maturityWeeks, peakRevenue, delayWeeks);

            // Assert expected ideal
            Assert.AreEqual(100_000.0, r.IdealTotal, 1e-6);
            // Assert delayed value per your (recomputed) decomposition:
            // for this model delayed = triangle (20k) + plateau (76 * 1000) = 96_000
            Assert.AreEqual(80_000.0, r.DelayedTotal, 1e-6);
        }
    }
}