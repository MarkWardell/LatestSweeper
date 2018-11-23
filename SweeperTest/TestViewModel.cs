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
            for (int i = 0; i < 100; i++)
                vm.NewGameCommand.Execute("SAMEGAME");
            Assert.AreNotEqual(null, vm);
        }
    }
}
