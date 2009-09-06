namespace Domain {
	public interface ILogger {
		void LogSuccessful(IMdsDescriptor sequence);
		void LogUnsuccessful(IMdsDescriptor sequence);
	}
}
