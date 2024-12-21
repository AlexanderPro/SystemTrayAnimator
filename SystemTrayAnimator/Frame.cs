using System;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;

namespace SystemTrayAnimator
{
    class Frame : IDisposable
    {
        public string FileName { get; set; }

        public string FileHash { get; set; }

        public Icon Icon { get; set; }

        public static Frame Parse(string fullFileName)
        {
            try
            {
                var fileName = Path.GetFileName(fullFileName);
                using var fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.ReadWrite);
                using var sha256 = SHA256.Create();
                var hashBytes = sha256.ComputeHash(fileStream);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                fileStream.Position = 0;
                using var bitmap = new Bitmap(fileStream);
                var iconHandle = bitmap.GetHicon();
                var icon = Icon.FromHandle(iconHandle);
                return new Frame
                {
                    FileName = fileName,
                    FileHash = hashString,
                    Icon = icon
                };
            }
            catch
            {
                return null;
            }
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return Equals((Frame)other);
        }

        public bool Equals(Frame other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            if (string.Compare(FileName, other.FileName, StringComparison.CurrentCultureIgnoreCase) != 0 ||
                string.Compare(FileHash, other.FileHash, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            hashCode ^= FileName.GetHashCode();
            hashCode ^= FileHash.GetHashCode();
            return hashCode;
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
