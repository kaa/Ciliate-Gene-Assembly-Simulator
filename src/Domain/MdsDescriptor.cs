using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Domain {
	public struct MdsDescriptor : IMdsDescriptor {
		readonly Mds[] list;
		public readonly IEnumerable<string> History;

		public IMds this[int index] { get { return list[index]; } }
		public int Length { get { return list.Length;  } }

		public MdsDescriptor( IEnumerable<string> history, params IEnumerable<Mds>[] parts ) {
			History = history;
			if(parts.Length>1) {
				list = parts.Aggregate(Enumerable.Empty<Mds>(), (acc, descriptor) => acc.Union(descriptor)).ToArray();
			} else if(parts.Length==1) {
				list = parts[0].ToArray();
			} else {
				list = new Mds[0];
			}
		}

		public MdsDescriptor(params IEnumerable<Mds>[] parts) : this(Enumerable.Empty<string>(),parts) { }

		public bool IsSuccessful {
			get {
				return list.Any(t => t.In == 0 && t.Out == 0);
			}
		}

		public bool CanLd(int p, int q) {
			return list[p].Out==list[q].In;
		}
		public bool TryLd(int p, int q, out IMdsDescriptor result) {
			if(CanLd(p, q)) {
				result = Ld(p, q);
				return true;
			}
			result = default(IMdsDescriptor);
			return false;
		}
		public IMdsDescriptor Ld(int p, int q) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Ld({0},{1}:{2}", p, q, ToString()) }),
				list.Take(p), 
				new [] {new Mds(
					list[p].In,
					list[q].Out,
					list[p].Content, new [] {list[p].Out}, list[q].Content
				)}, 
				list.Skip(q+1)
			);
		}

		public bool CanHi(int p, int q) {
			return CanHi1(p, q) || CanHi2(p, q);
		}
		public bool CanHi1(int p, int q) {
			return list[p].In==-list[q].In;
		}
		public bool CanHi2(int p, int q) {
			return list[p].Out==-list[q].Out;
		}
		
		public bool TryHi(int p, int q, out IMdsDescriptor result ) {
			if(CanHi1(p, q)) {
				result = Hi1(p, q);
				return true;
			}
			if(CanHi2(p, q)) {
				result = Hi2(p, q);
				return true;
			}
			result = default(IMdsDescriptor);
			return false;
		}
	
		public IMdsDescriptor Hi(int p, int q) {
			IMdsDescriptor result;
			if(!TryHi(p,q,out result)) {
				throw new InvalidOperationException(String.Format("Cannot apply Hi on {0} and {1}", list[p], list[q]));
			}
			return result;
		}

		public IMdsDescriptor Hi1(int p, int q) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Hi1({0},{1}):{2}", p, q, ToString()) }),
				list.Take(p),
				list.Skip(p+1).Take(q-p-1).Reverse().Select(t => t.Invert()),
				new[] {
					new Mds(
						-list[p].Out,
						list[q].Out,
						list[p].Content.Reverse().Select(t=>-t),
						new [] { -list[p].In },
						list[q].Content
						)
				},
				list.Skip(q+1)
			);
		}
		public IMdsDescriptor Hi2(int p, int q) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Hi2({0},{1}:{2}", p, q, ToString()) }),
				list.Take(p),
				new[] {
					new Mds(
						list[p].In,
						-list[q].In,
						list[p].Content,
						new [] { list[p].Out },
						list[q].Content.Reverse().Select(t=>-t)
						)
				},
				list.Skip(p+1).Take(q-p-1).Reverse().Select(t => t.Invert()),
				list.Skip(q+1)
			);
		}

		public bool CanDlad(int p, int q, int p2, int q2 ) {
			return CanDlad1(p, q, p2, q2) || CanDlad2(p, q, p2, q2) ||
			       CanDlad3(p, q, p2, q2) || CanDlad4(p, q, p2, q2) ||
			       CanDlad5(p, q, p2, q2) || CanDlad6(p, q, p2, q2) ||
						 CanDlad7(p, q, p2, q2);
		}
		public bool CanDlad1(int p, int q, int p2, int q2) {
			return
				list[p].In!=0 && list[p2].Out!=0 && list[q].In!=0 && list[q2].Out!=0 &&
				p<q && q<p2 && p2<q2 &&
				list[p].In == list[p2].Out && list[q].In == list[q2].Out;
		}
		public bool CanDlad2(int p, int q, int p2, int q2) {
			return
				list[p].In!=0 && list[p2].Out!=0 && list[q].Out!=0 && list[q2].In!=0 &&
				p<q && q<p2 && p2<q2 &&
				list[p].In == list[p2].Out && list[q].Out == list[q2].In;
		}
		public bool CanDlad3(int p, int q, int p2, int q2) {
			return
				list[p].Out!=0 && list[p2].In!=0 && list[q].In!=0 && list[q2].Out!=0 &&
				p<q && q<p2 && p2<q2 &&
				list[p].Out == list[p2].In && list[q].In == list[q2].Out;
		}
		public bool CanDlad4(int p, int q, int p2, int q2) {
			return
				list[p].Out!=0 && list[p2].In!=0 && list[q].Out!=0 && list[q2].In!=0 &&
				p<q && q<p2 && p2<q2 &&
				list[p].Out == list[p2].In && list[q].Out == list[q2].In;
		}
		public bool CanDlad5(int p, int q, int p2, int q2) {
			return
				list[p].In!=0 && list[p2].Out!=0 && list[q].In!=0 && list[q2].Out!=0 &&
				p<q && q==p2 && p2<q2 &&
				list[p].In == list[p2].Out && list[q].In == list[q2].Out;
		}
		public bool CanDlad6(int p, int q, int p2, int q2) {
			return
				list[p].In!=0 && list[p2].Out!=0 && list[q].Out!=0 && list[q2].In!=0 &&
				p==q && p<p2 && p2<q2 &&
				list[p].In == list[p2].Out && list[q].Out == list[q2].In;
		}
		public bool CanDlad7(int p, int q, int p2, int q2) {
			return
				list[p].Out!=0 && list[p2].In!=0 && list[q].In!=0 && list[q2].Out!=0 &&
				p<q && q<p2 && p2==q2 &&
				list[p].Out == list[p2].In && list[q].In == list[q2].Out;
		}
		public bool TryDlad(int p, int q, int p2, int q2, out IMdsDescriptor result) {
			if(CanDlad(p, q, p2, q2)) {
				result = Dlad(p, q, p2, q2);
				return true;
			}
			result = default(MdsDescriptor);
			return false;
		}

		public IMdsDescriptor Dlad(int p, int q, int p2, int q2) {
			if(CanDlad1(p, q, p2, q2)) {
				return Dlad1(p, q, p2, q2);
			}
			if(CanDlad2(p, q, p2, q2)) {
				return Dlad2(p, q, p2, q2);
			}
			if(CanDlad3(p, q, p2, q2)) {
				return Dlad3(p, q, p2, q2);
			}
			if(CanDlad4(p, q, p2, q2)) {
				return Dlad4(p, q, p2, q2);
			}
			if(CanDlad5(p, q, p2, q2)) {
				return Dlad5(p, q, p2, q2);
			}
			if(CanDlad6(p, q, p2, q2)) {
				return Dlad6(p, q, p2, q2);
			}
			if(CanDlad7(p, q, p2, q2)) {
				return Dlad7(p, q, p2, q2);
			}
			throw new NotImplementedException();
		}
		public IMdsDescriptor Dlad1(int p, int q, int p2, int q2 ) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad1({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				new[] {
					new Mds(
						list[q2].In,
						list[q].Out,
						list[q2].Content,
						new[] {list[q].In},
						list[q].Content
						)
				},
				list.Skip(q + 1).Take(p2 - q - 1),
				new[] {
					new Mds(
						list[p2].In,
						list[p].Out,
						list[p2].Content,
						new[] {list[p].In},
						list[p].Content
						)
				},
				list.Skip(p + 1).Take(q - p - 1),
				list.Skip(q2 + 1)
				);
		}

		public IMdsDescriptor Dlad2(int p, int q, int p2, int q2) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad2({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				list.Skip(q + 1).Take(p2 - q - 1),
				new[] {
					new Mds(
						list[p2].In,
						list[p].Out,
						list[p2].Content,
						new[] {list[p].In},
						list[p].Content)
				},
				list.Skip(p + 1).Take(q - p - 1),
				new[] {
					new Mds(
						list[q].In,
						list[q2].Out,
						list[q].Content,
						new[] {list[q].Out},
						list[q2].Content
						)
				},
				list.Skip(q2 + 1)
				);
		}
		public IMdsDescriptor Dlad3(int p, int q, int p2, int q2) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad3({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				new[] {
					new Mds(
						list[p].In,
						list[p2].Out,
						list[p].Content,
						new[] {list[p].Out},
						list[p2].Content)
				},
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				new[] {
					new Mds(
						list[q2].In,
						list[q].Out,
						list[q2].Content,
						new[] {list[q].In},
						list[q].Content
						)
				},
				list.Skip(q + 1).Take(p2 - q - 1),
				list.Skip(p + 1).Take(q - p - 1),
				list.Skip(q2 + 1)
				);
		}
		public IMdsDescriptor Dlad4(int p, int q, int p2, int q2) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad4({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				new[] {
					new Mds(
						list[p].In,
						list[p2].Out,
						list[p].Content,
						new[] {list[p].Out},
						list[p2].Content
						)
				},
				list.Skip(p2 + 1).Take(q2 - p2 - 1),
				list.Skip(q + 1).Take(p2 - q - 1),
				list.Skip(p + 1).Take(q - p - 1),
				new[] {
					new Mds(
						list[q].In,
						list[q2].Out,
						list[q].Content,
						new[] {list[q].Out},
						list[q2].Content
						)
				},
				list.Skip(q2 + 1)
				);
		}
		public IMdsDescriptor Dlad5(int p, int q, int p2, int q2) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad5({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				list.Skip(q+1).Take(q2-q-1),
				new[] {
					new Mds(
						list[q2].In,
						list[p].Out,
						list[q2].Content,
						new [] { list[q].In },
						list[q].Content,
						new [] { list[p].In },
						list[p].Content
					)
				},
				list.Skip(p+1).Take(q-p-1),
				list.Skip(q2+1)
				);
		}
		public IMdsDescriptor Dlad6(int p, int q, int p2, int q2) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad6({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				list.Skip(p2+1).Take(q2-p2-1),
				list.Skip(p+1).Take(p2-p-1),
				new[] {
					new Mds(
						list[p2].In,
						list[q2].Out,
						list[p2].Content,
						new [] { list[p].In },
						list[p].Content,
						new [] { list[p].Out },
						list[q2].Content
					)
				},
				list.Skip(q2+1)
				);
		}
		public IMdsDescriptor Dlad7(int p, int q, int p2, int q2) {
			return new MdsDescriptor(
				History.Concat(new[] { String.Format("Dlad7({0},{1},{2},{3}):{4}", p, q, p2, q2, ToString()) }),
				list.Take(p),
				new[] {
					new Mds(
						list[p].In,
						list[q].Out,
						list[p].Content,
						new [] { list[p].Out },
						list[p2].Content,
						new [] { list[q].In },
						list[q].Content
					)
				},
				list.Skip(q+1).Take(p2-q-1),
				list.Skip(p+1).Take(q-p-1),
				list.Skip(q2+1)
				);
		}

		public IEnumerator<Mds> GetEnumerator() {
			return (IEnumerator<Mds>) list.GetEnumerator();
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
