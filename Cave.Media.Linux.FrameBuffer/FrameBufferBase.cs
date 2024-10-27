// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux;

public abstract class FrameBufferBase : IDisposable
{
    #region Protected Methods

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
            }

            IsDisposed = true;
        }
    }

    #endregion Protected Methods

    #region Public Constructors

    public FrameBufferBase()
    {
    }

    #endregion Public Constructors

    #region Public Properties

    public bool IsDisposed { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public abstract void Present();

    #endregion Public Methods
}
