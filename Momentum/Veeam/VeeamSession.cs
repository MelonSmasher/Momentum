using System;

namespace Momentum.Veeam {
	public class VeeamSession {
		public string Name { get; set; }
		public string JobName { get; set; }
		public string Id { get; set; }
		public string Result { get; set; }
		public string Bottleneck { get; set; }
		public string RateHuman { get; set; }
		public string ProcessedHuman { get; set; }
		public string ReadHuman { get; set; }
		public string TransferredHuman { get; set; }
		public string JobSourceType { get; set; }
		public string JobType { get; set; }
		public string StartTimeLocal { get; set; }
		public string StopTimeLocal { get; set; }
		public string StartTimeUtc { get; set; }
		public string StopTimeUtc { get; set; }
		public string Duration { get; set; }
		public int SuccessCount { get; set; }
		public int WarningCount { get; set; }
		public int FailureCount { get; set; }
		public int TotalObjects { get; set; }
		public int ProcessedObjects { get; set; }
		public int DedupRatio { get; set; }
		public int CompressRatio { get; set; }
		public int Days { get; set; }
		public int Hours { get; set; }
		public int Milliseconds { get; set; }
		public int Minutes { get; set; }
		public int Seconds { get; set; }
		public long BackupSize { get; set; }
		public long DataSize { get; set; }
		public long BackedUpSize { get; set; }
		public long BackupTotalSize { get; set; }
		public long ReadSize { get; set; }
		public long ReadedAverageSize { get; set; }
		public long TransferedSize { get; set; }
		public long ProcessedDelta { get; set; }
		public long ProcessedUsedDelta { get; set; }
		public long ReadDelta { get; set; }
		public long ReadedAverageDelta { get; set; }
		public long TransferedDelta { get; set; }
		public long AvgSpeed { get; set; }
		public long TotalSize { get; set; }
		public long TotalUsedSize { get; set; }
		public long TotalSizeDelta { get; set; }
		public long Ticks { get; set; }

		public VeeamSession() { }
	}
}