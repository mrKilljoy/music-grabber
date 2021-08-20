using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Models;
using GrabberClient.Test;

namespace GrabberClient.Services
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
