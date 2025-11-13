namespace RC2K.DomainModel;

public enum RallyCode
{
    Sony,
    Vauxhall,
    Pirelli,
    Scottish,
    Seat,
    Stena
}

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
    Replay = 'r'
}