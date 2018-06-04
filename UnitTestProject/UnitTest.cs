using System.Diagnostics;
using Windows.ApplicationModel.Appointments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _3DSoftEngine.Scripts;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext testcontext { get; set; }
        [ClassInitialize]
        public void Setup(TestContext test)
        {
            testcontext = test;
        }
        [TestMethod]
        public void TestMethod1()
        {
            var matrix = MatrixUnit.CreateRotateMatrix(90, 0, 0);
            testcontext.WriteLine("ff");
            Assert.IsTrue(true);
        }
    }
}
