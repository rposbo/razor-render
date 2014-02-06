using System;
using System.Web.Helpers;
using NUnit.Framework;
using RazorEngine.Templating;
using Xipton.Razor;

namespace razor_render.Tests
{
    [TestFixture]
    public class WhenUsingStrings
    {

        private string _layoutRazor;
        private string _templateRazor;
        private dynamic _modelJson;
        private const string MatchText = "Hello World";

        [SetUp]
        public void Setup()
        {
            _modelJson = Json.Decode("{\"Description\":\"Hello World\"}");
            _templateRazor = "<div class=\"helloworld\">@Model.Description</div>";
            _layoutRazor = "<html><body>@RenderBody()</body></html>";
        }

        [Test]
        public void RazorEngineResultShouldContainTestText()
        {
            _templateRazor = string.Format("{0}{1}", "@{Layout=\"_layout\";}", _templateRazor);

            using (var service = new TemplateService())
            {
                service.GetTemplate(_layoutRazor, null, "_layout");
                service.GetTemplate(_templateRazor, _modelJson, "template");

                var result = service.Parse(_templateRazor, _modelJson, null, "page");

                Console.Write(result);
                Assert.IsTrue(result.Contains(MatchText));
            }
        }

        [Test]
        public void RazorMachineResultShouldContainTestText()
        {
            var rm = new RazorMachine();
            rm.RegisterTemplate("~/shared/_layout.cshtml", _layoutRazor);

            var renderedContent = rm.ExecuteContent(string.Format("{0}{1}", "@{Layout=\"_layout\";}", _templateRazor), _modelJson);
            var result = renderedContent.Result;

            Console.Write(result);
            Assert.IsTrue(result.Contains(MatchText));
        }
    }
}