using Utilities;

namespace UnitTests_Utilities
{
    [TestClass]
    public class ValidateTEST
    {
        [TestMethod]
        public void IsValidEmailTEST()
        {
            // Arrange
            string[] emails = {
                "test123@gmail.com",   // Valid email
                "verkeerd@.com",       // Invalid email
                "domein.com",        // Invalid email
                "juist.email@gmail.com", // Valid email
                "fout@domain",         // Invalid email
                "nog.een-goed@email.adres.com", // Valid email
                "verkeerd-email@adres@gmail.com", // Invalid email
                "",                    // Invalid email
                " "                    // Invalid email
            };
            bool[] expectedResults = {
                true,
                false,
                false,
                true,
                false,
                true,
                false,
                false,
                false
            };
            bool actual;

            for (int i = 0; i < emails.Length; i++)
            {
                // Act
                actual = Validate.IsValidEmail(emails[i]);

                // Assert
                Assert.AreEqual(expectedResults[i], actual, $"Failed for email: {emails[i]}");
            }
        }
        [TestMethod]
        public void ValidCharactersTEST()
        {
            // Arrange
            string[] testStrings = {
                "testFileForUnitTest12", // Valid characters
                "TestFileForUnitTest12", // Valid characters
                "test-file-for-unit-test-12", // Valid characters (hyphen allowed)
                "test/file/for/unit/test/12", // Valid characters (slash allowed)
                "testFile ForUnitTest12", // Invalid characters (contains space)
                "testFileForUnitTest12!", // Invalid characters (contains exclamation mark)
                "", // Invalid characters (empty string)
                "12345", // Valid characters (numbers only)
                "-/--", // Valid characters (only hyphens and slashes)
                "test\nFile", // Invalid characters (contains newline)
                "   " // Invalid characters (space only)
            };
            bool[] expectedResults = {
                true,
                true,
                true,
                true,
                false,
                false,
                false,
                true,
                true,
                false,
                false
            };
            bool actual;

            for (int i = 0; i < testStrings.Length; i++)
            {
                // Act
                actual = Validate.ValidCharacters(testStrings[i]);

                // Assert
                Assert.AreEqual(expectedResults[i], actual, $"Failed for string: {testStrings[i]}");
            }
        }
    }
}