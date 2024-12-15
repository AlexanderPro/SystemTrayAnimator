using System;
using System.IO;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Settings
{
    public class ApplicationSettings : ICloneable
    {
        private const string DefaultDirectoryName = "Icons";
        private const string DefaultPauseFileName = "Pause.txt";
        private const string DefaultSupportedFileExtensions = "*.ico";

        private const int DeafultIntervalBetweenFrames = 200;
        private const int DeafultIntervalForShowOneFrame = 0;

        public string DirectoryName { get; set; }

        public string PauseFileName { get; set; }

        public string SupportedFileExtensions { get; set; }

        public int IntervalBetweenFrames { get; set; }

        public int IntervalForShowOneFrame { get; set; }

        public bool IsPaused { get; set; }

        public ApplicationSettings()
        {
            DirectoryName = Path.Combine(AssemblyUtils.AssemblyDirectory, DefaultDirectoryName);
            PauseFileName = Path.Combine(AssemblyUtils.AssemblyDirectory, DefaultPauseFileName);
            SupportedFileExtensions = DefaultSupportedFileExtensions;
            IntervalBetweenFrames = DeafultIntervalBetweenFrames;
            IntervalForShowOneFrame = DeafultIntervalForShowOneFrame;
        }

        public object Clone() => new ApplicationSettings
        {
            DirectoryName = DirectoryName,
            PauseFileName = PauseFileName,
            IntervalBetweenFrames = IntervalBetweenFrames,
            IntervalForShowOneFrame = IntervalForShowOneFrame
        };

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

            return Equals(other as ApplicationSettings);
        }

        public bool Equals(ApplicationSettings other)
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

            if (string.Compare(DirectoryName, other.DirectoryName, StringComparison.CurrentCultureIgnoreCase) != 0 ||
                string.Compare(PauseFileName, other.PauseFileName, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return false;
            }

            if (IntervalBetweenFrames != other.IntervalBetweenFrames || IntervalForShowOneFrame != other.IntervalForShowOneFrame)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            hashCode ^= DirectoryName.GetHashCode();
            hashCode ^= PauseFileName.GetHashCode();
            hashCode ^= IntervalBetweenFrames.GetHashCode();
            hashCode ^= IntervalForShowOneFrame.GetHashCode();
            return hashCode;
        }       
    }
}
