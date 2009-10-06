using System;
using Domain;
using System.IO;

namespace ConsoleApp {
	class Program {
		static int Main(string[] args) {

			var opts = new ConsoleOptions(args);
			if(opts.ParameterCount>1) {
				opts.Help();
				return 1;
			}

			var counts = new CountingRecorder();
			var recorder = counts;
			//var logger = new TraceLogger(counts,Console.Out);

			var assembler = new StrategyEnumerator(recorder);
			if(!(opts.ld||opts.hi||opts.dlad)) {
				assembler.LdIsAllowed = true;
				assembler.HiIsAllowed = true;
				assembler.DladIsAllowed = true;
			} else {
				assembler.LdIsAllowed = opts.ld;
				assembler.HiIsAllowed = opts.hi;
				assembler.DladIsAllowed = opts.dlad;
			}
			string descriptorString;
			if(opts.ParameterCount==1) {
				descriptorString = opts.Parameters[0] as string;
			} else if(!String.IsNullOrEmpty(opts.file)) {
				descriptorString = File.ReadAllText(opts.file);
			} else {
				descriptorString = Console.ReadLine();
			}
			assembler.Enumerate(MdsDescriptorParser.Parse(descriptorString));

			Console.Out.WriteLine("Successful: {0}", counts.Successful);
			Console.Out.WriteLine("Total:      {0}", counts.Total);

			return 0;
		}
	}
}
