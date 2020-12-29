
# Connect to CRM
if ((-not $conn) -or ($conn.GetType().Name -ne "CrmServiceClient")) {
    $conn = (Connect-CrmOnlineDiscovery -InteractiveMode)
}

$lastname = "TestLastName123"
$campaignName = "TestCampaign"

Write-Host "This script will delete test entities: Contact '* $lastname', Campaign '$campaignName', and their associated Transactions"
Write-Host "Default Configuration and Payment Processor won't be affacted."
$ignored = Read-Host -Prompt "Press Enter to continue or CTRL+C to quit" 

$existingContacts = Get-CrmRecords -conn $conn -EntityLogicalName contact -FilterAttribute lastname -FilterOperator eq -FilterValue $lastname -WarningAction SilentlyContinue 
ForEach ($contact in $existingContacts.CrmRecords) {
    $transactions = Get-CrmRecords -conn $conn -EntityLogicalName msnfp_transaction -FilterAttribute msnfp_customerid -FilterOperator eq -FilterValue $contact.contactid -WarningAction SilentlyContinue
    ForEach ($transaction in $transactions.CrmRecords) {
        Write-Host "Deleting Transaction $($transaction.msnfp_transactionid)"
        Remove-CrmRecord -conn $conn -EntityLogicalName msnfp_transaction -Id $transaction.msnfp_transactionid
    }
    Write-Host "Deleting Contact $($contact.contactid)"
    Remove-CrmRecord -conn $conn -EntityLogicalName contact -Id $contact.contactid
}
    

$existingCampaigns = Get-CrmRecords -conn $conn -EntityLogicalName campaign -FilterAttribute name -FilterOperator eq -FilterValue $campaignName -WarningAction SilentlyContinue
ForEach ($campaign in $existingCampaigns.CrmRecords) {
    Write-Host "Deleting Campaign $($campaign.campaignid)"
    Remove-CrmRecord -conn $conn -EntityLogicalName campaign -Id $campaign.campaignid
}
    
Write-Host -ForegroundColor Green "Cleanup successful"