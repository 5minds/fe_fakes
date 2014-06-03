namespace Module._1.Configuration
{
    using System;
    using System.Diagnostics;
    using System.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Module1Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var context = ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(1973, 12, 24);
                Debug.WriteLine("DateTime.Now [shims-context]: {0}", DateTime.Now);

                Debug.WriteLine("DateTime.Now [no shims-context]: {0}", ShimsContext.ExecuteWithoutShims(() => DateTime.Now));
            }
        }
    }
}
