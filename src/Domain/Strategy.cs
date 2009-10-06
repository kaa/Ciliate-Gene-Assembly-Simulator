using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain {
	public abstract class Strategy : IEnumerable<IOperation> {
		public abstract IEnumerator<IOperation> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public static Strategy operator +(Strategy tail, IOperation op ) {
			return new CompositeStrategy(tail, op);
		}
		public override string ToString() {
			return string.Join("+", this.Select(t=>t.ToString()).ToArray());
		}
	}
	public class EmptyStrategy : Strategy {
		public override IEnumerator<IOperation> GetEnumerator() {
			yield break;
		}
	}
	public class CompositeStrategy : Strategy {
		private readonly IOperation op;
		private readonly Strategy tail;
		public CompositeStrategy(Strategy tail, IOperation op) {
			this.op = op;
			this.tail = tail;
		}
		public override IEnumerator<IOperation> GetEnumerator() {
			foreach(var tailOp in tail) yield return tailOp;
			yield return op;
		}
	}
}
