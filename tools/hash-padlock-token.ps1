Param (
    [Parameter(Mandatory=$true)]
    [string]
    $clearTextPadlock
)

# Hash the Padlock value with the sha256 algorithm.
$hasher = [System.Security.Cryptography.HashAlgorithm]::Create('sha256')
$hashedPadlock = $hasher.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($clearTextPadlock))
$hashedPadlockString = [System.BitConverter]::ToString($hashedPadlock)
$hashedPadlockString = $hashedPadlockString.Replace('-', '')

Write-Host ""
"    " + $hashedPadlockString
Write-Host ""
