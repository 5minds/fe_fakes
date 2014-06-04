namespace Module._2.Casuals
{
    using System;
    using System.IO;
    using System.IO.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Module2Tests
    {
        [TestMethod]
        public void Event_And_StaticFunction_Fakes()
        {
            using (ShimsContext.Create())
            {
                // arrange
                const string fileName = "test.ini";
                const string directoryName = @"not/existing";

                var handler = default(FileSystemEventHandler);

                ShimFile.ReadAllTextString = s => "timer:100";
                ShimFileSystemWatcher.ConstructorString = (watcher, s) => new ShimFileSystemWatcher(watcher)
                                                                              {
                                                                                  ChangedAddFileSystemEventHandler = h => handler = h
                                                                              };

                var sut = new Configuration(Path.Combine(directoryName, fileName));

                // act
                handler.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, directoryName, fileName));
                
                // assert
                Assert.AreEqual(TimeSpan.FromSeconds(100), sut.RefreshTime);
            }
        }
    }
}
