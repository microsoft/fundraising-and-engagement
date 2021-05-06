param(
    [ValidateNotNullOrEmpty()] [Parameter(Mandatory)] [String] $MigrationName
)
cd $PSScriptRoot
dotnet ef migrations add $MigrationName --context PaymentContext --project Data.Migrations --startup-project API