using System;

namespace test_wpf1.Models
{
    public class Track
    {
        public string Artist { get; set; }

        public string Title { get; set; }

        public TimeSpan? Length { get; set; }

        public bool IsHQ { get; set; }

        public Guid UID { get; set; }

        public string Extension { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}{3}",
                string.Format("[{0}]", this.UID.ToString()),
                this.Title,
                this.Length.HasValue ? string.Format("({0})", this.Length.ToString()) : string.Empty,
                this.IsHQ ? "(HQ)" : string.Empty
                );
        }
    }
}
