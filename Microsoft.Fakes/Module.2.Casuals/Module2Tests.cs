namespace Module._2.Casuals
{
    using System;
    using System.Diagnostics;
    using System.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Module2Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var context = ShimsContext.Create())
            {
                ShimEnvironment.NewLineGet = () => "[new-line]";
                Debug.WriteLine("Environment.NewLine: {0}", Environment.NewLine);
            }
        }
    }
}
