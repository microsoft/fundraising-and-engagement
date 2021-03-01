
Add-Type -AssemblyName 'System.Web'
$password = [System.Web.Security.Membership]::GeneratePassword(30,3)
$passwordBytes = [System.Text.Encoding]::Unicode.GetBytes($password)
$tdes = New-Object System.Security.Cryptography.TripleDESCryptoServiceProvider
$pdb = New-Object System.Security.Cryptography.PasswordDeriveBytes @($passwordBytes, @())
$key = $pdb.CryptDeriveKey("TripleDES", "SHA1", 192, $tdes.IV);
$hashedPassword = [Convert]::ToBase64String($key)

Write-Host "Unencrypted padlock token (goes to Configuration record in F&E application): "
Write-Host -ForegroundColor Green $password

Write-Host "`nHashed padlock token (goes to Azure ARM template parameters): "
Write-Host -ForegroundColor Green $hashedPassword
