using System.Diagnostics.CodeAnalysis;

namespace PhaticBot.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum SentenceType
    {
        NP_VP_ADJP,
        Small,
        None,
        Greeting,
        Farewell,
        UnknownADVP
        
    }
}