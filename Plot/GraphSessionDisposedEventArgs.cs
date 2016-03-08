using System;

namespace Plot
{
    public class GraphSessionDisposedEventArgs : EventArgs
    {
        public GraphSessionDisposedEventArgs(IGraphSession session)
        {
            Session = session;
        }

        public IGraphSession Session { get; set; }
    }
}
