# A simple shim that uses the VeeamPSSnapin to grab job info.
#
# This script grabs data from Veeam about the Job that launched it.
# Converts this data to JSON and writes it out to a tmp file.
# It them launches the Momentum binary telling telling it where the JSON data is.

# Add Veeam commands
Add-PSSnapin VeeamPSSnapin

# Returns the Veeam Job from the partent process
function Get-VeeamJob()
{
    $parentpid = (Get-WmiObject Win32_Process -Filter "processid='$pid'").parentprocessid.ToString();
    $parentcmd = (Get-WmiObject Win32_Process -Filter "processid='$parentpid'").CommandLine;
    return Get-VBRJob | ?{ $parentcmd -like "*" + $_.Id.ToString() + "*" };
}

# Returns the Veeam session data from the Job
function Get-VeeamSession($job)
{
    $session = Get-VBRBackupSession | ?{ ($_.OrigJobName -eq $job.Name) -and ($parentcmd -like "*" + $_.Id.ToString() + "*") };
    # Wait for the session to finish up
    while ($session.IsCompleted -eq $false)
    {
        Start-Sleep -m 200
        $session = Get-VBRBackupSession | ?{ ($_.OrigJobName -eq $job.Name) -and ($parentcmd -like "*" + $_.Id.ToString() + "*") };
    }
    return $session;
}

# This function converts sizes to human readable formats
function Convert-BytesToHuman([int64]$Bytes)
{
    switch ($Bytes)
    {
        { $Bytes -gt 1PB } {
            return '{0:N1} PB' -f ($Bytes / 1PB)
        }
        { $Bytes -gt 1TB } {
            return '{0:N1} TB' -f ($Bytes / 1TB)
        }
        { $Bytes -gt 1GB } {
            return '{0:N1} GB' -f ($Bytes / 1GB)
        }
        { $Bytes -gt 1MB } {
            return '{0:N1} MB' -f ($Bytes / 1MB)
        }
        { $Bytes -gt 1KB } {
            return '{0:N1} KB' -f ($Bytes / 1KB)
        }
        default          {
            return '{0:N1} B ' -f $Bytes
        }
    }
}

# This function converts time to human readable formats
function Convert-TimeSpanToHuman([TimeSpan]$TimeSpan)
{
    switch ($TimeSpan)
    {
        { $TimeSpan.Days -gt 1 } {
            $Format = '{0}.{1:00}:{2:00}:{3:00}'
        }
        { $TimeSpan.Hours -gt 1 } {
            $Format = '{1:00}:{2:00}:{3:00}'
        }
        default                   {
            $Format = '{2:00}:{3:00}'
        }
    }

    $Format -f $TimeSpan.Days, $TimeSpan.Hours, $TimeSpan.Minutes, $TimeSpan.Seconds
}

# Get the Veeam Job
$job = Get-VeeamJob;
# Get the Veeam session
$session = Get-VeeamSession -Job $job;

# calculate success/warning/error counts
$TaskSessions = $Session.GetTaskSessions()
$SuccessCount = ( $TaskSessions | Where-Object Status -eq 'Success').Count
$WarningCount = ( $TaskSessions | Where-Object Status -eq 'Warning').Count
$FailureCount = ( $TaskSessions | Where-Object Status -eq 'Failed').Count

# Format some values for human eyes
$Duration = Convert-TimeSpanToHuman $Session.Progress.Duration
$Rate = '{0}/s' -f (Convert-BytesToHuman $Session.Progress.AvgSpeed)
$Processed = '{0} ({1}%)' -f (Convert-BytesToHuman $Session.Progress.ProcessedUsedSize), $Session.Info.CompletionPercentage
$Read = Convert-BytesToHuman $Session.Progress.ReadSize
$Transferred = '{0} ({1:N1}x)' -f (Convert-BytesToHuman $Session.Progress.TransferedSize), ($Session.Progress.ReadSize / $Session.Progress.TransferedSize)

# Grab and form only the data we want from the session into a flat object that matches our C# class
$momentumData = [PSCustomObject]@{
    Name = $session.Name
    JobName = $session.OrigJobName
    Id = $session.Id.Guid
    SuccessCount = $SuccessCount
    WarningCount = $WarningCount
    FailureCount = $FailureCount
    Duration = $Duration
    RateHuman = $Rate
    ProcessedHuman = Processed
    ReadHuman = $Read
    TransferredHuman = $Transferred
    BackupSize = $session.Info.BackedUpSize
    DataSize = $session.BackupStats.DataSize
    DedupRatio = $session.BackupStats.DedupRatio
    CompressRatio = $session.BackupStats.CompressRatio
    JobSourceType = $session.JobSourceType
    JobType = $session.JobType
    Failures = $session.Info.Failures
    Warnings = $session.Info.Warnings
    Result = $session.Info.Result
    Bottleneck = $session.Info.Progress.BottleneckInfo.Bottleneck
    BottleneckSource = $session.Info.Progress.BottleneckInfo.Source
    BottleneckProxy = $session.Info.Progress.BottleneckInfo.Proxy
    BottleneckNetwork = $session.Info.Progress.BottleneckInfo.Network
    BottleneckTarget = $session.Info.Progress.BottleneckInfo.Target
    TotalObjects = $session.Info.Progress.TotalObjects
    ProcessedObjects = $session.Info.Progress.ProcessedObjects
    BackedUpSize = $session.Info.BackedUpSize
    BackupTotalSize = $session.Info.BackupTotalSize
    ReadSize = $session.Info.Progress.ReadSize
    ReadedAverageSize = $session.Info.Progress.ReadedAverageSize
    TransferedSize = $session.Info.Progress.TransferedSize
    ProcessedDelta = $session.Info.Progress.ProcessedDelta
    ProcessedUsedDelta = $session.Info.Progress.ProcessedUsedDelta
    ReadDelta = $session.Info.Progress.ReadDelta
    ReadedAverageDelta = $session.Info.Progress.ReadedAverageDelta
    TransferedDelta = $session.Info.Progress.TransferedDelta
    StartTimeLocal = $session.Info.Progress.StartTimeLocal.DateTime
    StopTimeLocal = $session.Info.Progress.StopTimeLocal.DateTime
    StartTimeUtc = $session.Info.Progress.StartTimeUtc.DateTime
    StopTimeUtc = $session.Info.Progress.StopTimeUtc.DateTime
    AvgSpeed = $session.Info.Progress.AvgSpeed
    TotalSize = $session.Info.Progress.TotalSize
    TotalUsedSize = $session.Info.Progress.TotalUsedSize
    TotalSizeDelta = $session.Info.Progress.TotalSizeDelta
    Ticks = $session.Info.Progress.Duration.Ticks
    Days = $session.Info.Progress.Duration.Days
    Hours = $session.Info.Progress.Duration.Hours
    Milliseconds = $session.Info.Progress.Duration.Milliseconds
    Minutes = $session.Info.Progress.Duration.Minutes
    Seconds = $session.Info.Progress.Duration.Seconds
}

# Create a new tmp file
$tmp = New-TemporaryFile;
# Convert the Momentum Data to a JSON object and send it to the tmp file
ConvertTo-Json -Compress -InputObject $momentumData | Out-File $tmp.FullName;

# Send the temp file path to our binary
Start-Process -NoNewWindow -Wait -FilePath "$Env:Programfiles\Momentum\Momentum.exe" -ArgumentList "run", "-i", "$tmp.FullName";
