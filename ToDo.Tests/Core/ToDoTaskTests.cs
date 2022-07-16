using NUnit.Framework;

namespace ToDo.Tests.Core
{
    public class ToDoTaskTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void CleanUp()
        {

        }

        [Test]
        [TestCase()]
        [TestCase("")]
        [TestCase("   ")]
        public void OnDescriptionSetCalled_WithEmptyString_DoesNotUpdateDescription_AndDoesNotUpdateStroge(string description = null)
        {
            Assert.Fail();
        }

        [Test]
        public void OnDescriptionSetCalled_WithNonEmptyString_UpdatesDescription_AndUpdatesStorage()
        {
            Assert.Fail();
        }

        [Test]
        public void OnHasCompletedSetCalled_UpdatesHasCompleted_AndUpdatesStorage()
        {
            Assert.Fail();
        }
    }
}
