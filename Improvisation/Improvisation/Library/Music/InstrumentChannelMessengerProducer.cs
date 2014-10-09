using Sanford.Multimedia.Midi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    public class InstrumentChannelMessengerProducer
    {
        private Dictionary<GeneralMidiInstrument, IReadOnlyList<ChannelMessage>> channelMessageByInstrument;

        public InstrumentChannelMessengerProducer(Sequence sequence)
        {
            sequence.NullCheck();

            this.channelMessageByInstrument = new Dictionary<GeneralMidiInstrument, IReadOnlyList<ChannelMessage>>();

            foreach (var seq in sequence)
            {
                if (seq.Iterator().Any())
                {
                    var query = seq.Iterator().Where(x => x.MidiMessage is ChannelMessage).Select(y => y.MidiMessage as ChannelMessage);
                    if (query.Any())
                    {
                        List<ChannelMessage> message = new List<ChannelMessage>();
                        var instrument = query.First().Instrument();
                        foreach (var item in query)
                        {
                            message.Add(item);
                        }
                        this.channelMessageByInstrument.Add(instrument, message.AsReadOnly());
                    }
                }
            }
        }
        public InstrumentChannelMessengerProducer(IEnumerable<Sequence> sequences)
        {
            sequences.NullCheck();

            HashSet<Sequence> unique = new HashSet<Sequence>(sequences);

            var instruments = from sequence in sequences
                              from item in sequence
                              where item.Iterator().Count(x => x.MidiMessage is ChannelMessage) > 0
                              group item by (item.Iterator().FirstOrDefault(x => x.MidiMessage is ChannelMessage).MidiMessage as ChannelMessage).Instrument() into group1
                              select group1;

            this.channelMessageByInstrument = new Dictionary<GeneralMidiInstrument, IReadOnlyList<ChannelMessage>>();

            if (instruments.Any())
            {
                foreach (var group in instruments)
                {
                    if (group.Any())
                    {
                        List<ChannelMessage> message = new List<ChannelMessage>();

                        var instrument = group.First().Iterator().Where(x => x.MidiMessage is ChannelMessage).Select(y => y.MidiMessage as ChannelMessage).First().Instrument();
                        foreach (var track in group)
                        {
                            var query = track.Iterator().Where(x => x.MidiMessage is ChannelMessage).Select(y => y.MidiMessage as ChannelMessage);
                            if (query.Any())
                            {
                                foreach (var item in query)
                                {
                                    message.Add(item);
                                }
                            }
                        }
                        this.channelMessageByInstrument.Add(instrument, message.AsReadOnly());
                    }
                }
            }
        }

        public IReadOnlyList<ChannelMessage> GetOrderedMessages(GeneralMidiInstrument inst, bool throwExceptionIfNotFound = false)
        {
            if (this.channelMessageByInstrument.ContainsKey(inst))
            {
                return this.channelMessageByInstrument[inst];
            }
            if (throwExceptionIfNotFound)
            {
                throw new KeyNotFoundException();
            }
            return new List<ChannelMessage>().AsReadOnly();
        }

    }
    public class InstrumentMidiEventProducer
    {
        private Dictionary<GeneralMidiInstrument, IReadOnlyList<MidiEvent>> channelMessageByInstrument;

        public InstrumentMidiEventProducer(IEnumerable<Sequence> sequences)
        {
            sequences.NullCheck();

            HashSet<Sequence> unique = new HashSet<Sequence>(sequences);

            var instruments = from sequence in unique
                              from item in sequence
                              where item.Iterator().Count(x => x.MidiMessage is ChannelMessage) > 0
                              group item by (item.Iterator().FirstOrDefault(x => x.MidiMessage is ChannelMessage).MidiMessage as ChannelMessage).Instrument() into group1
                              select group1;

            this.channelMessageByInstrument = new Dictionary<GeneralMidiInstrument, IReadOnlyList<MidiEvent>>();

            if (instruments.Any())
            {
                foreach (var group in instruments)
                {
                    if (group.Any())
                    {
                        List<MidiEvent> message = new List<MidiEvent>();

                        var instrument = group.First().Iterator().Where(x => x.MidiMessage is ChannelMessage).Select(y => y.MidiMessage as ChannelMessage).First().Instrument();
                        foreach (var track in group)
                        {
                            var query = track.Iterator().Any();
                            if (track.Iterator().Any())
                            {
                                foreach (var item in track.Iterator())
                                {
                                    message.Add(item);
                                }
                            }
                        }
                        this.channelMessageByInstrument.Add(instrument, message.AsReadOnly());
                    }
                }
            }
        }
        public IReadOnlyList<MidiEvent> GetOrderedMessages(GeneralMidiInstrument inst, bool throwExceptionIfNotFound = false)
        {
            if (this.channelMessageByInstrument.ContainsKey(inst))
            {
                return this.channelMessageByInstrument[inst];
            }
            if (throwExceptionIfNotFound)
            {
                throw new KeyNotFoundException();
            }
            return new List<MidiEvent>().AsReadOnly();
        }
        public List<float> TicksPerQuarter(GeneralMidiInstrument inst, bool throwExceptionIfNotFound = false)
        {
            var messages = this.GetOrderedMessages(inst, throwExceptionIfNotFound);
            var meta = messages
                .Where(x => x.MidiMessage is MetaMessage)
                .Select(x => x.MidiMessage as MetaMessage)
                .Where(x => x.MetaType == MetaType.TimeSignature);

            var time = new TimeSignatureBuilder();

            return null;
        }
        private float TicksPerQuarterSingular(MetaMessage message)
        {
            var bytes = message.GetBytes();
            return 0;
        }
    }
}
