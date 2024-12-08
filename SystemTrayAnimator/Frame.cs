using System.IO;
using System.Drawing;

namespace SystemTrayAnimator
{
    class Frame
    {
        public string FileName { get; set; }

        public string FileHash { get; set; }

        public Icon Icon { get; set; }

        public string IconTitle { get; set; }

        public int IntervalForShow { get; set; }

        public static Frame Parse(string fullFileName)
        {
            var fileName = Path.GetFileName(fullFileName);
            return new Frame
            {
                FileName = fileName
            };
        }
    }
}
