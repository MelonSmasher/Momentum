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
# Create a new tmp file
$tmp = New-TemporaryFile;
# Convert the session to a JSON object and send it to the tmp file
ConvertTo-Json -Compress -InputObject $session | Out-File $tmp.FullName;
# Send the temp file path to our binary
###