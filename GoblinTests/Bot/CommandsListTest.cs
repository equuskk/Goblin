using Goblin.Bot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoblinTests.Bot
{
    [TestClass]
    public class CommandsListTest
    {
        [TestMethod]
        public async void ExecuteWrongCommand()
        {
            var result = await CommandsList.ExecuteCommand("�����", 1);
            Assert.AreEqual(result, CommandsList.ErrorMessage);
        }

        [TestMethod]
        public void ExecuteRightCommand()
        {
            var result = CommandsList.ExecuteCommand("������ 1 ��� 2", 1);
            Assert.AreNotEqual(result, CommandsList.ErrorMessage);
        }
    }
}
