using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain {
	public class MdsDescriptorParser {

		static readonly Regex SequencePattern = new Regex(@"\(?<i>-?\d+(,\[(?<c>-?\d+(,-?\d+)*)\])?,(?<o>-?\d+)\)");
		public static IMdsDescriptor Parse( string str ) {
			return Parse(str, new MdsDescriptorFactory());
		}
		public static IMdsDescriptor Parse( string str, IMdsDescriptorFactory factory ) {
			if(str==null) throw new ArgumentNullException("str");
			var match = SequencePattern.Match(str);
			while(true) {
				if(!match.Success) {
					throw new FormatException("Invalid MDS sequence format, '"+str+"'");
				}
				var inPtr = int.Parse(match.Groups["i"].Value);
				var outPtr = int.Parse(match.Groups["o"].Value);
				var content = match.Groups["c"].Success?match.Groups["c"].Value.Split(',').Select(t => int.Parse(t)).ToArray():new int[0];
				factory.Add(inPtr,content,outPtr);
				if( match.Index+match.Length==str.Length ) break;
				match = match.NextMatch();
			}
			return factory.Create();
		}
	}
}
