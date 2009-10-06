namespace Domain {
	public sealed class StrategyEnumerator {
		public bool LdIsAllowed;
		public bool HiIsAllowed;
		public bool DladIsAllowed;

		private readonly IRecorder recorder;
		public StrategyEnumerator( IRecorder recorder ) {
			this.recorder = recorder;
		}
		public void Enumerate(IMdsDescriptor descriptor) {
			Enumerate(descriptor, new EmptyStrategy() );
		}
		private void Enumerate(IMdsDescriptor descriptor, Strategy strategy ) {
			var deadEnd = true;
			if(LdIsAllowed) {
				for(var p=0; p<descriptor.Length-1; p++) {
					for(var q=p+1; q<descriptor.Length; q++) {
						IMdsDescriptor t;
						if(!descriptor.TryLd(p, q, out t)) continue;
						deadEnd = false;
						Enumerate(t, strategy + new LdOperation(p,q));
					}
				}
			}
			if(HiIsAllowed) {
				for(var p=0; p<descriptor.Length-1; p++) {
					for(var q=p+1; q<descriptor.Length; q++) {
						IMdsDescriptor t;
						if(descriptor.TryHi1(p, q, out t)) {
							deadEnd = false;
							Enumerate(t, strategy + new HiOperation(p,q,1));
						}
						if(descriptor.TryHi2(p, q, out t)) {
							deadEnd = false;
							Enumerate(t, strategy + new HiOperation(p,q,2));
						}
					}
				}
			}
			if(DladIsAllowed) {
				for(var p=0; p<descriptor.Length; p++) {
					for(var q=p; q<descriptor.Length; q++) {
						for(var p2 = q; p2 < descriptor.Length; p2++) {
							for(var q2 = p2; q2 < descriptor.Length; q2++) {
								IMdsDescriptor t;
								if(descriptor.TryDlad1(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 1));
								}
								if(descriptor.TryDlad2(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 2));
								}
								if(descriptor.TryDlad3(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 3));
								}
								if(descriptor.TryDlad4(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 4));
								}
								if(descriptor.TryDlad5(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 5));
								}
								if(descriptor.TryDlad6(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 6));
								}
								if(descriptor.TryDlad7(p, q, p2, q2, out t)) {
									deadEnd = false;
									Enumerate(t, strategy + new DladOperation(p, q, p2, q2, 7));
								}
							}
						}
					}
				}
			}
			if(!deadEnd) return;
			recorder.Record(descriptor,strategy);
		}
	}
}
