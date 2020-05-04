using PoshCode.Pansies;
using PoshCode.Pansies.ColorSpaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text;

namespace FIGlet.Commands
{    
    [Cmdlet(VerbsCommunications.Write, "Figlet")]
    public class WriteFigletCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Message { get; set; }

        [Parameter()]
        public string ColorChars { get; set; }

        [Parameter()]
        [Alias("Color")]
        public RgbColor[] Foreground { get; set; }

        [Parameter()]
        public RgbColor[] Background { get; set; }

        [Parameter()]
        [ArgumentCompleter(typeof(FontNameCompleter))]
        public string Font { get; set; }

        [Parameter()]
        public LayoutRule LayoutRule { get; set; } = LayoutRule.Smushing;

        [ValidateSet("HSL", "LCH", "RGB", "LAB", "XYZ")]
        [Parameter()]
        public string Colorspace { get; set; } = "LAB";

        protected override void EndProcessing()
        {
            FIGfont font = null;
            if (!string.IsNullOrEmpty(Font))
            {
                font = FIGfontReference.GetFIGfont(Font);
            }

            if (font == null)
            {
                var first = FIGfontReference.Integrated.First();
                if (!string.IsNullOrEmpty(Font))
                {
                    WriteWarning($"Can't find a font with the name '{Font}' font. Using default font '{first.Name}'");
                }
                font = first.LoadFont();
            }

            var driver = new FIGdriver(font);
                driver.LayoutRule = LayoutRule;
                driver.Write(Message);

            var output = new PSObject(driver);
            output.Properties.Add(new PSNoteProperty("Foreground", Foreground));
            output.Properties.Add(new PSNoteProperty("Background", Background));
            output.Properties.Add(new PSNoteProperty("Colorspace", Colorspace));
            output.Properties.Add(new PSNoteProperty("ColorChars", ColorChars));
            output.Properties.Add(new PSNoteProperty("Message", Message));

            WriteObject(driver);
                base.EndProcessing();
        }

        private class FontNameCompleter : IArgumentCompleter
        {
            FIGfontReference[] Fonts;

            public FontNameCompleter()
            {
                Fonts = FIGfontReference.FindFIGfonts().ToArray();
            }

            public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
            {
                return Fonts.Where(font => font.Name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase)).Select(font => new CompletionResult(font.Name));
            }
        }
    }
}
