namespace RC2K.Parser.Models.Repl;

public class Repl
{
    int _F1 { get; set; } // "REPL"
    int _F2 { get; set; }
    int _F3 { get; set; } // 00 04 05 00
    int _F4 { get; set; } // hash
    string _F5 { get; set; } = ""; // filename, 32 bytes
    int _F6 { get; set; } // stage code
    int _F7 { get; set; }
    int _F8 { get; set; }
    int _F9 { get; set; }
    int _F10 { get; set; } // car
    int _F11 { get; set; }
    int _F12 { get; set; }
    int _F13 { get; set; }
    int _F14 { get; set; }
    int _F15 { get; set; }
    int _F16 { get; set; }
    int _F17 { get; set; }
    int _F18 { get; set; }
    int _F19 { get; set; }
    int _F20 { get; set; }
    int _F21 { get; set; }
    // further we have data


    // ...

    int _F101 { get; set; } // 52 43 49 42  (RCIB)
    int _F102 { get; set; } // ??
    int _F103 { get; set; } // ??
    int _ReplTime { get; set; } // time of the replay, rounded up, +8sec
    // 3ec wait, 5sec before start, ~3 or 4 sec on end, rounded ; warning: if broken and 'R' returned car, then time is misleading; then its impossible to 
}
