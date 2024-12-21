using System;
using System.IO;
using System.Drawing;

namespace SystemTrayAnimator
{
    class Frame : IDisposable
    {
        public string FileName { get; set; }

        public Icon Icon { get; set; }

        public static Frame Parse(string fullFileName)
        {
            try
            {
                var fileName = Path.GetFileName(fullFileName);
                using var fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.ReadWrite);
                using var bitmap = new Bitmap(fileStream);
                var iconHandle = bitmap.GetHicon();
                var icon = Icon.FromHandle(iconHandle);
                return new Frame
                {
                    FileName = fileName,
                    Icon = icon
                };
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Icon?.Dispose();
            }
        }

        ~Frame()
        {
            Dispose(false);
        }
    }
}
