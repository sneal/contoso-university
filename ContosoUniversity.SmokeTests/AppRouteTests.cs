using Xunit;

namespace ContosoUniversity.SmokeTests
{
    public class AppRouteTests
    {
        private const string VcapApplication = @"
{
  ""application_id"": ""32e6c6a0-9256-345d-980c-486f16ae50d3"",
  ""application_name"": ""contoso"",
  ""application_uris"": [
  ""contoso.apps.example.com""
  ],
  ""application_version"": ""ef221e4e-485e-4ac7-905b-0b7ec22eec56"",
  ""cf_api"": ""https://api.system.example.com"",
  ""limits"": {
      ""disk"": 1024,
      ""fds"": 16384,
      ""mem"": 1024
  },
  ""name"": ""contoso"",
  ""space_id"": ""e4f35fe7-5115-4fb4-9255-76c71f6b04f3"",
  ""space_name"": ""dev"",
  ""uris"": [
    ""contoso.apps.example.com""
  ],
  ""users"": null,
  ""version"": ""ef221e4e-485e-4ac7-905b-0b7ec22eec56""
}
";
        [Fact]
        public void Can_parse_route_from_vcap_application()
        {
            var ar = new AppRoute(VcapApplication);
            Assert.Equal("https://contoso.apps.example.com/", ar.Url());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("{}")]
        [InlineData("{ \"uris\": [] }")]
        [InlineData("{ \"uris\": [\"\"] }")]
        public void Url_returns_empty_string_when_no_uris(string vcapApp)
        {
            var ar = new AppRoute(vcapApp);
            Assert.Equal("", ar.Url());
        }
    }
}
