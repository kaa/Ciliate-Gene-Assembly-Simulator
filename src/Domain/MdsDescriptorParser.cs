using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Domain {
	public class MdsDescriptorParser {
		static readonly Regex SequencePattern = new Regex(@"\((?<ptrs>-?(\d+|\w)(,-?(\d+|\w))+)\)");
		public static IMdsDescriptor Parse(string str) {
			return Parse(str, new LinqMdsDescriptorFactory());
		}

		public static IMdsDescriptor Parse( string str, IMdsDescriptorFactory factory ) {
			if(str==null) throw new ArgumentNullException("str");
			var match = SequencePattern.Match(str);
			var pointerParser = new PointerParser();
			while(true) {
				if(!match.Success) {
					throw new FormatException("Invalid MDS sequence format, '"+str+"'");
				}
				var ptrs = match.Groups["ptrs"].Value.Split(',').Select(t => pointerParser.Parse(t)).ToArray();
				factory.Add(ptrs[0], ptrs.Skip(1).Take(ptrs.Length - 2).ToArray(), ptrs[ptrs.Length - 1]);
				if( match.Index+match.Length==str.Length ) break;
				match = match.NextMatch();
			}
			return factory.Create();
		}
		private class PointerParser {
			private int nextFreePointer = Pointer.End-1;
			private readonly Dictionary<char, int> assignments = new Dictionary<char, int>();
			int Assign(char v) {
				if(!assignments.ContainsKey(v)) {
					assignments[v] = nextFreePointer--;
				}
				return assignments[v];
			}
			public int Parse(string p) {
				var negative = p[0] == '-';
				int ptr;
				switch (p[negative?1:0]) {
					case 'b':
						ptr = Pointer.Start;
						break;
					case 'e':
						ptr = Pointer.End;
						break;
					default:
						if(Char.IsLetter(p, negative?1:0)) {
							ptr = Assign(p[negative?1:0]);
						} else {
							ptr = int.Parse(p.Substring(negative ? 1 : 0));
						}
						break;
				}
				return negative?-ptr:ptr;
			}
		}
	}
}
