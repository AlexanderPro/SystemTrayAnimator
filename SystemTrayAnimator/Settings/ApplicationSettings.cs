using System;
using System.IO;
using SystemTrayAnimator.Utils;

namespace SystemTrayAnimator.Settings
{
    public class ApplicationSettings : ICloneable
    {
        public const string DefaultDirectoryName = "Images";
        public const bool DefaultIncludeSubdirectories = false;
        public const string DefaultFileExtensions = "*.gif";
        public const int DefaultIntervalInMilliseconds = 50;
        public const bool DefaultUseDelayFromFirstFrame = false;
        public const bool DefaultHighDpiSupport = true;

        public string DirectoryName { get; set; }

        public bool IncludeSubdirectories { get; set; }

        public string FileExtensions { get; set; }

        public int Interval { get; set; }
        
        public bool UseDelayFromFirstFrame { get; set; }

        public bool HighDpiSupport { get; set; }

        public bool IsPaused { get; set; }

        public ApplicationSettings()
        {
            DirectoryName = Path.Combine(AssemblyUtils.AssemblyDirectory, DefaultDirectoryName);
            IncludeSubdirectories = DefaultIncludeSubdirectories;
            FileExtensions = DefaultFileExtensions;
            Interval = DefaultIntervalInMilliseconds;
            UseDelayFromFirstFrame = DefaultUseDelayFromFirstFrame;
            HighDpiSupport = DefaultHighDpiSupport;
        }

        public object Clone() => new ApplicationSettings
        {
            DirectoryName = DirectoryName,
            IncludeSubdirectories = IncludeSubdirectories,
            FileExtensions = FileExtensions,
            Interval = Interval,
            UseDelayFromFirstFrame = UseDelayFromFirstFrame,
            HighDpiSupport = HighDpiSupport
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

            return Equals((ApplicationSettings)other);
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
                string.Compare(FileExtensions, other.FileExtensions, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return false;
            }

            if (IncludeSubdirectories != other.IncludeSubdirectories || Interval != other.Interval || 
                UseDelayFromFirstFrame != other.UseDelayFromFirstFrame || HighDpiSupport != other.HighDpiSupport)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            hashCode ^= DirectoryName.GetHashCode();
            hashCode ^= IncludeSubdirectories.GetHashCode();
            hashCode ^= FileExtensions.GetHashCode();
            hashCode ^= Interval.GetHashCode();
            hashCode ^= UseDelayFromFirstFrame.GetHashCode();
            hashCode ^= HighDpiSupport.GetHashCode();
            return hashCode;
        }
    }
}
