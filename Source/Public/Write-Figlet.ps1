using namespace PoshCode.Pansies
function Write-Figlet {
    <#
        .SYNOPSIS
            Get a text ASCII art "FIGlet" with formatting on it so it prints with colors
    #>
    [CmdletBinding()]
    param(
        # The text you want to write
        [Parameter(Mandatory)]
        [string]$Message,

        # The font you want to use (one of the figlet fonts, see http://www.figlet.org/examples.html)
        [Parameter()]
        [ArgumentCompleter([FIGlet.Commands.GetFigletCommand+FontNameCompleter])]
        [string]$Font,

        # The LayoutRule: FullSize, Fitting, or Smushing
        [Parameter()]
        [Figlet.LayoutRule]$LayoutRule = "Smushing",

        # A regex character list of chacters which will be colored. Defaults to coloring all characters.
        [Parameter()]
        [string]$ColorChars,

        # One or two colors for the foreground. Note that only characters matching the ColorChars pattern will be colored.
        # - If you provide one color, it will be used. The default is the default PowerShell foreground color.
        # - If you provide two, a gradient between them will be used.
        # - For more than two colors, a gradient between the first and last will be used (everything extra is ignored)
        [Parameter()]
        [Alias("Color")]
        [RgbColor[]]$Foreground,

        # One or two colors for the background. Note that the whole background will be colored
        # - If you provide one color, it will be used. The default is the default PowerShell background color.
        # - If you provide two, a gradient between them will be used.
        # - For more than two colors, a gradient between the first and last will be used (everything extra is ignored)
        [Parameter()]
        [RgbColor[]]$Background,

        # The color space controls how gradients come out (see PANSIES' Get-Gradient)
        [ValidateSet("HSL", "LCH", "RGB", "LAB", "XYZ")]
        [Parameter()]
        [string]$Colorspace = "LAB"
    )
    $Figlet = @{ Message = $Message }
    if ($PSBoundParameters.ContainsKey("Font")) {
        $null = $PSBoundParameters.Remove("Font")
        $Figlet.Font = $Font
        if ($Font -eq "Mirror") {
            $Reverseable = $Message.ToCharArray()
            [array]::Reverse($Reverseable)
            $Figlet["Message"] = New-Object String (, [char[]]$Reverseable)
        }
    }
    if ($PSBoundParameters.ContainsKey("LayoutRule")) {
        $null = $PSBoundParameters.Remove("LayoutRule")
        $Figlet.LayoutRule = $LayoutRule
    }

    Get-Figlet @Figlet |
        Add-Member -NotePropertyMembers $PSBoundParameters -PassThru
}