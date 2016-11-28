// Copyright (c) 2016 Quinn Damerell
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


using UniversalMarkdown.Helpers;

namespace UniversalMarkdown.Parse.Elements
{
    /// <summary>
    /// Represents a horizontal line.
    /// </summary>
    public class HorizontalRuleBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new horizontal rule block.
        /// </summary>
        public HorizontalRuleBlock()
            : base(MarkdownBlockType.HorizontalRule)
        {
        }

        /// <summary>
        /// Parses a horizontal rule.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location of the start of the line. </param>
        /// <param name="end"> The location of the end of the line. </param>
        /// <returns> A parsed horizontal rule block, or <c>null</c> if this is not a horizontal rule. </returns>
        internal static HorizontalRuleBlock Parse(string markdown, int start, int end)
        {
            // A horizontal rule is a line with at least 3 stars, optionally separated by spaces
            // OR a line with at least 3 dashes, optionally separated by spaces
            // OR a line with at least 3 underscores, optionally separated by spaces.
            char hrChar = '\0';
            int hrCharCount = 0;
            int pos = start;
            while (pos < end)
            {
                char c = markdown[pos++];
                if (c == '*' || c == '-' || c == '_')
                {
                    // All of the non-whitespace characters on the line must match.
                    if (hrCharCount > 0 && c != hrChar)
                        return null;
                    hrChar = c;
                    hrCharCount++;
                }
                else if (c == '\n')
                    break;
                else if (!Common.IsWhiteSpace(c))
                    return null;
            }

            // Hopefully there were at least 3 stars/dashes/underscores.
            return hrCharCount >= 3 ? new HorizontalRuleBlock() : null;
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return "---";
        }
    }
}
