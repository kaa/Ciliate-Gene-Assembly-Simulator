using System.Collections.Generic;
namespace Domain {
	internal class LinqMdsDescriptorFactory : IMdsDescriptorFactory {
		private IList<Mds> buffer;
		public LinqMdsDescriptorFactory() {
			buffer = new List<Mds>();
		}
		public void Add(int incoming, int[] content, int outgoing) {
			buffer.Add(new Mds(incoming, outgoing, content));
		}
		public IMdsDescriptor Create() {
			var descriptor = new LinqMdsDescriptor(buffer);
			buffer = new List<Mds>();
			return descriptor;
		}
	}
}
