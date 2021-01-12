# delete-sample-data.ps1
# this is a comment

param (
    [Parameter(Mandatory=$true)][string]$sampledatafile
)

#C:\src\CRM.Solutions.Nonprofit.Fundraising\samples\FundraisingandEngagement.Sample.Data\data.xml
write-host "`nParsing file $sampledatafile`n"

[System.Xml.XmlDocument] $xd = new-object System.Xml.XmlDocument
$file = resolve-path($sampledatafile)
$xd.load($file)

# Process all entity types
$entities = $xd.selectnodes("/entities/entity")
foreach ($entity in $entities) {
  $name = $entity.getAttribute("name")

  # Process all records of this entity type
  $recordsNode = $entity.selectSingleNode("records")
  $records = $recordsNode.selectNodes("record")

  foreach ($recordNode in $records) {
    $id = $recordNode.getAttribute("id")

    # If the record exists in CDS, then remove it
    Try {
        $record = Get-CrmRecord -EntityLogicalName $name -Id $id -Fields *    
        write-host "Will delete the following record:"
        write-host $record
        Remove-CrmRecord -EntityLogicalName $name -Id $id
    }
    Catch {
        write-host "Record with type $name ID $id does not exist"
    }
  }
}