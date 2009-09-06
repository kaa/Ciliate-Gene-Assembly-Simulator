using System.IO;

namespace Domain {
	public class TraceLogger : ILogger {
		private TextWriter successfulWriter;
		private TextWriter unsuccessfulWriter;
		private ILogger wrappedLogger;

		public TraceLogger(TextWriter writer) : this(null, writer) {
		}
		public TraceLogger(ILogger wrappedLogger, TextWriter writer ) {
			successfulWriter = writer;
			unsuccessfulWriter = writer;
			this.wrappedLogger = wrappedLogger;
		}
	
		public void LogSuccessful(IMdsDescriptor sequence) {
			if(successfulWriter!=null) {
				successfulWriter.WriteLine("");
			}
			if(wrappedLogger==null) return;
			wrappedLogger.LogSuccessful(sequence);
		}

		public void LogUnsuccessful(IMdsDescriptor sequence) {
			if(unsuccessfulWriter!=null) {
				unsuccessfulWriter.WriteLine("");
			}
			if(wrappedLogger==null) return;
			wrappedLogger.LogUnsuccessful(sequence);
		}
	}
}
