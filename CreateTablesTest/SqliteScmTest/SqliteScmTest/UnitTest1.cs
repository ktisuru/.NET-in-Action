using System.Linq;
using Xunit;
using WidgetScmDataAccess;

namespace SqliteScmTest
{   //this is comment
    public class UnitTest1 : IClassFixture<SampleScmDataFixture>
    {
      private SampleScmDataFixture fixture;
      private ScmContext context;


        public UnitTest1(SampleScmDataFixture fixture)
        {
            this.fixture = fixture;
            this.context = new ScmContext(fixture.Connection);
        }


        [Fact]
        public void Test1()
        {
            var parts = context.Parts;
            Assert.Equal(1, parts.Count());
            var part = parts.First();
            Assert.Equal("8289 L-shaped plate", part.Name);
        }
    }
}