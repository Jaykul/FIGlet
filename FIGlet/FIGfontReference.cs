﻿// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fonts;

    /// <summary>
    /// A reference to a fig font... Somewhere
    /// </summary>
    public abstract class FIGfontReference
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Loads the font.
        /// </summary>
        /// <returns></returns>
        public abstract FIGfont LoadFont();

        /// <summary>
        /// Initializes a new instance of the <see cref="FIGfontReference"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected FIGfontReference(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Parses an assembly for fonts related to a sibling type.
        /// Type and embedded resources must be in the same project folder for this to work.
        /// </summary>
        /// <param name="siblingType">Type of the sibling.</param>
        /// <returns></returns>
        public static IEnumerable<FIGfontReference> Parse(Type siblingType)
        {
            var prefix = siblingType.Namespace + ".";
            foreach (var resourcePath in siblingType.Assembly.GetManifestResourceNames())
            {
                if (!resourcePath.StartsWith(prefix))
                    continue;

                var resourceName = resourcePath.Substring(prefix.Length);
                if (!FIGfont.CanHandleExtension(resourceName))
                    continue;

                yield return new EmbeddedFIGfontReference(resourceName, siblingType, Path.GetFileNameWithoutExtension(resourceName));
            }
        }

        /// <summary>
        /// Parses the specified directory for fonts.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="recurse">if set to <c>true</c> recurse.</param>
        /// <returns></returns>
        public static IEnumerable<FIGfontReference> Parse(string directory, bool recurse)
        {
            var entriesInDirectory = from e in Directory.GetFiles(directory)
                                     where FIGfont.CanHandleExtension(e)
                                     select new FileFIGfontReference(e, Path.GetFileNameWithoutExtension(e))
                                     as FIGfontReference;
            if (recurse)
                entriesInDirectory = entriesInDirectory.Concat(Directory.GetDirectories(directory).SelectMany(d => Parse(d, true)));
            return entriesInDirectory;
        }

        public static IEnumerable<FIGfontReference> FindFIGfonts()
        {
            var localPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            foreach (FIGfontReference font in Parse(typeof(FontsRoot)))
                yield return font;

            foreach (FIGfontReference font in Parse(localPath, true))
                yield return font;
        }

        public static FIGfont GetFIGfont(string name)
        {
            return FindFIGfonts().Where(font => font.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.LoadFont();
        }

        private static IList<FIGfontReference> _integrated;

        /// <summary>
        /// Gets the integrated fonts references.
        /// </summary>
        /// <value>
        /// The integrated.
        /// </value>
        public static IList<FIGfontReference> Integrated
        {
            get
            {
                if (_integrated is null)
                    _integrated = Parse(typeof(FontsRoot)).ToArray();
                return _integrated;
            }
        }
    }
}