using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAndFolderHelperLogicTest
{
    class TestHelper : IDisposable
    {
        private string testDirectory;

        public TestHelper()
        {
            testDirectory = Directory.GetCurrentDirectory() + @"\" + Guid.NewGuid();
            Directory.CreateDirectory(testDirectory);
        }

        public string GetTestDirectory()
        {
            return testDirectory;
        }

        public void Dispose()
        {
            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true);
            }
        }
    }
}
