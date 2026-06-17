using System;
using System.Collections.Generic;
using System.Text;

namespace RC2K.E2ETests;

public class Configuration
{
    public string Host { get; set; }
    public string TestUser { get; set; }
    public string TestUserPassword { get; set; }
    public bool ShowBrowser { get; set; }
}
