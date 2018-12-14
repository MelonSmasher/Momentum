using System.Collections.Generic;
using CommandLine;
using System;

namespace Momentum
{
    internal class Cli
    {
        [Verb("add-service", HelpText = "Add service accounts.")]
        private class AddServiceOptions
        {
        }

        [Verb("del-service", HelpText = "Delete service accounts.")]
        private class DelServiceOptions
        {
        }

        [Verb("run", HelpText = "Used by the Momentum Launcher to read data files.")]
        private class RunOptions
        {
            [Option('i', "input", Required = true, HelpText = "Input files to be processed.")]
            public IEnumerable<string> InputFiles { get; set; }
        }

        private static int RunAndReturnExitCode(RunOptions opts)
        {
            foreach (var file in opts.InputFiles)
            {
                Console.WriteLine("Reading: " + file);
            }

            return 0;
        }

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<RunOptions, AddServiceOptions, DelServiceOptions>(args)
                .MapResult(
                    (RunOptions opts) => RunAndReturnExitCode(opts),
                    errs => 1
                );
        }
    }
}