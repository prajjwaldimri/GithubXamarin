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


namespace UniversalMarkdown.Parse.Elements
{
    /// <summary>
    /// Implemented by all inline link elements.
    /// </summary>
    public interface ILinkElement
    {
        /// <summary>
        /// The link URL.  This can be a relative URL, but note that subreddit links will always
        /// have the leading slash (i.e. the Url will be "/r/baconit" even if the text is
        /// "r/baconit").
        /// </summary>
        string Url { get; }

        /// <summary>
        /// A tooltip to display on hover.  Can be <c>null</c>.
        /// </summary>
        string Tooltip { get; }
    }
}
