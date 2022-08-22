using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Water.Utils
{
    public static class InterfaceUtils
    {
        public static Point CalculateAspectRatioMaintainingFill(Rectangle parentPosition, Rectangle childPosition)
        {
            float parentAspectRatio = parentPosition.Width / parentPosition.Height;
            float childAspectRatio = childPosition.Width / childPosition.Height;

            float scalingFactor;
            if (parentAspectRatio > childAspectRatio) scalingFactor = parentPosition.Width / childPosition.Width;
            else scalingFactor = parentPosition.Height / childPosition.Height;

            return new((int)(childPosition.Width * scalingFactor), (int)(childPosition.Height * scalingFactor));
        }

        public static void OpenURL(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
    }
}
