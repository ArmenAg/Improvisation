using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Improvisation.Library.Data
{
    public static class MusicCorpus
    {
        public static void PlayPiano()
        {
            Sequencer a = new Sequencer();
            using (OutputDevice outDevice = new OutputDevice(0))
            {
                for (int i = 60; i < 100; i++)
                {
                    ChannelMessageBuilder builder = new ChannelMessageBuilder();

                    builder.Command = ChannelCommand.NoteOn;
                    builder.MidiChannel = 14;
                    builder.Data1 = i;
                    builder.Data2 = i - 20;
                    builder.Build();

                    outDevice.Send(builder.Result);

                    Thread.Sleep(500);

                    builder.Command = ChannelCommand.NoteOff;
                    builder.Data2 = 0;
                    builder.Build();

                    outDevice.Send(builder.Result);
                }
            }
        }
        public static void PlayPartOfTrack()
        {
            Sequence seq = new Sequence(@"C:\Users\armen_000\Documents\Visual Studio 2013\Projects\Improvisation\Improvisation\bin\Debug\loseyourself.mid");
            using (var device = new OutputDevice(0))
            {
                var track = seq[1];
                foreach (var item in track.Iterator())
                {
                    var k = item.MidiMessage as ChannelMessage;
                    if (k != null)
                    {
                        device.Send(k);
                        Thread.Sleep(item.DeltaTicks);
                    }
                }
            }
        }
    }
}
