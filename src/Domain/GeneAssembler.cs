namespace Domain {
	public sealed class GeneAssembler {
		public bool LdIsAllowed;
		public bool HiIsAllowed;
		public bool DladIsAllowed;

		private readonly ILogger logger;
		public GeneAssembler( ILogger logger ) {
			this.logger = logger;
		}

		public void Assemble(IMdsDescriptor descriptor) {
			if(descriptor.IsSuccessful) {
				logger.LogSuccessful(descriptor);
				return;
			}
			var deadEnd = true;
			if(LdIsAllowed) {
				for(var p=0; p<descriptor.Length-1; p++) {
					for(var q=p+1; q<descriptor.Length; q++) {
						if (!descriptor.CanLd(p, q)) continue;
						deadEnd = false;
						Assemble(descriptor.Ld(p, q));
					}
				}
			}
			if(HiIsAllowed) {
				for(var p=0; p<descriptor.Length-1; p++) {
					for(var q=p+1; q<descriptor.Length; q++) {
						if(descriptor.CanHi1(p, q)) {
							deadEnd = false;
							Assemble(descriptor.Hi1(p, q));
						}
						if(descriptor.CanHi2(p, q)) {
							deadEnd = false;
							Assemble(descriptor.Hi2(p, q));
						}
					}
				}
			}
			if(DladIsAllowed) {
				for(var p=0; p<descriptor.Length; p++) {
					for(var q=p; q<descriptor.Length; q++) {
						for(var p2 = q; p2 < descriptor.Length; p2++) {
							for(var q2 = p2; q2 < descriptor.Length; q2++) {
								if(descriptor.CanDlad1(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad1(p, q, p2, q2));
								}
								if(descriptor.CanDlad2(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad2(p, q, p2, q2));
								}
								if(descriptor.CanDlad3(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad3(p, q, p2, q2));
								}
								if(descriptor.CanDlad4(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad4(p, q, p2, q2));
								}
								if(descriptor.CanDlad5(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad5(p, q, p2, q2));
								}
								if(descriptor.CanDlad6(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad6(p, q, p2, q2));
								}
								if(descriptor.CanDlad7(p, q, p2, q2)) {
									deadEnd = false;
									Assemble(descriptor.Dlad7(p, q, p2, q2));
								}
							}
						}
					}
				}
			}
			if(!deadEnd) return;
			logger.LogUnsuccessful(descriptor);
		}
	}
}
