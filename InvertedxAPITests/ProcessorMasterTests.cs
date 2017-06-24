using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvertedxAPI.Services;

namespace InvertedxAPITests
{
    [TestClass]
    public class ProcessorMasterTests
    {
        [TestMethod]
        public void CheckSetup()
        {
            ProcessorMaster processor = new ProcessorMaster();
            Assert.AreEqual("done", processor.Setup());
        }
    }
}
