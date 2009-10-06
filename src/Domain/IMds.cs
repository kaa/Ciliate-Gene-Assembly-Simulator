namespace Domain {
	public interface IMds {
		int In { get; }
		int Out { get; }
		int[] Content { get; }
		bool IsSuccessful { get; }
	}
}