using Improvisation.Library.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.MusicViaMelody
{
    public interface IMelodyTransformRule
    {
        string TransformName { get; }
        Melody Transform(Melody baseMelody);
    }
    public interface IChordTransformRule
    {
        string TransformName { get; }
        Chord Transform(Chord baseMelody);
    }
}
