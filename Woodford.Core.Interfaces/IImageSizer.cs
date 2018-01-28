using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.Interfaces {
    public interface IImageSizer {

        bool IsImage(string extension);

        byte[] ImageResize(byte[] fileContent, int? width, int? height, int? crop, int? upscale);

    }
}
