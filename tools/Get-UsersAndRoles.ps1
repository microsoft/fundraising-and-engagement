
$conn = (Connect-CrmOnlineDiscovery -InteractiveMode)

$userIds = (Get-CrmRecords -conn $conn -EntityLogicalName systemuser).CrmRecords | select systemuserid

ForEach ($userId in $userIds) {
    Get-CrmRecord -conn $conn -EntityLogicalName systemuser -Id $userId.systemuserid -Fields domainname | select domainname,systemuserid | Format-Table -AutoSize
    $roles = Get-CrmUserSecurityRoles -conn $conn -UserId $userId.systemuserid.Guid
    foreach ($role in $roles) {
        Write-Output "Role: $role"
    }
}

