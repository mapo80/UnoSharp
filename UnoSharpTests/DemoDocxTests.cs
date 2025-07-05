using NUnit.Framework;
using System;
using System.IO;
using unoidl.com.sun.star.frame;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.uno;
using unoidl.com.sun.star.document;

namespace UnoSharp.Tests
{
    public class DemoDocxTests : SetupBase
    {
        [Test]
        public void ExportDocxToPdf()
        {
            var loader = UnoSharp.OfficeServiceManager.loader;
            var url = new Uri(Path.GetFullPath("demo.docx")).AbsoluteUri;
            var props = new PropertyValue[] {
                new PropertyValue { Name = "Hidden", Value = new uno.Any(true) },
                new PropertyValue { Name = "ReadOnly", Value = new uno.Any(true) }
            };
            XComponent doc = loader.loadComponentFromURL(url, "_blank", 0, props);
            var exportProps = new PropertyValue[] {
                new PropertyValue { Name = "FilterName", Value = new uno.Any("writer_pdf_Export") }
            };
            var pdfUrl = new Uri(Path.GetFullPath("demo.pdf")).AbsoluteUri;
            ((XStorable)doc).storeToURL(pdfUrl, exportProps);
            if (doc is unoidl.com.sun.star.util.XCloseable c)
                c.close(false);
            else
                doc.dispose();

            Assert.IsTrue(File.Exists("demo.pdf"));
        }
    }
}
