using System;

namespace Plot
{
    public class ItemRegisteredEventArgs : EventArgs
    {
        public ItemRegisteredEventArgs(object item)
        {
            Item = item;
        }

        public object Item { get; private set; }
    }
}
