#if ((-not $conn) -or ($conn.GetType().Name -ne "CrmServiceClient")) {
    $conn = (Connect-CrmOnlineDiscovery -InteractiveMode)
#}
$userId = Get-CrmCurrentUserId
$records = (Get-CrmRecords -conn $conn -EntityLogicalName msnfp_bankrun -Fields *).CrmRecords
ForEach ($record in $records) {
  Set-CrmRecord -conn $conn -EntityLogicalName msnfp_bankrun -Id $record.msnfp_bankrunid -Fields @{
  }
}

msnfp_designation
msnfp_eventtables
msnfp_giftaiddeclaration
msnfp_membershipgroup
msnfp_paymentprocessor
msnfp_preferencecategory
msnfp_registrationpreference
q
transactioncurrency
msnfp_tributeormemory
msnfp_configuration
msnfp_paymentmethod
msnfp_eventpreference
msnfp_preference
msnfp_membershipcategory
msnfp_event
msnfp_receiptstack
msnfp_membership
msnfp_membershiporder
msnfp_eventdisclaimer
msnfp_eventdonation
msnfp_eventpackage
msnfp_eventproduct
msnfp_eventsponsor
msnfp_eventsponsorship
msnfp_eventticket
msnfp_receiptlog
msnfp_paymentschedule
msnfp_product
msnfp_ticket
msnfp_sponsorship
msnfp_registration
msnfp_transaction
account
$records = (Get-CrmRecords -conn $conn -EntityLogicalName contact -Fields *).CrmRecords
ForEach ($record in $records) {
  Set-CrmRecord -conn $conn -EntityLogicalName contact -Id $record.contactid -Fields @{
    firstname = $record.firstname;
  }
}

msnfp_refund
msnfp_response