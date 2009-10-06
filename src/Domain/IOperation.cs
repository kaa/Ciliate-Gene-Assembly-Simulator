namespace Domain {
	public interface IOperation { }
	struct LdOperation : IOperation {
		public readonly int P, Q;
		public LdOperation(int p, int q) {
			P = p; Q = q;
		}
		public override string ToString() {
			return string.Format("Ld({0},{1})", P, Q);
		}
	}
	struct HiOperation : IOperation {
		public readonly int P, Q, Type;
		public HiOperation(int p, int q, int type) {
			P = p; Q = q; Type = type;
		}
		public override string ToString() {
			return string.Format("Hi{0}({1},{2})", Type, P, Q);
		}
	}
	struct DladOperation : IOperation {
		public readonly int P, Q, P2, Q2, Type;
		public DladOperation(int p, int q, int p2, int q2, int type) {
			P = p; Q = q; P2 = p2; Q2 = q2; Type = type;
		}
		public override string ToString() {
			return string.Format("Dlad{0}({1},{2},{3},{4})", Type, P, Q, P2, Q2 );
		}
	}
}
