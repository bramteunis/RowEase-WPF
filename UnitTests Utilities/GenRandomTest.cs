using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace UnitTests_Utilities
{
    [TestClass]
    public class GenRandomTest
    {
        [TestMethod]
        public void GenerateRandomStringTest()
        {
            // Arrange
            string oldString;
            string newString;
            // Act
            oldString = GenRandom.GenerateRandomString();
            for(int i = 0; i < 10; i++)
            {
                newString = GenRandom.GenerateRandomString();
                // Assert
                Assert.AreNotEqual(oldString, newString);
                oldString = newString;
            }
        }
    }
}
