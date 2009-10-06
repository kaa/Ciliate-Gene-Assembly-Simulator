using System.IO;

namespace Domain {
	public class TraceRecorder : IRecorder {
		private readonly TextWriter writer;
		private readonly IRecorder wrappedLogger;

		public TraceRecorder(TextWriter writer) : this(null, writer) {
		}
		public TraceRecorder(IRecorder wrappedLogger, TextWriter writer ) {
			this.writer = writer;
			this.wrappedLogger = wrappedLogger;
		}
	
		public void Record(IMdsDescriptor sequence, Strategy strategy ) {
			if(writer!=null) {
				writer.WriteLine("{0} {1} => {2}", sequence.IsSuccessful?"SUCCESS":"FAILURE", strategy, sequence);
			}
			if(wrappedLogger==null) return;
			wrappedLogger.Record(sequence,strategy);
		}
	}
}
