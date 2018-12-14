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

# Get the Veeam Job
$job = Get-VeeamJob;
# Get the Veeam session
$session = Get-VeeamSession -Job $job;

# Grab and form only the data we want from the session into a flat object that matches our C# class
$momentumData = [PSCustomObject]@{
    Name                = $session.Name
    JobName             = $session.OrigJobName
    Id                  = $session.Id.Guid
    BackupSize          = $session.Info.BackedUpSize
    DataSize            = $session.BackupStats.DataSize
    DedupRatio          = $session.BackupStats.DedupRatio
    CompressRatio       = $session.BackupStats.CompressRatio
    JobSourceType       = $session.JobSourceType
    JobType             = $session.JobType
    Failures            = $session.Info.Failures
    Warnings            = $session.Info.Warnings
    Result              = $session.Info.Result
    Bottleneck          = $session.Info.Progress.BottleneckInfo.Bottleneck
    BottleneckSource    = $session.Info.Progress.BottleneckInfo.Source
    BottleneckProxy     = $session.Info.Progress.BottleneckInfo.Proxy
    BottleneckNetwork   = $session.Info.Progress.BottleneckInfo.Network
    BottleneckTarget    = $session.Info.Progress.BottleneckInfo.Target
    TotalObjects        = $session.Info.Progress.TotalObjects
    ProcessedObjects    = $session.Info.Progress.ProcessedObjects
    BackedUpSize        = $session.Info.BackedUpSize
    BackupTotalSize     = $session.Info.BackupTotalSize
    ReadSize            = $session.Info.Progress.ReadSize
    ReadedAverageSize   = $session.Info.Progress.ReadedAverageSize
    TransferedSize      = $session.Info.Progress.TransferedSize
    ProcessedDelta      = $session.Info.Progress.ProcessedDelta
    ProcessedUsedDelta  = $session.Info.Progress.ProcessedUsedDelta
    ReadDelta           = $session.Info.Progress.ReadDelta
    ReadedAverageDelta  = $session.Info.Progress.ReadedAverageDelta
    TransferedDelta     = $session.Info.Progress.TransferedDelta
    StartTimeLocal      = $session.Info.Progress.StartTimeLocal.DateTime
    StopTimeLocal       = $session.Info.Progress.StopTimeLocal.DateTime
    StartTimeUtc        = $session.Info.Progress.StartTimeUtc.DateTime
    StopTimeUtc         = $session.Info.Progress.StopTimeUtc.DateTime
    AvgSpeed            = $session.Info.Progress.AvgSpeed
    TotalSize           = $session.Info.Progress.TotalSize
    TotalUsedSize       = $session.Info.Progress.TotalUsedSize
    TotalSizeDelta      = $session.Info.Progress.TotalSizeDelta
    Duration            = $session.Info.Progress.Duration.ToString()
    Ticks               = $session.Info.Progress.Duration.Ticks
    Days                = $session.Info.Progress.Duration.Days
    Hours               = $session.Info.Progress.Duration.Hours
    Milliseconds        = $session.Info.Progress.Duration.Milliseconds
    Minutes             = $session.Info.Progress.Duration.Minutes
    Seconds             = $session.Info.Progress.Duration.Seconds
}

# Create a new tmp file
$tmp = New-TemporaryFile;
# Convert the Momentum Data to a JSON object and send it to the tmp file
ConvertTo-Json -Compress -InputObject $momentumData | Out-File $tmp.FullName;
# Send the temp file path to our binary
#
# Momentum.exe run -i $tmp.FullName
#
