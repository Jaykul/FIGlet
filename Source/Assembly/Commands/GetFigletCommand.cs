using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text;

namespace FIGlet.Commands
{    
    [Cmdlet(VerbsCommon.Get, "Figlet")]
    public class GetFigletCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Message { get; set; }

        [Parameter()]
        [ArgumentCompleter(typeof(FontNameCompleter))]
        public string Font { get; set; }

        [Parameter()]
        public LayoutRule LayoutRule { get; set; } = LayoutRule.Smushing;

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

            WriteObject(driver);
            base.EndProcessing();
        }

        public class FontNameCompleter : IArgumentCompleter
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
