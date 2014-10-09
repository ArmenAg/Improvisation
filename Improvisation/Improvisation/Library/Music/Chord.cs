using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Music
{
    [Serializable]
    public class Chord : IEquatable<Chord>
    {
        public static bool AllowForComplexSimplification = true;

        public readonly int AbsoluteTick;
        public readonly int DeltaTick;

        public readonly IReadOnlyList<MidiEvent> MidiChunk;
        public readonly IReadOnlyList<FullNote> Notes;

        private int hash = 0;
        private Chord(List<MidiEvent> messages, List<FullNote> notes)
        {
            messages.NullCheck();
            notes.NullCheck();

            messages.Any().AssertTrue();
            notes.Any().AssertTrue();

            this.AbsoluteTick = messages.Last().AbsoluteTicks;
            this.DeltaTick = messages.Sum(x => x.DeltaTicks);

            this.MidiChunk = messages.AsReadOnly();
            this.Notes = new HashSet<FullNote>(notes)
                .ToList()
                .AsReadOnly();


            this.hash = HashHelper.FNVHashCode(this.Notes.ToArray());

        }

        public override int GetHashCode()
        {
            return this.hash;
        }
        public override bool Equals(object obj)
        {
            var maybe = obj as Chord;

            if (obj == null || maybe == null)
            {
                return false;
            }

            return this.Equals(maybe);
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("{");
            foreach (var item in new HashSet<FullNote>(this.Notes))
            {
                builder.Append(item.ToString() + " ");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("}");

            return builder.ToString();
        }

        public static List<Chord> RetrieveChords(IEnumerable<MidiEvent> track, BaseNoteRetriever retriever)
        {
            track.NullCheck();

            var individualMidiEvents = track.ToList();
            var indexes = Chord.GetIndexes(individualMidiEvents);
            var pureChords = Chord.GetAllFullChords(individualMidiEvents, indexes);

            var start = individualMidiEvents.FindIndex(x => x.MidiMessage is ChannelMessage);
            var last = individualMidiEvents.FindLastIndex(x => x.MidiMessage is ChannelMessage);

            List<Chord> chords = new List<Chord>();

            for (int pureChordCounter = 0; start < last && pureChordCounter < pureChords.Count; start++)
            {
                if (start < pureChords[pureChordCounter].Item1)
                {
                    var pair = Chord.FindPairCount(start, pureChords[pureChordCounter].Item2, individualMidiEvents);

                    if (pair.HasValue)
                    {
                        Chord outChord;
                        if (Chord.TryParse(individualMidiEvents.GetRange(start, pair.Value - start + 1), retriever, out outChord))
                        {
                            chords.Add(outChord);
                        }
                    }
                }
                else
                {
                    var item = pureChords[pureChordCounter];
                    Chord outChord;
                    if (Chord.TryParse(individualMidiEvents.GetRange(item.Item1, item.Item2 - item.Item1 + 1), retriever, out outChord))
                    {
                        chords.Add(outChord);
                    }
                    start = item.Item2;
                    pureChordCounter++;
                }
            }
            return chords.ToList();
        }

        private static int? FindPairCount(int start, int end, List<MidiEvent> individualMidiEvents)
        {
            ChannelMessage maybe = individualMidiEvents[start].MidiMessage as ChannelMessage;

            if (null == maybe || maybe.Command != ChannelCommand.NoteOn)
            {
                return null;
            }

            for (; start < end; start++)
            {
                var maybe1 = individualMidiEvents[start].MidiMessage as ChannelMessage;
                if (maybe1 != null)
                {
                    if (maybe1.Command == ChannelCommand.NoteOff && maybe1.Data1 == maybe.Data1)
                    {
                        return start;
                    }
                }
            }
            return start + 1;
        }
        public static bool TryParse(List<MidiEvent> messages, BaseNoteRetriever retriever, out Chord chord)
        {
            if (messages == null || retriever == null)
            {
                chord = null;
                return false;
            }
            chord = new Chord(messages,
                messages
                .Where(x => x.MidiMessage is ChannelMessage)
                .Select(x => x.MidiMessage as ChannelMessage)
                .Where(x => x.Command == ChannelCommand.NoteOn && retriever.ValidNoteId(x))
                .Select(x => retriever.RetrieveNote(x)).ToList());
            return true;
        }

        private static List<Tuple<int, int>> GetAllFullChords(List<MidiEvent> individualMidiEvents, Dictionary<ChannelMessage, List<int>> indexes)
        {

            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i < individualMidiEvents.Count; i++)
            {
                var tuple = Chord.RetrieveIndexRange(indexes, i);

                if (null != tuple)
                {
                    if (tuple.Item2 > tuple.Item1)
                    {
                        list.Add(tuple);
                        i = Math.Max(tuple.Item2 - 1, i);
                    }
                }
            }
            return new HashSet<Tuple<int, int>>(list).ToList();
        }
        private static Tuple<int, int> RetrieveIndexRange(Dictionary<ChannelMessage, List<int>> indexes, int startIndex)
        {
            Dictionary<ChannelMessage, int> fromStart = indexes
                .Where(x => x.Key.Command == ChannelCommand.NoteOn)
                .ToDictionary(x => x.Key, x => x.Value.Where(y => y >= startIndex).FirstOrDefault());

            var reversed = fromStart.ReverseDictionary();

            if (!fromStart.Any())
            {
                return null;
            }

            int count = 1;
            foreach (var top in reversed)
            {
                int lowCount = 1;
                foreach (var bottom in reversed.Skip(count))
                {
                    if (bottom.Key - top.Key == lowCount)
                    {
                        lowCount++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lowCount != 1)
                {
                    for (int q = 1; q < lowCount; q++)
                    {
                        var data = reversed[lowCount + top.Key - q].First();
                        var found = indexes.Where(x => x.Key.Command == ChannelCommand.NoteOff && x.Key.Data1 == data.Data1 && x.Value.Any(y => y >= startIndex));
                        if (found.Any())
                        {
                            return Tuple.Create(top.Key, found.First().Value.Where(i => i >= startIndex).First());
                        }
                    }
                }
                count++;
            }

            var sorted = new SortedDictionary<int, List<ChannelMessage>>(reversed);
            var first = sorted.FirstOrDefault(x => x.Key != 0);
            if (first.Key == 0)
            {
                return null;
            }
            var data1 = reversed[first.Key].First();
            var found1 = indexes.Where(x => x.Key.Command == ChannelCommand.NoteOff && x.Key.Data1 == data1.Data1 && x.Value.Any(y => y >= startIndex));
            if (found1.Any())
            {
                return Tuple.Create(first.Key, found1.First().Value.Where(i => i >= startIndex).First());
            }
            else if (Chord.AllowForComplexSimplification)
            {
                var found2 = indexes.Where(x => x.Key.Command == ChannelCommand.NoteOn && x.Value.Any(y => y >= startIndex));

                if (found2.Any())
                {
                    return Tuple.Create(first.Key, found2.First().Value.Where(i => i >= startIndex).First());
                }
            }
            return null;
        }
        private static Dictionary<ChannelMessage, List<int>> GetIndexes(List<MidiEvent> events)
        {
            var list = events.ToList();
            Dictionary<ChannelMessage, List<int>> retValue = new Dictionary<ChannelMessage, List<int>>();

            for (int i = 0; i < list.Count; i++)
            {
                var maybe = list[i].MidiMessage as ChannelMessage;
                if (null != maybe && (maybe.Command == ChannelCommand.NoteOn || maybe.Command == ChannelCommand.NoteOff))
                {
                    if (retValue.ContainsKey(maybe))
                    {
                        retValue[maybe].Add(i);
                        continue;
                    }
                    retValue.Add(maybe, new List<int>() { i });
                }
            }
            return retValue;
        }

        public bool Equals(Chord other)
        {
            if (other == null)
            {
                return false;
            }
            if (this.hash != other.hash)
            {
                return false;
            }
            for (int i = 0; i < other.Notes.Count; i++)
            {
                if (other.Notes[i] != this.Notes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

}
