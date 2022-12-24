using System.Drawing;
using Cave.Media;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class TestArgb
    {
        [Test]
        public void ToHexString()
        {
            var color = ARGB.FromColor(0xA1, 0xB2, 0xC3, 0xD4);
            Assert.AreEqual("0xA1B2C3D4", color.ToHexString());
        }

        [Test]
        public void ToHtmlString()
        {
            var color = ARGB.FromColor(0xA1, 0xB2, 0xC3, 0xD4);
            Assert.AreEqual("#B2C3D4", color.ToHtmlColor());
        }
    }
}
