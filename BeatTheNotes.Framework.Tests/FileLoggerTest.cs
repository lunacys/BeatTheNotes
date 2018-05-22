using System;
using System.IO;
using BeatTheNotes.Framework.Debugging.Logging;
using NUnit.Framework;

namespace BeatTheNotes.Framework.Tests
{
    [TestFixture]
    public class FileLoggerTest
    {
        [Test]
        public void TestFileLoggerInitialization()
        {
            FileLogger fileLogger = new FileLogger();

            Assert.That(fileLogger.FileDir, Is.Not.Empty);
            Assert.That(fileLogger.FileDir, Is.Not.Null);
            Assert.That(fileLogger.FilePath, Is.Not.Empty);
            Assert.That(fileLogger.FilePath, Is.Not.Null);

            Assert.AreEqual(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), fileLogger.FileDir);
            Assert.AreEqual(
                Path.Combine(fileLogger.FileDir,
                    $"log-{DateTime.Now.Month}.{DateTime.Now.Day}.{DateTime.Now.Year}.txt"), fileLogger.FilePath);
        }

        [Test]
        public void TestFileLoggerLogging()
        {
            FileLogger fileLogger = new FileLogger();

            fileLogger.Log("Test Log Message");

            string message;

            using (StreamReader reader = new StreamReader(fileLogger.FilePath))
                message = reader.ReadLine();

            Assert.AreEqual("Test Log Message", message);
        }
    }
}