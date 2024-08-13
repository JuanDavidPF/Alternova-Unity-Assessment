using NUnit.Framework;
using UnityEngine;
using Alternova.Runtime;
using Alternova.Runtime.Tiles;

namespace Alternova.Tests
{
    public class BoardDataTests
    {
        private BoardData boardData;

        [SetUp]
        public void SetUp()
        {
            boardData = ScriptableObject.CreateInstance<BoardData>();
        }

        [Test]
        public void TestIsDataValid_NullArray_ReturnsFalse()
        {

            Assert.IsFalse(boardData.IsDataValid(null as TileData[]));
        }

        [Test]
        public void TestIsDataValid_EmptyArray_ReturnsFalse()
        {

            TileData[] emptyArray = new TileData[0];

            Assert.IsFalse(boardData.IsDataValid(emptyArray));
        }

        [Test]
        public void TestIsDataValid_ValidData_ReturnsTrue()
        {
            boardData.groupSize = 2;
            boardData.minRows = 1;
            boardData.maxRows = 1;
            boardData.minColumns = 1;
            boardData.maxColumns = 2;
            boardData.minValue = 0;
            boardData.maxValue = 9;

            TileData[] validArray = new TileData[]
            {
                new () { number = 1, R = 1, C = 1 },
                new () { number = 1, R = 1, C = 2 }
            };

            Assert.IsTrue(boardData.IsDataValid(validArray));
        }

        [Test]
        public void TestIsDataValid_NumberHasPair_ReturnsTrue()
        {
            boardData.groupSize = 3;//This is the constraint
            boardData.minRows = 1;
            boardData.maxRows = 3;
            boardData.minColumns = 1;
            boardData.maxColumns = 3;
            boardData.minValue = 0;
            boardData.maxValue = 9;

            TileData[] validArray = new TileData[]
            {
                new () { number = 3, R = 1, C = 1 },
                new () { number = 3, R = 2, C = 1 },
                new () { number = 3, R = 3, C = 1 },
                new () { number = 2, R = 1, C = 2 },
                new () { number = 2, R = 2, C = 2 },
                new () { number = 2, R = 3, C = 2 },
                new () { number = 1, R = 1, C = 3 },
                new () { number = 1, R = 2, C = 3 },
                new () { number = 1, R = 3, C = 3 }
            };

            Assert.IsTrue(boardData.IsDataValid(validArray));
        }

        [Test]
        public void TestIsDataValid_NumberHasPair_ReturnsFalse()
        {
            boardData.groupSize = 2;//This is the constraint
            boardData.minRows = 1;
            boardData.maxRows = 3;
            boardData.minColumns = 1;
            boardData.maxColumns = 3;
            boardData.minValue = 0;
            boardData.maxValue = 9;

            TileData[] invalidArray = new TileData[]
            {
                new () { number = 1, R = 1, C = 1 },
                new () { number = 1, R = 2, C = 1 },
                new () { number = 2, R = 3, C = 1 },
                new () { number = 2, R = 1, C = 2 },
                new () { number = 3, R = 2, C = 2 },
                new () { number = 3, R = 3, C = 2 },
                new () { number = 4, R = 1, C = 3 },
                new () { number = 4, R = 2, C = 3 },
                new () { number = 5, R = 3, C = 3 }
            };

            Assert.IsFalse(boardData.IsDataValid(invalidArray));
        }


        [Test]
        public void TestIsDataValid_InvalidGridSize_ReturnsFalse()
        {
            boardData.groupSize = 2;
            boardData.minRows = 1;
            boardData.maxRows = 3;
            boardData.minColumns = 1;
            boardData.maxColumns = 2;//This is the limit
            boardData.minValue = 0;
            boardData.maxValue = 9;

            TileData[] validArray = new TileData[]
            {
                new () { number = 1, R = 1, C = 1 },
                new () { number = 1, R = 2, C = 1 },
                new () { number = 2, R = 3, C = 1 },
                new () { number = 2, R = 1, C = 2 },
                new () { number = 3, R = 2, C = 2 },
                new () { number = 3, R = 3, C = 2 },
                new () { number = 4, R = 1, C = 3 },
                new () { number = 4, R = 2, C = 3 },

            };

            Assert.IsFalse(boardData.IsDataValid(validArray));
        }



        [Test]
        public void TestIsDataValid_InvalidTile_ReturnsFalse()
        {
            boardData.groupSize = 2;
            boardData.minRows = 1;
            boardData.maxRows = 2;
            boardData.minColumns = 1;
            boardData.maxColumns = 2;
            boardData.minValue = 1;
            boardData.maxValue = 9;//This is the limit

            TileData[] invalidArray = new TileData[]
            {
                new () { number = 1, R = 1, C = 1 },
                new () { number = 10, R = 1, C = 2 }
            };


            Assert.IsFalse(boardData.IsDataValid(invalidArray));
        }

        [TearDown]
        public void TearDown()
        {
            ScriptableObject.DestroyImmediate(boardData);
        }
    }
}
