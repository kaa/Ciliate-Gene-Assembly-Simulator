using Domain;
using Xunit;

namespace Tests {
	public class MdsFixture {
		[Fact]
		public void Create() {
			var mds = new Mds(1,2);
			Assert.Equal(1, mds.incoming );
			Assert.Equal(0, mds.content.Length);
			Assert.Equal(2, mds.outgoing );
		}
		[Fact]
		public void TestEquals() {
			var a = new Mds(1,2);
			var b = new Mds(1,2);
			Assert.True( a.Equals(b), "Equal MDS:s dont match" );
		}
		[Fact]
		public void TestToString() {
			Assert.Equal("(1,2,3,4,5)", new Mds(1, 5, new[] { 2, 3, 4 }).ToString());
			Assert.Equal("(1,2)", new Mds(1, 2).ToString());
			Assert.Equal("(-1,2,3,-4)", new Mds(-1, -4, new [] { 2,3 }).ToString());
		}
		[Fact]
		public void Invert() {
			var mds = new Mds(1,5, new [] {2,3,4}).Invert();
			Assert.Equal( new [] {-4,-3,-2}, mds.content );
			Assert.Equal(-5, mds.incoming);
			Assert.Equal(-1, mds.outgoing );
		}
		[Fact]
		public void Parse() {
			Assert.Equal("(1,2)",new Mds(1,2).ToString());
			Assert.Equal("(-1,2)", new Mds(-1,2).ToString());
			Assert.Equal("(1,-2,3,4,5)", new Mds(1,5,new [] {-2,3,4}).ToString());
		}
	}
}
