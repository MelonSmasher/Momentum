using System;

namespace Momentum.Veeam {
	public class VeeamSession {
		public string Name { get; set; }
		public string JobName { get; set; }
		public string Id { get; set; }
		public int SuccessCount { get; set; }
		public int WarningCount { get; set; }
		public int FailureCount { get; set; }
		public string RateHuman { get; set; }
		public string ProcessedHuman { get; set; }
		public string ReadHuman { get; set; }
		public string TransferredHuman { get; set; }
		public string JobSourceType { get; set; }
		public string JobType { get; set; }
		public int TotalObjects { get; set; }
		public int ProcessedObjects { get; set; }
		public int DedupRatio { get; set; }
		public int CompressRatio { get; set; }
		public int Failures { get; set; }
		public int Warnings { get; set; }
		public string Result { get; set; }
		public string Bottleneck { get; set; }
		public int BottleneckSource { get; set; }
		public int BottleneckProxy { get; set; }
		public int BottleneckNetwork { get; set; }
		public int BottleneckTarget { get; set; }
		public int BackupSize { get; set; }
		public int DataSize { get; set; }
		public int BackedUpSize { get; set; }
		public int BackupTotalSize { get; set; }
		public int ReadSize { get; set; }
		public int ReadedAverageSize { get; set; }
		public int TransferedSize { get; set; }
		public int ProcessedDelta { get; set; }
		public int ProcessedUsedDelta { get; set; }
		public int ReadDelta { get; set; }
		public int ReadedAverageDelta { get; set; }
		public int TransferedDelta { get; set; }
		public int AvgSpeed { get; set; }
		public int TotalSize { get; set; }
		public int TotalUsedSize { get; set; }
		public int TotalSizeDelta { get; set; }
		public string StartTimeLocal { get; set; }
		public string StopTimeLocal { get; set; }
		public string StartTimeUtc { get; set; }
		public string StopTimeUtc { get; set; }
		public string Duration { get; set; }
		public int Ticks { get; set; }
		public int Days { get; set; }
		public int Hours { get; set; }
		public int Milliseconds { get; set; }
		public int Minutes { get; set; }
		public int Seconds { get; set; }

		public VeeamSession() { }
	}
}