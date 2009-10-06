using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Domain {
	internal struct LinqMdsDescriptor : IMdsDescriptor {
		static readonly Mds[] EmptyContent = new Mds[0];

		readonly Mds[] list;

		public int Length { get { return list.Length;  } }

		internal LinqMdsDescriptor( params IEnumerable<Mds>[] parts ) {
			if(parts.Length>1) {
				list = parts.Aggregate(Enumerable.Empty<Mds>(), (acc, descriptor) => acc.Union(descriptor)).ToArray();
			} else if(parts.Length==1) {
				list = parts[0].ToArray();
			} else {
				list = EmptyContent;
			}
		}

		public bool IsSuccessful {
			get { return list.Any(t => t.IsSuccessful); }
		}

		public bool CanLd(int p, int q) {
			return list[p].outgoing==list[q].incoming;
		}
		public bool TryLd(int p, int q, out IMdsDescriptor result) {
			if(CanLd(p, q)) {
				result = Ld(p, q);
				return true;
			}
			result = default(IMdsDescriptor);
			return false;
		}
		public LinqMdsDescriptor Ld(int p, int q) {
			return new LinqMdsDescriptor(
				list.Take(p), 
				new [] {new Mds(
					list[p].incoming,
					list[q].outgoing,
					list[p].content, new [] {list[p].outgoing}, list[q].content
				)}, 
				list.Skip(q+1)
			);
		}

		public bool CanHi1(int p, int q) {
			return list[p].incoming==-list[q].incoming;
		}
		public bool CanHi2(int p, int q) {
			return list[p].outgoing==-list[q].outgoing;
		}

		public bool TryHi1(int p, int q, out IMdsDescriptor result) {
			if(CanHi1(p, q)) {
				result = Hi1(p, q);
				return true;
			}
			result = default(IMdsDescriptor);
			return false;
		}
		public bool TryHi2(int p, int q, out IMdsDescriptor result) {
			if(CanHi2(p, q)) {
				result = Hi2(p, q);
				return true;
			}
			result = default(IMdsDescriptor);
			return false;
		}
		public LinqMdsDescriptor Hi1(int p, int q) {
			return new LinqMdsDescriptor(
				list.Take(p),
				list.Skip(p+1).Take(q-p-1).Reverse().Select(t => t.Invert()),
				new[] {
					new Mds(
						-list[p].outgoing,
						list[q].outgoing,
						list[p].content.Reverse().Select(t=>-t),
						new [] { -list[p].incoming },
						list[q].content
						)
				},
				list.Skip(q+1)
			);
		}
		public LinqMdsDescriptor Hi2(int p, int q) {
			return new LinqMdsDescriptor(
				list.Take(p),
				new[] {
					new Mds(
						list[p].incoming,
						-list[q].incoming,
						list[p].content,
						new [] { list[p].outgoing },
						list[q].content.Reverse().Select(t=>-t)
						)
				},
				list.Skip(p+1).Take(q-p-1).Reverse().Select(t => t.Invert()),
				list.Skip(q+1)
			);
		}

		public bool CanDlad1(int p, int q, int p2, int q2) {
			return
				p<q && q<p2 && p2<q2 &&
				list[p].incoming == list[p2].outgoing && list[q].incoming == list[q2].outgoing;
		}
		public bool CanDlad2(int p, int q, int p2, int q2) {
			return
				p<q && q<p2 && p2<q2 &&
				list[p].incoming == list[p2].outgoing && list[q].outgoing == list[q2].incoming;
		}
		public bool CanDlad3(int p, int q, int p2, int q2) {
			return
				p<q && q<p2 && p2<q2 &&
				list[p].outgoing == list[p2].incoming && list[q].incoming == list[q2].outgoing;
		}
		public bool CanDlad4(int p, int q, int p2, int q2) {
			return
				p<q && q<p2 && p2<q2 &&
				list[p].outgoing == list[p2].incoming && list[q].outgoing == list[q2].incoming;
		}
		public bool CanDlad5(int p, int q, int p2, int q2) {
			return
				p<q && q==p2 && p2<q2 &&
				list[p].incoming == list[p2].outgoing && list[q].incoming == list[q2].outgoing;
		}
		public bool CanDlad6(int p, int q, int p2, int q2) {
			return
				p==q && p<p2 && p2<q2 &&
				list[p].incoming == list[p2].outgoing && list[q].outgoing == list[q2].incoming;
		}
		public bool CanDlad7(int p, int q, int p2, int q2) {
			return
				p<q && q<p2 && p2==q2 &&
				list[p].outgoing == list[p2].incoming && list[q].incoming == list[q2].outgoing;
		}
		public bool TryDlad1(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad1(p, q, p2, q2)) {
				result = Dlad1(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}
		public bool TryDlad2(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad2(p, q, p2, q2)) {
				result = Dlad2(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}
		public bool TryDlad3(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad3(p, q, p2, q2)) {
				result = Dlad3(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}
		public bool TryDlad4(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad4(p, q, p2, q2)) {
				result = Dlad4(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}
		public bool TryDlad5(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad5(p, q, p2, q2)) {
				result = Dlad5(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}
		public bool TryDlad6(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad6(p, q, p2, q2)) {
				result = Dlad6(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}
		public bool TryDlad7(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad7(p, q, p2, q2)) {
				result = Dlad7(p, q, p2, q2);
				return true;
			}
			result = default(LinqMdsDescriptor);
			return false;
		}

		public LinqMdsDescriptor Dlad1(int p, int q, int p2, int q2 ) {
			return new LinqMdsDescriptor(
				list.Take(p),
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				new[] {
					new Mds(
						list[q2].incoming,
						list[q].outgoing,
						list[q2].content,
						new[] {list[q].incoming},
						list[q].content
						)
				},
				list.Skip(q + 1).Take(p2 - q - 1),
				new[] {
					new Mds(
						list[p2].incoming,
						list[p].outgoing,
						list[p2].content,
						new[] {list[p].incoming},
						list[p].content
						)
				},
				list.Skip(p + 1).Take(q - p - 1),
				list.Skip(q2 + 1)
				);
		}

		public LinqMdsDescriptor Dlad2(int p, int q, int p2, int q2) {
			return new LinqMdsDescriptor(
				list.Take(p),
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				list.Skip(q + 1).Take(p2 - q - 1),
				new[] {
					new Mds(
						list[p2].incoming,
						list[p].outgoing,
						list[p2].content,
						new[] {list[p].incoming},
						list[p].content)
				},
				list.Skip(p + 1).Take(q - p - 1),
				new[] {
					new Mds(
						list[q].incoming,
						list[q2].outgoing,
						list[q].content,
						new[] {list[q].outgoing},
						list[q2].content
						)
				},
				list.Skip(q2 + 1)
				);
		}
		public LinqMdsDescriptor Dlad3(int p, int q, int p2, int q2) {
			return new LinqMdsDescriptor(
				list.Take(p),
				new[] {
					new Mds(
						list[p].incoming,
						list[p2].outgoing,
						list[p].content,
						new[] {list[p].outgoing},
						list[p2].content)
				},
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				new[] {
					new Mds(
						list[q2].incoming,
						list[q].outgoing,
						list[q2].content,
						new[] {list[q].incoming},
						list[q].content
						)
				},
				list.Skip(q + 1).Take(p2 - q - 1),
				list.Skip(p + 1).Take(q - p - 1),
				list.Skip(q2 + 1)
				);
		}
		public LinqMdsDescriptor Dlad4(int p, int q, int p2, int q2) {
			return new LinqMdsDescriptor(
				list.Take(p),
				new[] {
					new Mds(
						list[p].incoming,
						list[p2].outgoing,
						list[p].content,
						new[] {list[p].outgoing},
						list[p2].content
						)
				},
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				list.Skip(q + 1).Take(p2 - q - 1),
				list.Skip(p + 1).Take(q - p - 1),
				new[] {
					new Mds(
						list[q].incoming,
						list[q2].outgoing,
						list[q].content,
						new[] {list[q].outgoing},
						list[q2].content
						)
				},
				list.Skip(q2 + 1)
				);
		}
		public LinqMdsDescriptor Dlad5(int p, int q, int p2, int q2) {
			return new LinqMdsDescriptor(
				list.Take(p),
				list.Skip(q+1).Take(q2-q-1),
				new[] {
					new Mds(
						list[q2].incoming,
						list[p].outgoing,
						list[q2].content,
						new [] { list[q].incoming },
						list[q].content,
						new [] { list[p].incoming },
						list[p].content
					)
				},
				list.Skip(p+1).Take(q-p-1),
				list.Skip(q2+1)
				);
		}
		public LinqMdsDescriptor Dlad6(int p, int q, int p2, int q2) {
			return new LinqMdsDescriptor(
				list.Take(p),
				list.Skip(p2+1).Take(q2-p2-1),
				list.Skip(p+1).Take(p2-p-1),
				new[] {
					new Mds(
						list[p2].incoming,
						list[q2].outgoing,
						list[p2].content,
						new [] { list[p].incoming },
						list[p].content,
						new [] { list[p].outgoing },
						list[q2].content
					)
				},
				list.Skip(q2+1)
				);
		}
		public LinqMdsDescriptor Dlad7(int p, int q, int p2, int q2) {
			return new LinqMdsDescriptor(
				list.Take(p),
				new[] {
					new Mds(
						list[p].incoming,
						list[q].outgoing,
						list[p].content,
						new [] { list[p].outgoing },
						list[p2].content,
						new [] { list[q].incoming },
						list[q].content
					)
				},
				list.Skip(q+1).Take(p2-q-1),
				list.Skip(p+1).Take(q-p-1),
				list.Skip(q2+1)
				);
		}

		public IEnumerator<IMds> GetEnumerator() {
			return (IEnumerator<IMds>) list.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public override string ToString() {
			if(list==null) return "";
			var sb = new StringBuilder();
			foreach(var mds in list) sb.Append(mds);
			return sb.ToString();
		}
	}
}
