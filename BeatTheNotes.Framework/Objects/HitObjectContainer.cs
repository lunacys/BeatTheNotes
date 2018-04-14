using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatTheNotes.Framework.Beatmaps;

namespace BeatTheNotes.Framework.Objects
{
    public class HitObjectContainer : IEnumerable<HitObject>
    {
        private List<HitObject> _rawHitObjects;
        private List<HitObject>[] _hitObjects;

        public int ColumnCount { get; }
        public int Count => _rawHitObjects.Count;

        public HitObject this[int index] => _rawHitObjects[index];

        public HitObjectContainer(int columnCount)
        {
            ColumnCount = columnCount;
            _hitObjects = new List<HitObject>[ColumnCount];
            for (var i = 0; i < _hitObjects.Length; i++)
            {
                _hitObjects[i] = new List<HitObject>();
            }

            _rawHitObjects = new List<HitObject>();
        }

        public void Add(HitObject hitObject)
        {
            Console.WriteLine($"Adding hitobject: {hitObject.Position}");
            _hitObjects[hitObject.Column - 1].Add(hitObject);
            _rawHitObjects.Add(hitObject);
        }

        public void Insert(List<HitObject> hitObjects)
        {
            for (int i = 0; i < _hitObjects.Length; i++)
                _hitObjects[i] = hitObjects.FindAll(o => o.Column == i + 1);
        }

        public void ResetAll()
        {
            foreach (var hitObject in this)
            {
                hitObject.Reset();
            }
        }

        /// <summary>
        /// Find and return the nearest object on the specified line. The nearest object is the first object on the line.
        /// </summary>
        /// <param name="line">Column starting from 1 to KeyAmount</param>
        /// <returns>Nearest object on specified line</returns>
        public HitObject GetNearestHitObjectOnLine(int line)
        {
            // If there are no objects on the line, return null
            if (_hitObjects[line - 1].Count == 0)
                return null;

            // If there are only already pressed objects on the line, return null
            if (_hitObjects[line - 1].Count(o => !o.IsExpired) == 0)
                return null;

            // Find first unpressed object on the line
            var first = _hitObjects[line - 1].First(o => !o.IsExpired);

            return first;
        }

        /// <summary>
        /// Find and return the nearest object on the specified line. The nearest object is the first object on the line.
        /// </summary>
        /// <param name="line">Column starting from 1 to KeyAmount</param>
        /// <param name="time"></param>
        /// <param name="threshold"></param>
        /// <returns>Nearest object on specified line within the threshold if found one, otherwise return null</returns>
        public HitObject GetNearestHitObjectOnLine(int line, double time, double threshold)
        {
            // If there are no objects on the line, return null
            if (_hitObjects[line - 1].Count == 0)
                return null;

            // If there are only already pressed objects on the line, return null
            if (_hitObjects[line - 1].Count(o => !o.IsExpired) == 0)
                return null;

            // Find first unpressed object on the line
            var first = _hitObjects[line - 1].First(o => !o.IsExpired);

            return Math.Abs(time - first.Position) <= (threshold) ? first : null;
        }

        public IEnumerator<HitObject> GetEnumerator()
        {
            foreach (var hitObject in _hitObjects)
                foreach (var o in hitObject)
                    yield return o;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}