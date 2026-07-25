using System.Collections.Generic;
using Avalonia.Headless.NUnit;
using NUnit.Framework;
using OdyTools.Widgets;

namespace OdyTools.Tests
{
    [TestFixture]
    public class ModelBrowserTests
    {
        [Test]
        [AvaloniaTest]
        public void UpdateModels_BeforeAttachedToNameScope_DoesNotThrow()
        {
            var browser = new ModelBrowser();
            var models = new Dictionary<string, string>
            {
                ["m01aa_01a"] = "/tmp/m01aa_01a.mdl",
                ["m01aa_02a"] = "/tmp/m01aa_02a.mdl",
            };

            Assert.DoesNotThrow(() => browser.UpdateModels(models));
            Assert.That(browser.GetModels(), Is.EquivalentTo(models.Keys));
        }
    }
}
