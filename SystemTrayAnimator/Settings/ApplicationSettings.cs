using System;
using System.IO;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Settings
{
    public class ApplicationSettings : ICloneable
    {
        private const string DefaultIconDirectoryName = "Icons";
        private const string DefaultPauseFileName = "Pause.txt";
        private const string DefaultListFileName = "Files.xml";

        private const int DeafultIntervalBetweenFrames = 200;
        private const int DeafultIntervalForShowOneFrame = 0;

        public string IconDirectoryName { get; set; }

        public string PauseFileName { get; set; }

        public string ListFileName { get; set; }

        public int IntervalBetweenFrames { get; set; }

        public int IntervalForShowOneFrame { get; set; }


        public ApplicationSettings()
        {
            IconDirectoryName = Path.Combine(AssemblyUtils.AssemblyDirectory, DefaultIconDirectoryName);
            PauseFileName = DefaultPauseFileName;
            ListFileName = DefaultListFileName;
            IntervalBetweenFrames = DeafultIntervalBetweenFrames;
            IntervalForShowOneFrame = DeafultIntervalForShowOneFrame;
        }

        public object Clone() => new ApplicationSettings
        {
            IconDirectoryName = IconDirectoryName,
            PauseFileName = PauseFileName,
            ListFileName = ListFileName,
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

            if (string.Compare(IconDirectoryName, other.IconDirectoryName, StringComparison.CurrentCultureIgnoreCase) != 0 ||
                string.Compare(PauseFileName, other.PauseFileName, StringComparison.CurrentCultureIgnoreCase) != 0 ||
                string.Compare(ListFileName, other.ListFileName, StringComparison.CurrentCultureIgnoreCase) != 0)
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
            hashCode ^= IconDirectoryName.GetHashCode();
            hashCode ^= PauseFileName.GetHashCode();
            hashCode ^= ListFileName.GetHashCode();
            hashCode ^= IntervalBetweenFrames.GetHashCode();
            hashCode ^= IntervalForShowOneFrame.GetHashCode();
            return hashCode;
        }       
    }
}
