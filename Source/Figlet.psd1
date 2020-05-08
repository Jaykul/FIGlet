@{
    # Version number of this module.
    ModuleVersion        = '0.0.1'
    PrivateData          = @{
        PSData = @{
            # The prerelease portion of a semantic version. Blank for releases
            Prerelease   = ''
            ReleaseNotes = ''

            Tags         = @('Figlet', 'ANSI', 'PANSIES', 'EzTheme')

            # LicenseUri = ''
            # ProjectUri = ''
            # IconUri = ''

            # Modules that aren't in the same PowerShellGallery
            # ExternalModuleDependencies = @()
        } # End of PSData hashtable
    } # End of PrivateData hashtable

    Description          = 'Figlet ANSI Text generator'
    FunctionsToExport    = @()
    CmdletsToExport      = @("Write-Figlet")
    VariablesToExport    = @()
    AliasesToExport      = @()
    FormatsToProcess     = @('Figlet.format.ps1xml')
    NestedModules        = @('lib\Figlet.dll')
    RequiredModules      = @(@{ModuleName = "PANSIES"; ModuleVersion = "2.0.0" })

    # Script module or binary module file associated with this manifest.
    RootModule           = 'Figlet.psm1'
    GUID                 = '5689d5ac-ec72-455a-bce5-a82e32a9255a'
    Author               = "Joel 'Jaykul' Bennett"
    CompanyName          = 'PoshCode.org'
    Copyright            = '(c) Joel Bennett 2020. All rights reserved.'
    CompatiblePSEditions = @('Desktop', 'Core')

    # Minimum version of the PowerShell engine required by this module
    PowerShellVersion    = '5.1'

    # HelpInfoURI = ''
}

