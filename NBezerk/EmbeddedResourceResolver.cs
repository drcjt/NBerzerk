using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace NBezerk
{
    public class EmbeddedResourceResolver : IContentResolver
    {
        public System.IO.Stream Resolve(string assetName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(assetName);
        }
    }
}
