using System;

namespace Plot
{
    public class GraphSessionFlushedEventArgs : EventArgs
    {
        public GraphSessionFlushedEventArgs(IGraphSession session)
        {
            Session = session;
        }

        public IGraphSession Session { get; set; }
    }
}
