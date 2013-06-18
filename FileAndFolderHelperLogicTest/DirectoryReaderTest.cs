using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.AccessControl;
using FileAndFolderHelper;

namespace FileAndFolderHelperLogicTest
{
    [TestClass]
    public class DirectoryReaderTest
    {
        [TestMethod]
        public void Test_Get_Files_In_Sub_Directories()
        {
            TestHelper testHelper = new TestHelper();
            string directory = testHelper.GetTestDirectory();
            using (StreamWriter writer = File.CreateText(directory + @"\testFile1.txt"))
            {
                writer.Write("Dummy test file");
            }

            DirectorySecurity security = new DirectorySecurity(directory, AccessControlSections.All);
            Directory.CreateDirectory(directory + @"\inner", security);
        }
    }
}
