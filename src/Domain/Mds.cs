using System;
using System.Linq;
using System.Collections.Generic;

namespace Domain {
	internal struct Mds : IMds {
		internal readonly int incoming;
		internal readonly int outgoing;
		internal readonly int[] content;
		public int In { get { return incoming; } }
		public int Out { get { return outgoing; } }
		public int[] Content { get { return content; } }
		public bool IsSuccessful {
			get {
				return incoming == Pointer.Start && outgoing == Pointer.End || 
				       incoming == -Pointer.End && outgoing == -Pointer.Start;
			}
		}

		public Mds(int left, int right, params IEnumerable<int>[] contents)
			: this() {
			incoming = left;
			outgoing = right;
			content = contents.Aggregate(Enumerable.Empty<int>(),(acc, seq) => acc.Concat(seq)).ToArray();
		}
		public Mds Invert() {
			return new Mds(-Out, -In, Content.Reverse().Select(t => -t).ToArray());
		}
		public override string ToString() {
			if(Content.Length>0) {
				return String.Format("({0},{1},{2})", 
					In,
					String.Concat(content.Select(t => "," + t).ToArray()).Substring(1), 
					Out);
			}
			return String.Format("({0},{1})", In, Out);
		}
		public bool Equals(Mds other) {
			if(In!=other.In||Out!=other.Out) return false;
			if(Content.Length!=other.Content.Length) return false;
			for(var i=0; i<Content.Length; i++) {
				if(Content[i]!=other.Content[i]) {
					return false;
				}
			}
			return true;
		}
		public override bool Equals(object obj) {
			if(!(obj is Mds)) return false;
			return Equals((Mds)obj);
		}
		public override int GetHashCode() {
			return In.GetHashCode() + Out.GetHashCode() + Content.GetHashCode();
		}
	}
}
