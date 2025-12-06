namespace DelayedTotalRevenue.Tests
{
    using System;
    using DelayedTotalRevenue.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="DelayCalculator"/> class.
    /// Verifies correct numeric outputs for representative inputs and asserts proper validation behavior for invalid arguments.
    /// </summary>
    /// <remarks>
    /// Tests are focused and small: one test exercises the example calculation, one verifies the no-delay equality,
    /// and the remaining tests assert that invalid inputs throw <see cref="ArgumentOutOfRangeException"/>.
    /// </remarks>
    [TestClass]
    public class DelayCalculatorTests
    {
        /// <summary>
        /// Tests the calculation with the example inputs.
        /// </summary>
        [TestMethod]
        public void GivenExampleInputs_ReturnsExpectedValues()
        {
            var calc = new DelayCalculator();
            double triangleWeeks = 20.0;
            double maturityWeeks = 80.0;
            double peakRevenue = 1000.0;
            double delayWeeks = 4.0;

            var r = calc.Calculate(triangleWeeks, maturityWeeks, peakRevenue, delayWeeks);

            Assert.AreEqual(20_000.0, r.IdealTriangle, 1e-6);
            Assert.AreEqual(80_000.0, r.IdealPlateau, 1e-6);
            Assert.AreEqual(100_000.0, r.IdealTotal, 1e-6);

            Assert.AreEqual(80_000.0, r.DelayedTotal, 1e-6);
            Assert.AreEqual(20_000.0, r.AbsoluteLoss, 1e-6);
            Assert.AreEqual(20.0, r.PercentLoss, 1e-6);
        }

        /// <summary>
        /// Tests the calculation with the example inputs.
        /// </summary>
        [TestMethod]
        public void NoDelay_ReturnsSameAsIdeal()
        {
            var calc = new DelayCalculator();
            double triangleWeeks = 10.0;
            double maturityWeeks = 5.0;
            double peakRevenue = 200.0;
            double delayWeeks = 0.0;

            var r = calc.Calculate(triangleWeeks, maturityWeeks, peakRevenue, delayWeeks);

            Assert.AreEqual(r.IdealTotal, r.DelayedTotal, 1e-9);
            Assert.AreEqual(0.0, r.AbsoluteLoss, 1e-9);
            Assert.AreEqual(0.0, r.PercentLoss, 1e-9);
        }

        /// <summary>
        /// Tests the calculation with the example inputs.
        /// </summary>
        [TestMethod]
        public void InvalidTriangle_Throws()
        {
            var calc = new DelayCalculator();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calc.Calculate(0.0, 10.0, 100.0, 0.0));
        }

        /// <summary>
        /// Tests the calculation with the example inputs.
        /// </summary>
        [TestMethod]
        public void NegativeMaturity_Throws()
        {
            var calc = new DelayCalculator();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calc.Calculate(10.0, -1.0, 100.0, 0.0));
        }

        /// <summary>
        /// Tests the calculation with the example inputs.
        /// </summary>
        [TestMethod]
        public void NegativePeak_Throws()
        {
            var calc = new DelayCalculator();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calc.Calculate(10.0, 5.0, -50.0, 0.0));
        }

        /// <summary>
        /// Tests the calculation with the example inputs.
        /// </summary>
        [TestMethod]
        public void InvalidDelay_Throws_WhenNegativeOrTooLarge()
        {
            var calc = new DelayCalculator();

            // negative
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calc.Calculate(10.0, 5.0, 100.0, -1.0));

            // equal to triangleWeeks (invalid per current implementation)
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calc.Calculate(10.0, 5.0, 100.0, 10.0));

            // greater than maturity (also checked)
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => calc.Calculate(10.0, 5.0, 100.0, 6.0));
        }
    }
}