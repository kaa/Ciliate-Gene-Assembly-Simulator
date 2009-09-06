using Moq;
using Xunit;
using Domain;
namespace Tests {
	public class MdsDescriptorParserFixture {
		public class ParseMethod {
			[Fact]
			public void An_MDS_incoming_and_outgoing_pointers_is_parsed_correctly() {
				var factory = new Mock<IMdsDescriptorFactory>(MockBehavior.Strict);
				factory.Setup(t => t.Add(1, new int[0], 2));
				factory.Setup(t => t.Create()).Returns((IMdsDescriptor)null);
				MdsDescriptorParser.Parse("(1,2)",factory.Object);
				factory.VerifyAll();
			}
			[Fact]
			public void An_MDS_with_inverted_incoming_and_outgoing_pointers_is_parsed_correctly() {
				var factory = new Mock<IMdsDescriptorFactory>(MockBehavior.Strict);
				factory.Setup(t => t.Add(-1, new int[0], -2));
				factory.Setup(t => t.Create()).Returns((IMdsDescriptor)null);
				MdsDescriptorParser.Parse("(-1,-2)", factory.Object);
				factory.VerifyAll();
			}
			[Fact]
			public void An_MDS_with_content_is_parsed_correctly() {
				var factory = new Mock<IMdsDescriptorFactory>(MockBehavior.Strict);
				factory.Setup(t => t.Add(1, new[] {2,3}, 4));
				factory.Setup(t => t.Create()).Returns((IMdsDescriptor)null);
				MdsDescriptorParser.Parse("(1,[2,3],4)", factory.Object);
				factory.VerifyAll();
			}
			[Fact]
			public void Two_MDSs_are_parsed_correctly() {
				var factory = new Mock<IMdsDescriptorFactory>(MockBehavior.Strict);
				factory.Setup(t => t.Add(1, new[] {2}, 3));
				factory.Setup(t => t.Add(4, new int[0], 5));
				factory.Setup(t => t.Create()).Returns((IMdsDescriptor)null);
				MdsDescriptorParser.Parse("(1,[2],3)(4,5)", factory.Object);
				factory.VerifyAll();
			}
		}
	}
}
