param(
    [ValidateNotNullOrEmpty()] [Parameter(Mandatory)] [String] $PadlockToken,
    [ValidateNotNullOrEmpty()] [Parameter(Mandatory)] [String] $AzureApiUrl

)

$ErrorActionPreference = "Stop"

# Connect to CRM
if ((-not $conn) -or ($conn.GetType().Name -ne "CrmServiceClient")) {
    $conn = (Connect-CrmOnlineDiscovery -InteractiveMode)
}

# Set up Config record
$configRecordName = "Default config record"
$existingConfigRecords = Get-CrmRecords -conn $conn -EntityLogicalName msnfp_configuration -FilterAttribute msnfp_defaultconfiguration -FilterOperator eq -FilterValue $true -WarningAction SilentlyContinue
if ($existingConfigRecords.Count -gt 1) {
    Write-Host -ForegroundColor Red "Only one Configuration record with 'Default Configuration' set to true is allowed. Delete extra such records before continuing."
    Exit 1
}
elseif ($existingConfigRecords.Count -eq 1) {
    $configRecordId = $existingConfigRecords.CrmRecords[0].EntityReference.Id
    Write-Host "Config record '$configRecordName' already exists with ID $configRecordId"
} else {
    $configRecordId = New-CrmRecord -conn $conn -EntityLogicalName msnfp_configuration -Fields @{
        msnfp_identifier = $configRecordName;
        msnfp_defaultconfiguration = $true;
        msnfp_apipadlocktoken = ($PadlockToken);
        msnfp_azure_webapiurl = $AzureApiUrl;
        msnfp_showapierrorresponses = New-Object Microsoft.Xrm.Sdk.OptionSetValue(844060000); # Yes
        msnfp_sche_enablerecurringdonation = $true;
        msnfp_tran_donation = $true;
        msnfp_sche_pledgeschedule = $true;
        msnfp_tran_softcredit = $true;
        msnfp_tran_nonreceiptable = $true;
        msnfp_tran_cash = $true;
        msnfp_tran_cheque = $true;
        msnfp_tran_creditcard = $true;
        msnfp_tran_extcreditcard = $true;
        msnfp_tran_enablegiftaid = $true;
        msnfp_tran_forcefullrefund = $true;
        msnfp_tran_inkind = $true;
        msnfp_tran_property = $true;
        msnfp_tran_stock = $true;
        msnfp_tran_wireortransfer = $true;
        msnfp_transaction_other = $true;
        msnfp_event_credit = $true;
    }
    Write-Host "Created Config record '$configRecordName' ($configRecordId)"
}

# Assign Config record
$userId = (Get-MyCrmUserId)
Set-CrmRecord -conn $conn -EntityLogicalName "systemuser" -Id $userId -Fields @{
    msnfp_configurationid = (New-CrmEntityReference -Id $configRecordId -EntityLogicalName msnfp_configuration);
}
Write-Host "Assigned Config record $configRecordId to current user $userId"

# Create payment processor
$paymentProcessorName = "TEST Moneris processor"
$existingPaymentProcessors = Get-CrmRecords -conn $conn -EntityLogicalName msnfp_paymentprocessor -FilterAttribute msnfp_name -FilterOperator eq -FilterValue $paymentProcessorName -WarningAction SilentlyContinue
if ($existingPaymentProcessors.Count -gt 0) {
    Write-Host "Payment processor '$paymentProcessorName' already exists"
    $paymentProcessorId = $existingPaymentProcessors.CrmRecords[0].EntityReference.Id
} else {
    $paymentProcessorId = New-CrmRecord -conn $conn -EntityLogicalName msnfp_paymentprocessor -Fields @{
        msnfp_name = $paymentProcessorName;
        msnfp_paymentgatewaytype = New-Object Microsoft.Xrm.Sdk.OptionSetValue(844060000); # Moneris
        msnfp_apikey = "yesguy";
        msnfp_storeid = "store5";
        msnfp_testmode = $true;
        msnfp_avsvalidation = $false;
    }
    Write-Host "Created Payment Processor '$paymentProcessorName' ($paymentProcessorId)"
}

# Associate Payment Processor with Config record
Set-CrmRecord -conn $conn -EntityLogicalName "msnfp_configuration" -Id $configRecordId -Fields @{
    msnfp_paymentprocessorid = (New-CrmEntityReference -Id $paymentProcessorId -EntityLogicalName msnfp_paymentprocessor);
}
Write-Host "Assigned Payment Processor $paymentProcessorId to Config record $configRecordId"

# Copy transactioncurrency entities to SQL Azure
$currencies=(Get-CrmRecords -conn $conn -EntityLogicalName transactioncurrency -Fields *)
ForEach ($currency in $currencies.CrmRecords) {
    Set-CrmRecord -conn $conn -EntityLogicalName "transactioncurrency" -Id $currency.transactioncurrencyid -Fields @{
        currencyname = $currency.currencyname;
    }
}

# Verify Configuration record and currencies in Azure
$headers = @{'Padlock'=$PadlockToken}
$getConfigurationUrl = -join("$AzureApiUrl", "Configuration/", $configRecordId)
$result = Invoke-RestMethod -Uri $getConfigurationUrl -Headers $headers
if ($result.ConfigurationId -ne $configRecordId) {
    Write-Host -ForegroundColor Red "Configuration recored $configRecordId was not successfully copied to Azure"
    Exit 1
}
ForEach ($currency in $currencies.CrmRecords) {
    $getCurrencyUrl = -join("$AzureApiUrl", "TransactionCurrency/", $currency.transactioncurrencyid)
    $azureCurrencyCopy = Invoke-RestMethod -Uri $getCurrencyUrl -Headers $headers
    if ($currency.transactioncurrencyid -ne $azureCurrencyCopy.TransactionCurrencyId) {
        Write-Host -ForegroundColor Red "Currency '$($currency.currencyname)' with id $($currency.transactioncurrencyid) was not successfully copied to Azure."
        Exit 1
    }
}

# Finish
Write-Host -ForegroundColor Green "Environment setup was successful"


