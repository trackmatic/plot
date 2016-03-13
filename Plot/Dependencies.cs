using System;
using System.Collections.Generic;

namespace Plot
{
    public class Dependencies
    {
        private readonly List<Dependencies> _dependencies;

        private int _sequence;

        private readonly string _name;

        public Dependencies(string name)
        {
            _dependencies = new List<Dependencies>();
            _sequence = 1;
            _name = name;
        }

        public int Sequence => _sequence;

        public void Register(Dependencies dependency)
        {
            if (!_dependencies.Contains(dependency))
            {
                _dependencies.Add(dependency);
                dependency.Increment(_sequence);
            }
        }

        public void Increment(int parent)
        {
            if (parent + 1 > Sequence)
            {
                _sequence = parent + 1;
            }

            foreach (var dependency in _dependencies)
            {
                if (dependency._sequence + 1 > _sequence + 1)
                {
                    continue;
                }

                //dependency.Increment(_sequence);

                dependency._sequence = _sequence + 1;
            }
        }
    }
}
