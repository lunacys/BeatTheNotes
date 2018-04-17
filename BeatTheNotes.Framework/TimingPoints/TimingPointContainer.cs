using System.Collections;
using System.Collections.Generic;

namespace BeatTheNotes.Framework.TimingPoints
{
    public class TimingPointContainer : IEnumerable<TimingPoint>
    {
        private readonly List<TimingPoint> _timingPoints;

        public int Count => _timingPoints.Count;

        public TimingPoint this[int index] => _timingPoints[index];

        public TimingPointContainer()
        {
            _timingPoints = new List<TimingPoint>();
        }

        public void Add(TimingPoint timingPoint)
        {
            _timingPoints.Add(timingPoint);
        }

        public TimingPoint GetCurrentTimingPoint(double time)
        {
            // If there's no timing point at the time, return the first timing point
            if (_timingPoints[0].Position > time)
                return _timingPoints[0];

            // If there's the only one timing point on the map, return that timing point
            if (_timingPoints.Count == 1)
                return _timingPoints[0];

            // TODO: This
            return _timingPoints[0];
        }

        public IEnumerator<TimingPoint> GetEnumerator()
        {
            foreach (var timingPoint in _timingPoints)
                yield return timingPoint;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}