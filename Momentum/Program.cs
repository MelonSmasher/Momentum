using System.Collections.Generic;
using CommandLine;
using System;
using Momentum.Services;

namespace Momentum {
	internal class Cli {
		/// <summary>
		/// Add services verb
		/// </summary>
		[Verb("add", HelpText = "Add service accounts.")]
		private class AddServiceOptions { }

		/// <summary>
		/// Delete services verb
		/// </summary>
		[Verb("del", HelpText = "Delete service accounts.")]
		private class DelServiceOptions { }

		/// <summary>
		/// Run verb
		/// </summary>
		[Verb("run", HelpText = "Used by the Momentum Launcher to read data files.")]
		private class RunOptions {
			[Option('i', "input", Required = true, HelpText = "Input files to be processed.")]
			public IEnumerable<string> InputFiles { get; set; }
		}

		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args) {
			Parser.Default.ParseArguments<RunOptions, AddServiceOptions, DelServiceOptions>(args).MapResult(
				(RunOptions opts) => Run(opts),
				(AddServiceOptions opts) => AddService(opts),
				(DelServiceOptions opts) => DelService(opts),
				errs => 1
			);
		}

		/// <summary>
		/// Method used for running actions
		/// </summary>
		/// <param name="opts"></param>
		/// <returns></returns>
		private static int Run(RunOptions opts) {
			foreach (var file in opts.InputFiles) {
				Console.WriteLine("Reading: " + file);
			}

			return 0;
		}

		/// <summary>
		/// Method used for adding a new service
		/// </summary>
		/// <param name="opts"></param>
		/// <returns></returns>
		private static int AddService(AddServiceOptions opts) {
			var serviceManager = new ServiceManager();

			return serviceManager.AddService();
		}

		/// <summary>
		/// Method used for deleting a service
		/// </summary>
		/// <param name="opts"></param>
		/// <returns></returns>
		private static int DelService(DelServiceOptions opts) {
			var serviceManager = new ServiceManager();

			return serviceManager.DelService();
		}
	}
}