# Powershell script to run DB migrations
$webConfig = "$PSScriptRoot\Web.config"
$binDir = "$PSScriptRoot\bin"

.\bin\migrate.exe ContosoUniversity.dll /startupConfigurationFile=$webConfig /StartUpDirectory=$binDir /verbose
