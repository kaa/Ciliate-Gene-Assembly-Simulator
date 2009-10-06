namespace Domain {
	public interface IMdsDescriptorFactory {
		void Add(int incoming, int[] content, int outgoing);
		IMdsDescriptor Create();
	}
}
