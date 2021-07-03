using NUnit.Framework;

namespace RandomThoughts.UnitTests
{
    public class Tests
    {
        private byte[] _buffer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _buffer = new byte[24];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(_buffer);
            }
        }

        [Test]
        public void MemoryStream_GetBuffer_SizeIsNotDynamic()
        {
            // Arrange
            var memoryStream = new System.IO.MemoryStream();

            // Act
            memoryStream.Write(_buffer, 0, _buffer.Length);
            var getBuffer = memoryStream.GetBuffer();

            // Assert
            Assert.AreNotEqual(_buffer.Length, getBuffer.Length);
        }

        [Test]
        public void MemoryStream_ToArray_SizeIsDynamic()
        {
            // Arrange
            var memoryStream = new System.IO.MemoryStream();

            // Act
            memoryStream.Write(_buffer, 0, _buffer.Length);
            var toArray = memoryStream.ToArray();

            // Assert
            Assert.AreEqual(_buffer.Length, toArray.Length);
        }

        [Test]
        public void MemoryStream_ToArray_PreAllocatedBuffer_SizeIsDynamic()
        {
            // Arrange
            var memoryStream = new System.IO.MemoryStream(8096);

            // Act
            memoryStream.Write(_buffer, 0, _buffer.Length);
            var toArray = memoryStream.ToArray();

            // Assert
            Assert.AreEqual(_buffer.Length, toArray.Length);
        }
    }
}
