using System.IO;
using BeatTheNotes.Framework.Debugging.Logging;
using NUnit.Framework;

namespace BeatTheNotes.Framework.Tests
{
    [TestFixture]
    public class LogHelperTests
    {
        [Test]
        public void TestLogHelperInitialization()
        {
            LogHelper.Target = LogTarget.File;

            for (int i = 0; i < 10; i++)
            {
                LogHelper.Log($"Hello from the Test! {i}", LogLevel.Info);
                LogHelper.Update();
            }

            // TODO: This
        }
    }
}