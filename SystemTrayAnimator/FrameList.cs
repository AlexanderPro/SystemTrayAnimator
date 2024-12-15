using System;
using System.Collections.Generic;

namespace SystemTrayAnimator
{
    class FrameList : List<Frame>
    {
        public FrameList(params string[] fullFileNames)
        {
            foreach (var fullFileName in fullFileNames)
            {
                var frame = Frame.Parse(fullFileName);
                if (frame != null)
                {
                    Add(frame);
                }
            }
        }
    }
}
