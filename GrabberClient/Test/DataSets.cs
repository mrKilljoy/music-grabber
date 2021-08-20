using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GrabberClient.Models;

namespace GrabberClient.Test
{
    public static class DataSets
    {
        public static readonly ObservableCollection<Track> TestTrackSetOne = new ObservableCollection<Track>(new List<Track>
        {
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #1", Length = TimeSpan.FromSeconds(412), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #2", Length = TimeSpan.FromSeconds(231), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #3", Length = TimeSpan.FromSeconds(115), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #4", Length = TimeSpan.FromSeconds(510), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #5", Length = TimeSpan.FromSeconds(30), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #6", Length = TimeSpan.FromSeconds(1231), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #7", Length = TimeSpan.FromSeconds(99), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #8", Length = TimeSpan.FromSeconds(91), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #9", Length = TimeSpan.FromSeconds(312), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #10", Length = TimeSpan.FromSeconds(982), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #11", Length = TimeSpan.FromSeconds(101), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #12", Length = TimeSpan.FromSeconds(82), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #13", Length = TimeSpan.FromSeconds(1), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #14", Length = TimeSpan.FromSeconds(289), IsHQ = false },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #15", Length = TimeSpan.FromSeconds(431), IsHQ = true },
            new Track { Artist = "Random", Extension = "mp3", UID = Guid.NewGuid(), Title = "Track #16", Length = TimeSpan.FromSeconds(642), IsHQ = false }
        });
    }
}
