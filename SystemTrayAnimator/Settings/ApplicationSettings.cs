using System;
using System.IO;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Settings
{
    public class ApplicationSettings : ICloneable
    {
        private const string DefaultDirectoryName = "Icons";
        private const bool DefaultIncludeSubdirectories = false;
        private const string DefaultPauseFileName = "Pause.txt";
        private const string DefaultSupportedFileExtensions = "*.ico";
        private const int DefaultIntervalInMilliseconds = 200;

        public string DirectoryName { get; set; }

        public bool IncludeSubdirectories { get; set; }

        public string PauseFileName { get; set; }

        public string SupportedFileExtensions { get; set; }

        public int Interval { get; set; }

        public bool IsPaused { get; set; }

        public ApplicationSettings()
        {
            DirectoryName = Path.Combine(AssemblyUtils.AssemblyDirectory, DefaultDirectoryName);
            IncludeSubdirectories = DefaultIncludeSubdirectories;
            PauseFileName = DefaultPauseFileName;
            SupportedFileExtensions = DefaultSupportedFileExtensions;
            Interval = DefaultIntervalInMilliseconds;
        }

        public object Clone() => new ApplicationSettings
        {
            DirectoryName = DirectoryName,
            IncludeSubdirectories = IncludeSubdirectories,
            PauseFileName = PauseFileName,
            SupportedFileExtensions = SupportedFileExtensions,
            Interval = Interval
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
                string.Compare(PauseFileName, other.PauseFileName, StringComparison.CurrentCultureIgnoreCase) != 0 ||
                string.Compare(SupportedFileExtensions, other.SupportedFileExtensions, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return false;
            }

            if (IncludeSubdirectories != other.IncludeSubdirectories || Interval != other.Interval)
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
            hashCode ^= SupportedFileExtensions.GetHashCode();
            hashCode ^= Interval.GetHashCode();
            hashCode ^= IncludeSubdirectories.GetHashCode();
            return hashCode;
        }
    }
}
