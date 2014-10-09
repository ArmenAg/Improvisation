using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public sealed class AsyncOutputPlayer
        : IDisposable
    {
        public bool Playing { get; private set; }

        private OutputDevice outDevice;
        private Thread thread;
        private int wait = 0;
        public AsyncOutputPlayer()
        {
            this.outDevice = new OutputDevice(0);
        }

        public async void Play(IEnumerable<NGram<Chord>> enumerable)
        {
            Task.Factory.StartNew(() =>
                {
                    enumerable.NullCheck();

                    this.Playing = true;

                    var many = enumerable.SelectMany(x => x);
                    var count = many.Count(item => item.MidiChunk.Count);

                    foreach (var item in many)
                    {
                        foreach (var note in item.MidiChunk.Where(x => x.MidiMessage is ChannelMessage))
                        {
                            if (!Playing)
                            {
                                return;
                            }
                            var message = note.MidiMessage as ChannelMessage;
                            this.outDevice.Send(message);
                            if (note.DeltaTicks != 0)
                            {

                                float kSecondsPerTick = note.DeltaTicks / (OutputConstants.TicksPerQuarter * 1000000.0f);
                                float deltaTimeInMilliSeconds = note.DeltaTicks * kSecondsPerTick * 10000;
                                if (deltaTimeInMilliSeconds < 2000)
                                {
                                    this.wait = (int)deltaTimeInMilliSeconds;
                                    Thread.Sleep(this.wait);
                                }
                                else
                                {
                                    this.wait = OutputConstants.GotoSleepTImeInMilliseconds;
                                    Thread.Sleep(OutputConstants.GotoSleepTImeInMilliseconds);
                                }
                            }
                            else if (null != message && message.Command == ChannelCommand.NoteOn)
                            {
                                Thread.Sleep(OutputConstants.GotoSleepTImeInMilliseconds);
                            }
                        }
                    }
                });
        }


        public void Stop()
        {
            lock (this)
            {
                this.Playing = false;
            }
            Thread.Sleep(this.wait);
        }

        public void Dispose()
        {
            if (!this.outDevice.IsDisposed)
            {
                this.Stop();
                this.outDevice.Dispose();
            }
        }
    }
}
