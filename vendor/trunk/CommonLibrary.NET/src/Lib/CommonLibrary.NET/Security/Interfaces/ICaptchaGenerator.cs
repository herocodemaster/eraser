/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace CommonLibrary
{

    /// <summary>
    /// Interface for the captcha generator.
    /// </summary>
    public interface ICaptchaGenerator
    {
        /// <summary>
        /// Generates the specified random text.
        /// </summary>
        /// <param name="randomText">The random text.</param>
        /// <returns></returns>
        Bitmap Generate(string randomText);


        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        CaptchaGeneratorSettings Settings { get; set; }
    }




    /// <summary>
    /// Settings for the random text generator.
    /// </summary>
    public class CaptchaGeneratorSettings
    {
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }


        /// <summary>
        /// Gets or sets the allowed chars.
        /// </summary>
        /// <value>The allowed chars.</value>
        public int Width { get; set; }


        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>The font.</value>
        public string Font { get; set; }
    }
}
