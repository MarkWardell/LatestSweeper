using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sweeper.ViewModels;

namespace SweeperTest
{
    [TestClass]
    public class TestViewModel
    {
        [TestMethod]
        public void  CreateGame()
        {
            var vm = new SweeperViewModel();

            Assert.AreNotEqual(null, vm);
        }
    }
}
