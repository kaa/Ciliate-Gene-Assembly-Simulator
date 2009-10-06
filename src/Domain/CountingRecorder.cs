namespace Domain {
	public class CountingRecorder : IRecorder {
		public int Successful;
		public int Total;
		public void Record(IMdsDescriptor sequence, Strategy strategy ) {
			Total++;
			if(sequence.IsSuccessful) {
				Successful++;
			}
		}
	}
}
