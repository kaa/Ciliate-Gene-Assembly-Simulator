using System;
using Domain;

namespace ConsoleApp {
	class Program {
		static int Main(string[] args) {

			var opts = new ConsoleOptions(args);
			if(opts.ParameterCount!=1) {
				opts.Help();
				return 1;
			}

			var counts = new CountingLogger();
			var logger = new TraceLogger(counts,Console.Out);

			var assembler = new GeneAssembler(logger);
			if(!(opts.ld||opts.hi||opts.dlad)) {
				assembler.LdIsAllowed = true;
				assembler.HiIsAllowed = true;
				assembler.DladIsAllowed = true;
			} else {
				assembler.LdIsAllowed = opts.ld;
				assembler.HiIsAllowed = opts.hi;
				assembler.DladIsAllowed = opts.dlad;
			}
			assembler.Assemble(MdsDescriptorParser.Parse(opts.Parameters[0] as string));

			Console.Out.WriteLine("Successful: {0}", counts.Successful);
			Console.Out.WriteLine("Total:      {0}", counts.Total);

			return 0;
		}
	}
}
