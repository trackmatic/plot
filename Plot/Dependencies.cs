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

                var diff = (_sequence+1) - dependency.Sequence;

                if (diff <= 0)
                {
                    return;
                }

                dependency.Increment(_sequence);
            }
        }

        private void Increment(int parent)
        {
            _sequence = parent + 1;

            foreach (var dependency in _dependencies)
            {
                dependency._sequence = _sequence + 1;
            }
        }
    }
}