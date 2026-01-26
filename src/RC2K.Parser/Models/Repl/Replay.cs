namespace RC2K.Parser.Models.Repl;

public class Repl
{
    int F1 { get; set; } // "REPL"
    int F2 { get; set; }
    int F3 { get; set; } // 00 04 05 00
    int F4 { get; set; } // hash
    string F5 { get; set; } // filename, 32 bytes
    int F6 { get; set; } // stage code
    int F7 { get; set; }
    int F8 { get; set; }
    int F9 { get; set; }
    int F10 { get; set; } // car
    int F11 { get; set; }
    int F12 { get; set; }
    int F13 { get; set; }
    int F14 { get; set; }
    int F15 { get; set; }
    int F16 { get; set; }
    int F17 { get; set; }
    int F18 { get; set; }
    int F19 { get; set; }
    int F20 { get; set; }
    int F21 { get; set; }
    // further we have data


    // ...

    int F101 { get; set; } // 52 43 49 42  (RCIB)
    int F102 { get; set; } // ??
    int F103 { get; set; } // ??
    int ReplTime { get; set; } // time of the replay, rounded up, +8sec
    // 3ec wait, 5sec before start, ~3 or 4 sec on end, rounded ; warning: if broken and 'R' returned car, then time is misleading; then its impossible to 
}
