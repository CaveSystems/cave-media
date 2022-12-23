using System.ComponentModel;
using System.Drawing;
using Cave.Media;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class TestBitmap32
    {
        #region Public Methods

#if !NETCOREAPP
        [Test]
        public void TestGdiBlend()
        {
            var loader = new GdiBitmap32Loader();

            var red = loader.Create(60, 60);
            red.Clear(Color.Red);
            var green = loader.Create(60, 60);
            green.Clear(Color.Green);
            var blue = loader.Create(60, 60);
            blue.Clear(Color.Blue);

            var bmp = loader.Create(120, 120);
            bmp.Draw(red, 0, 0);
            bmp.Draw(green, 60, 0);
            bmp.Draw(blue, 0, 60);

            var data = bmp.GetImageData();
            Assert.AreEqual((ARGB)Color.Red, data[30, 30]);
            Assert.AreEqual((ARGB)Color.Green, data[90, 30]);
            Assert.AreEqual((ARGB)Color.Blue, data[30, 90]);
            Assert.AreEqual((ARGB)0, data[90, 90]);
        }
#endif

#if NET47_OR_GREATER || NETCOREAPP
        [Test]
        public void TestSkiaBlend()
        {
            var loader = new SkiaBitmap32Loader();

            var red = loader.Create(60, 60);
            red.Clear(Color.Red);
            var green = loader.Create(60, 60);
            green.Clear(Color.Green);
            var blue = loader.Create(60, 60);
            blue.Clear(Color.Blue);

            var bmp = loader.Create(120, 120);
            bmp.Draw(red, 0, 0);
            bmp.Draw(green, 60, 0);
            bmp.Draw(blue, 0, 60);

            var data = bmp.GetImageData();
            Assert.AreEqual((ARGB)Color.Red, data[30, 30]);
            Assert.AreEqual((ARGB)Color.Green, data[90, 30]);
            Assert.AreEqual((ARGB)Color.Blue, data[30, 90]);
            Assert.AreEqual((ARGB)0, data[90, 90]);
        }
#endif

        #endregion Public Methods
    }
}
