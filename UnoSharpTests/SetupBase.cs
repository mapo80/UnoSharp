using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace UnoSharp.Tests
{
    public class SetupBase
    {
        private static bool unoReady = true;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            try
            {
                var bootType = Type.GetType("uno.util.Bootstrap, cli_cppuhelper", false);
                if (bootType != null)
                {
                    var method = bootType.GetMethod("bootstrap", Type.EmptyTypes);
                    var ctx = method?.Invoke(null, null);
                    if (ctx == null)
                        unoReady = false;
                }
                else
                {
                    unoReady = false;
                }
            }
            catch
            {
                unoReady = false;
            }
        }
        [SetUp]
        public void Setup()
        {
            Assembly myAssembly = typeof(SetupBase).Assembly;
            string path = myAssembly.Location;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(path));

            if (!unoReady)
                Assert.Ignore("UNO runtime not available on this platform");
        }
    }
}
