namespace RC2K.DomainModel;

public enum RallyCode
{
    Vauxhall = 1,
    Pirelli = 2,
    Scottish = 3,
    Seat = 4,
    Stena = 5,
    Sony = 6,
}

[Flags]
public enum Mood
{
    Sunrise     = 1 << 0,
    Day         = 1 << 1,
    Sunset      = 1 << 2,
    Night       = 1 << 3,

    Cloud       = 1 << 8,
    Rain        = 1 << 9,
    Snow        = 1 << 10,
    Fog         = 1 << 11
}

public enum Direction
{
    Simulation,
    Arcade
}

public enum ProofType
{
    Unknown = '?',
    Image = 'i',
    Twitch = 't',
    Youtube = 'y',
    Replay = 'r',
    Hst = 'h'
}