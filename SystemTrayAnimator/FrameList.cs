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

            return Equals((FrameList)other);
        }

        public bool Equals(FrameList other)
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

            if (Count != other.Count)
            {
                return false;
            }

            for (var i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach (var item in this)
            {
                hashCode ^= item.GetHashCode();
            }
            return hashCode;
        }
    }
}
