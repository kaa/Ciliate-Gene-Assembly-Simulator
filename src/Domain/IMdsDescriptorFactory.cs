namespace Domain {
	public interface IMdsDescriptorFactory {
		void Add(int incomingPointer, int[] content, int outgoingPointer);
		IMdsDescriptor Create();
	}
}
