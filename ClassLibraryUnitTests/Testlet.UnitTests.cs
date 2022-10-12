using System.Collections.Generic;
using System.Linq;
using ClassLibrary1;
using ClassLibrary1.Models;
using ClassLibrary1.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ArcadiaUnittests
{
    [TestClass]
    public class TestletUnitTests
    {
        private Mock<IRandomNumberProvider> _providerMock = new Mock<IRandomNumberProvider>();

        private readonly List<Item> _items = new List<Item>
        {
            new Item {ItemId = "1", ItemType = ItemTypeEnum.Pretest},
            new Item {ItemId = "2", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "3", ItemType = ItemTypeEnum.Pretest},
            new Item {ItemId = "4", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "5", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "6", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "7", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "8", ItemType = ItemTypeEnum.Pretest},
            new Item {ItemId = "9", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "10", ItemType = ItemTypeEnum.Pretest},
        };

        [TestInitialize]
        public void TestInitialize()
        {
            _providerMock.SetupSequence(provider => provider.Get())
                .Returns(1)
                .Returns(2)
                .Returns(3)
                .Returns(4)
                .Returns(5)
                .Returns(6)
                .Returns(7)
                .Returns(8)
                .Returns(9)
                .Returns(10)
                .Returns(11)
                .Returns(12);
        }

        [TestMethod]
        public void ReturnsTenItems()
        {
            var testlet = new Testlet("testletId", _items, _providerMock.Object);
            var actual = testlet.Randomize();

            const int expected = 10;
            Assert.AreEqual(expected, actual.Count(), "Incorrect number of elements");
        }

        [TestMethod]
        public void ReturnsSixOperationalAndFourPretestItems()
        {
            var testlet = new Testlet("testletId", _items, _providerMock.Object);
            var actual = testlet.Randomize();

            Assert.AreEqual(6, actual.Count(item => item.ItemType == ItemTypeEnum.Operational),
                "Incorrect number of operational elements");
            Assert.AreEqual(4, actual.Count(item => item.ItemType == ItemTypeEnum.Pretest),
                "Incorrect number of pretest elements");
        }

        [TestMethod]
        public void ReturnsFirstTwoItemsAreAlwaysPretestItems()
        {
            var testlet = new Testlet("testletId", _items, _providerMock.Object);
            var randomizedItems = testlet.Randomize();

            var actual = randomizedItems.Take(2);
            const ItemTypeEnum expected = ItemTypeEnum.Pretest;
            Assert.AreEqual(expected, actual.First().ItemType, "Incorrect item type for first element");
            Assert.AreEqual(expected, actual.Last().ItemType, "Incorrect item type for second element");
            Assert.AreNotEqual(actual.First().ItemId, actual.Last().ItemId, "The first element is equal to the second");
        }

        [TestMethod]
        public void ReturnsNextEightItemsAreMixOfPretestAndOperationalItems()
        {
            var testlet = new Testlet("testletId", _items, _providerMock.Object);
            var randomizedItems = testlet.Randomize();

            var firstTwoItems = randomizedItems.Take(2);
            var actual = randomizedItems.Skip(2);
            Assert.AreEqual(6, actual.Count(item => item.ItemType == ItemTypeEnum.Operational),
                "Incorrect number of operational elements");
            Assert.AreEqual(2, actual.Count(item => item.ItemType == ItemTypeEnum.Pretest),
                "Incorrect number of pretest elements");
            Assert.IsFalse(actual.Contains(firstTwoItems.First()), "Eight items contains first elements");
            Assert.IsFalse(actual.Contains(firstTwoItems.Last()), "Eight items contains second elements");
        }


        [TestMethod]
        public void ReturnsNextEightItemsOrderedRandomlyFromTheRemainingEightItems()
        {
            var providerMock = new Mock<IRandomNumberProvider>();
            providerMock.SetupSequence(provider => provider.Get())
                .Returns(12)
                .Returns(11)
                .Returns(10)
                .Returns(9)
                .Returns(8)
                .Returns(7)
                .Returns(6)
                .Returns(5)
                .Returns(4)
                .Returns(3)
                .Returns(2)
                .Returns(1);

            var testlet = new Testlet("testletId", _items, providerMock.Object);
            var randomizedItems = testlet.Randomize();

            var firstTwoItems = randomizedItems.Take(2);
            var actual = randomizedItems.Skip(2);
            Assert.AreEqual("10", firstTwoItems.First().ItemId, "Incorrect order for first element");
            Assert.AreEqual("8", firstTwoItems.Last().ItemId, "Incorrect order for second element");
            Assert.AreEqual("9", actual.First().ItemId, "Incorrect order for third element");
            Assert.AreEqual("7", actual.Skip(1).First().ItemId, "Incorrect order for fourth element");
            Assert.AreEqual("1", actual.Last().ItemId, "Incorrect order for last element");
        }
    }
}