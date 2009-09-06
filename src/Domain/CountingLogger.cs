namespace Domain {
	public class CountingLogger : ILogger {
		public int Successful;
		public int Total;
		public void LogSuccessful(IMdsDescriptor sequence) {
			Total++;
			Successful++;
		}
		public void LogUnsuccessful(IMdsDescriptor sequence) {
			Total++;
		}
	}
}
