using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain {
	public class MdsDescriptorParser {

		static readonly Regex SequencePattern = new Regex(@"\((?<in>-?\d+)(,\[(?<content>-?\d+(,-?\d+)*)\])?,(?<out>-?\d+)\)");
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
				factory.Add(
					int.Parse(match.Groups["in"].Value), 
					match.Groups["content"].Success?match.Groups["content"].Value.Split(',').Select(t => int.Parse(t)).ToArray():new int[0],
					int.Parse(match.Groups["out"].Value)
				);
				if( match.Index+match.Length==str.Length ) break;
				match = match.NextMatch();
			}
			return factory.Create();
		}
	}
}
