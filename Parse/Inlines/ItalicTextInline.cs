﻿// Copyright (c) 2016 Quinn Damerell
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.


using System.Collections.Generic;
using UniversalMarkdown.Helpers;

namespace UniversalMarkdown.Parse.Elements
{
    /// <summary>
    /// Represents a span containing italic text.
    /// </summary>
    public class ItalicTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new italic text span.
        /// </summary>
        public ItalicTextInline() : base(MarkdownInlineType.Italic)
        {
        }

        /// <summary>
        /// The contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Returns the chars that if found means we might have a match.
        /// </summary>
        /// <returns></returns>
        internal static void AddTripChars(List<Common.InlineTripCharHelper> tripCharHelpers)
        {
            tripCharHelpers.Add(new Common.InlineTripCharHelper() { FirstChar = '*', Method = Common.InlineParseMethod.Italic });
            tripCharHelpers.Add(new Common.InlineTripCharHelper() { FirstChar = '_', Method = Common.InlineParseMethod.Italic });
        }

        /// <summary>
        /// Attempts to parse a italic text span.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location to start parsing. </param>
        /// <param name="maxEnd"> The location to stop parsing. </param>
        /// <returns> A parsed italic text span, or <c>null</c> if this is not a italic text span. </returns>
        internal static Common.InlineParseResult Parse(string markdown, int start, int maxEnd)
        {
            // Check the first char.
            char startChar = markdown[start];
            if (start == maxEnd || (startChar != '*' && startChar != '_'))
                return null;

            // Find the end of the span.  The end character (either '*' or '_') must be the same as
            // the start character.
            var innerStart = start + 1;
            int innerEnd = Common.IndexOf(markdown, startChar, start + 1, maxEnd);
            if (innerEnd == -1)
                return null;

            // The span must contain at least one character.
            if (innerStart == innerEnd)
                return null;

            // The first character inside the span must NOT be a space.
            if (Common.IsWhiteSpace(markdown[innerStart]))
                return null;

            // The last character inside the span must NOT be a space.
            if (Common.IsWhiteSpace(markdown[innerEnd - 1]))
                return null;

            // We found something!
            var result = new ItalicTextInline();
            result.Inlines = Common.ParseInlineChildren(markdown, innerStart, innerEnd);
            return new Common.InlineParseResult(result, start, innerEnd + 1);
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Inlines == null)
                return base.ToString();
            return "*" + string.Join(string.Empty, Inlines) + "*";
        }
    }
}
