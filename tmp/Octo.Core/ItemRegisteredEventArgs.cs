using System;

namespace Octo.Core
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
