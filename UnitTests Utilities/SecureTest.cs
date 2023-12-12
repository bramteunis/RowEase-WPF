using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace UnitTests_Utilities
{
    [TestClass]
    public class SecureTest
    {
        [TestMethod]
        public void EncryptStringTest()
        {
            // Arrange
            string string1;
            string string2;
            // Act
            string1 = Secure.EncryptString("test");
            string2 = Secure.EncryptString("test");
            // Assert
            Assert.AreEqual(string1, string2);
        }

        [TestMethod]
        public void EncryptDifferentStringsTest()
        {
            // Arrange
            string string1;
            string string2;
            // Act
            string1 = Secure.EncryptString("test");
            string2 = Secure.EncryptString("test2");
            // Assert
            Assert.AreNotEqual(string1, string2);
        }
    }
}
