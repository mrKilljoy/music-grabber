using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_wpf1.Contracts;
using test_wpf1.Models;
using test_wpf1.Test;

namespace test_wpf1.Services
{
    public sealed class DummyMusicService : IMusicService
    {
        private readonly Random rnd;

        public DummyMusicService()
        {
            rnd = new Random();
        }

        public Task<IEnumerable<Track>> GetTracksAsync(string query)
        {
            return Task.FromResult(DataSets.TestTrackSetOne.Take(rnd.Next(0, DataSets.TestTrackSetOne.Count)));
        }
    }
}
