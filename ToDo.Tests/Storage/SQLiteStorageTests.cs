using NUnit.Framework;

namespace ToDo.Tests.Storage
{
    public class SQLiteStorageTests : StorageTestsBase
    {
        private const string _kDatabasePath = "testDatabase.db";

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void CleanUp()
        {
        }

        [TestCase]
        public override void OnObjectCreation_StorageFileExists()
        {
            Assert.Fail();
        }
    }
}