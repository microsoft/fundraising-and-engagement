
$ErrorActionPreference = "Stop"

# Connect to CRM
if ((-not $conn) -or ($conn.GetType().Name -ne "CrmServiceClient")) {
    $conn = (Connect-CrmOnlineDiscovery -InteractiveMode)
}

# Create minimum test entities
$firstname = "TestFirstName123"
$lastname = "TestLastName123"
$existingContacts = Get-CrmRecords -conn $conn -EntityLogicalName contact -FilterAttribute lastname -FilterOperator eq -FilterValue $lastname -WarningAction SilentlyContinue
if ($existingContacts.Count -ge 1) {
    $contactId = $existingContacts.CrmRecords[0].EntityReference.Id
    Write-Host "Contact '$firstname $lastname' already exists"
} else {
    $contactId = New-CrmRecord -conn $conn -EntityLogicalName contact -Fields @{
      firstname = $firstname;
      lastname = $lastname;
    }
    Write-Host "Created test Contact '$firstname $lastname'"
}

$campaignName = "TestCampaign"
$existingCampaign = Get-CrmRecords -conn $conn -EntityLogicalName campaign -FilterAttribute name -FilterOperator eq -FilterValue $campaignName -WarningAction SilentlyContinue
if ($existingCampaign.Count -ge 1) {
    $campaignId = $existingCampaign.CrmRecords[0].EntityReference.Id
    Write-Host "Campaign '$campaignName' already exists"
} else {
    $campaignid = New-CrmRecord -conn $conn -EntityLogicalName campaign -Fields @{
        name = 'TestCampaign';
    }
    Write-Host "Created test Campaign '$campaignName'"
}


# Create a payment method and a transaction
$paymentProcessorName = "TEST Moneris processor"
$paymentProcessors = Get-CrmRecords -conn $conn -EntityLogicalName msnfp_paymentprocessor -FilterAttribute msnfp_name -FilterOperator eq -FilterValue $paymentProcessorName -Fields msnfp_paymentprocessorid 
if ($paymentProcessors.Count -eq 0) {
    Write-Host -ForegroundColor Red "Could not find payment processor '$paymentProcessorName'"
    Exit 1
}
$paymentProcessorId = $paymentProcessors.CrmRecords[0].msnfp_paymentprocessorid

$paymentMethodId = New-CrmRecord -conn $conn -EntityLogicalName msnfp_paymentmethod -Fields @{
    msnfp_customerid         = (New-CrmEntityReference -Id $contactId -EntityLogicalName contact);
    msnfp_paymentprocessorid = (New-CrmEntityReference -Id $paymentProcessorId -EntityLogicalName msnfp_paymentprocessor);
    msnfp_ccexpmmyy          = "1029";
    msnfp_cclast4            = "4242424242424242";
    msnfp_firstname          = $firstname;
    msnfp_lastname           = $lastname;
    msnfp_name               = "TEST Visa - 4242";
    msnfp_nameonfile         = "$firstname $lastname";
    msnfp_type               = New-CrmOptionSetValue(844060000); # Credit card transaction
    msnfp_ccbrandcode        = New-CrmOptionSetValue(844060000); # Visa
    msnfp_isreusable         = $false 
}
Write-Host "Created payment method 'TEST Visa - xxxx'"

$user = Get-CrmRecord -conn $conn -EntityLogicalName systemuser -Id (Get-MyCrmUserId) -Fields msnfp_configurationid
$amount = New-Object Microsoft.Xrm.Sdk.Money(102.5)
$transactionId = New-CrmRecord -conn $conn -EntityLogicalName msnfp_transaction -Fields @{
    msnfp_bookdate                    = Get-Date;
    msnfp_amount                      = $amount;
    msnfp_amount_receipted            = $amount;
    msnfp_customerid                  = (New-CrmEntityReference -Id $contactid -EntityLogicalName contact);
    msnfp_originatingcampaignid       = (New-CrmEntityReference -Id $campaignid -EntityLogicalName campaign);
    msnfp_typecode                    = (New-CrmOptionSetValue(844060000));
    msnfp_dataentrysource             = (New-CrmOptionSetValue(844060000));
    msnfp_paymenttypecode             = (New-CrmOptionSetValue(844060002)); # credit card
    msnfp_firstname                   = $firstname;
    msnfp_lastname                    = $lastname;
    msnfp_transaction_paymentmethodid = (New-CrmEntityReference -id $paymentMethodId -EntityLogicalName msnfp_paymentmethod);
    msnfp_chargeoncreate              = $true;
    msnfp_configurationid             = $user.msnfp_configurationid_Property.Value;    
    statuscode= (New-CrmOptionSetValue(844060000));
}

Write-Host "Created a test gift transaction"

$createdTransaction = Get-CrmRecord -conn $conn -EntityLogicalName msnfp_transaction -Id $transactionId -Fields *
Write-Host "  Payment gateway response: "
if (-not $createdTransaction.msnfp_responseid_Property.Value.Id) {
    Write-Host -ForegroundColor Red "    No response"
} else {
    $transactionResponse = Get-CrmRecord -conn $conn -EntityLogicalName msnfp_response -Id $createdTransaction.msnfp_responseid_Property.Value.Id -Fields msnfp_response
    $transactionResponse.msnfp_response
    Write-Host -ForegroundColor Green "  Payment gateway Response received"
}
Write-Host "  Transaction status: $($createdTransaction.statuscode)"
