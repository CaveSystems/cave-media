using System;
using System.Diagnostics;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a basic platform and device independent audio queue.
    /// </summary>
    public abstract class AudioOut : IDisposable
    {
        #region constructor

        /// <summary>
        /// Creates a new audio stream for the specified device.
        /// </summary>
        /// <param name="device">The device to use.</param>
        /// <param name="configuration">The configuration to use.</param>
        protected internal AudioOut(IAudioDevice device, IAudioConfiguration configuration)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        #endregion

        #region abstract class

        #region protected abstract functions

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Begins playing.
        /// </summary>
        protected abstract void StartPlayback();

        /// <summary>
        /// Stops playing.
        /// </summary>
        protected abstract void StopPlayback();

        #endregion

        #region public abstract properties

        /// <summary>
        /// Gets the latency of the queue.
        /// </summary>
        public abstract TimeSpan Latency { get; }

        /// <summary>
        /// Gets the number of bytes passed since starting this queue.
        /// </summary>
        public abstract long BytesPassed { get; }

        /// <summary>
        /// Gets the bytes buffered (bytes to play until queue gets empty).
        /// </summary>
        public abstract long BytesBuffered { get; }

        /// <summary>
        /// Gets whether the IAudioQueue supports 3D positioning or not.
        /// </summary>
        public abstract bool Supports3D { get; }
        #endregion

        #region public abstract functions

        /// <summary>Writes a buffer to the device.</summary>
        /// <param name="audioData">The buffer.</param>
        public abstract void Write(IAudioData audioData);

        #endregion

        #endregion

        #region public implemented functions

        /// <summary>Writes the specified data to the device.</summary>
        /// <param name="data">The data.</param>
        public void Write(byte[] data) => Write(new AudioData(Configuration.SamplingRate, Configuration.Format, Configuration.ChannelSetup, TimeBuffered, 0, 0, data));

        /// <summary>
        /// Starts the device with the specified output configuration.
        /// </summary>
        public void Start()
        {
            switch (State)
            {
                case AudioDeviceState.Started: throw new InvalidOperationException(string.Format("Cannot start device twice!"));
                case AudioDeviceState.Invalid: throw new InvalidOperationException(string.Format("Device is invalid!"));
                case AudioDeviceState.Stopped: break;
                default: throw new NotImplementedException(string.Format("Unknown state {0}!", State));
            }
            Trace.WriteLine("Start Playback");
            StartPlayback();
            State = AudioDeviceState.Started;
        }

        /// <summary>
        /// Stops all streams connected to this device and closes the device.
        /// </summary>
        public void Stop()
        {
            switch (State)
            {
                case AudioDeviceState.Started: break;
                case AudioDeviceState.Invalid: throw new InvalidOperationException(string.Format("Device is invalid!"));
                case AudioDeviceState.Stopped: throw new InvalidOperationException(string.Format("Cannot stop device (was not started)!"));
                case AudioDeviceState.Disposed: throw new ObjectDisposedException(LogSourceName);
                case AudioDeviceState.Closed: throw new InvalidOperationException(string.Format("Device already closed!"));
                default: throw new NotImplementedException(string.Format("Unknown state {0}!", State));
            }
            Trace.WriteLine("Stop Playback");
            StopPlayback();
            State = AudioDeviceState.Stopped;
        }

        /// <summary>
        /// Closes the stream and releases both managed and unmanaged resources.
        /// </summary>
        public virtual void Close()
        {
            if (State == AudioDeviceState.Started)
            {
                Stop();
            }

            if (State < AudioDeviceState.Closed)
            {
                State = AudioDeviceState.Closed;
                Dispose();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            if (State < AudioDeviceState.Disposed)
            {
                State = AudioDeviceState.Disposed;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region public implemented properties

        /// <summary>Gets the buffer underflow count.</summary>
        /// <value>The buffer underflow count.</value>
        public abstract long BufferUnderflowCount { get; }

        /// <summary>Gets or sets the pitch.</summary>
        /// <value>The pitch.</value>
        public abstract float Pitch { get; set; }

        /// <summary>Gets or sets the volume.</summary>
        /// <value>The volume in range 0..1.</value>
        public abstract float Volume { get; set; }

        /// <summary>
        /// sets / gets the 3d position of the sound source.
        /// </summary>
        public abstract Vector3 Position3D { get; set; }

        /// <summary>
        /// Retrieves the <see cref="IAudioConfiguration"/> used by this queue.
        /// </summary>
        public IAudioConfiguration Configuration { get; private set; }

        /// <summary>
        /// Retrieves the <see cref="IAudioDevice"/> used by this queue.
        /// </summary>
        public IAudioDevice Device { get; private set; }

        /// <summary>
        /// Gets the time passed since starting this queue.
        /// </summary>
        public long TicksPassed { get { return BytesPassed / Configuration.BytesPerTick; } }

        /// <summary>
        /// Gets the current state of the device.
        /// </summary>
        public AudioDeviceState State { get; private set; } = AudioDeviceState.Stopped;

        /// <summary>
        /// Gets the time buffered (time to play until queue gets empty).
        /// </summary>
        public TimeSpan TimeBuffered { get { return TimeSpan.FromSeconds((double)BytesBuffered / (Configuration.BytesPerTick * Configuration.SamplingRate)); } }

        /// <summary>
        /// Gets the time passed since starting this queue.
        /// </summary>
        public TimeSpan TimePassed { get { return TimeSpan.FromSeconds(TicksPassed / (double)Configuration.SamplingRate); } }

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public string LogSourceName { get { return "AudioOut: " + Device.Name; } }
        #endregion
    }
}
