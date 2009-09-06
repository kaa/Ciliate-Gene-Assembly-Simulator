using System.Collections.Generic;
namespace Domain {
	internal class MdsDescriptorFactory : IMdsDescriptorFactory {
		private IList<Mds> buffer;
		public MdsDescriptorFactory() {
			buffer = new List<Mds>();
		}
		public void Add(int incomingPointer, int[] content, int outgoingPointer) {
			buffer.Add(new Mds(incomingPointer, outgoingPointer, content));
		}
		public IMdsDescriptor Create() {
			var descriptor = new MdsDescriptor(buffer);
			buffer = new List<Mds>();
			return descriptor;
		}
	}
}
