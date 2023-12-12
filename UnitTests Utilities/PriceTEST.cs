using System;
using Utilities;

namespace UnitTests_Utilities
{
    [TestClass]
    public class PriceTEST
    {
        [TestMethod]
        public void CalculateRentPriceTESTNoSale()
        {
            // Arrange
            string[] pricesPerHour = { "9,99", "10,00", "9,30", "8,00", "8,03" };
            string[] timeSpans = { "2 Uur", "1 Uur", "2 Uur", "0,5 Uur", "1 Uur" };
            bool sale = false;
            string[] expectedResults = { "19,98", "10,00", "18,60", "4,00", "8,03" };
            string[] actual = new string[pricesPerHour.Length]; 

            // Act
            for (int i = 0; i < pricesPerHour.Length; i++)
            {
                actual[i] = Price.CalculateRentPrice(timeSpans[i], pricesPerHour[i], sale);
            }

            // Assert
            CollectionAssert.AreEqual(expectedResults, actual);
        }
        [TestMethod]
        public void CalculateRentPriceTESTWithSale()
        {
            // Arrange
            string[] pricesPerHour = { "9,99", "12,00", "10,50", "7,00", "5,50" };
            string[] timeSpans = { "0,5 Uur", "1 Uur", "1,5 Uur", "2 Uur", "2 Uur" };
            bool sale = true; 
            string[] expectedResults = { "3,75", "9,00", "11,81", "10,50", "8,25" };
            string[] actualResults = new string[pricesPerHour.Length]; 

            // Act
            for (int i = 0; i < pricesPerHour.Length; i++)
            {
                actualResults[i] = Price.CalculateRentPrice(timeSpans[i], pricesPerHour[i], sale);
            }

            // Assert
            CollectionAssert.AreEqual(expectedResults, actualResults); 
        }
    }
}


//price time validate 