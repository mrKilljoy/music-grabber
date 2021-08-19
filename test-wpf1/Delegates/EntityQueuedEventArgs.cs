using System;
using System.Collections.Generic;
using System.Text;
using test_wpf1.Contracts;
using test_wpf1.Models;

namespace test_wpf1.Delegates
{
    public class EntityQueuedEventArgs : EventArgs
    {
        public EntityQueuedEventArgs(IQueryableEntity track) => this.Track = track;

        public IQueryableEntity Track { get; }
    }
}
