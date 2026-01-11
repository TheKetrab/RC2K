using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC2K.Logic.Interfaces;

public class Result
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int ErrorCode { get; set; } = 0;
}

public class Result<T> : Result
{
    public T? Payload { get; set; }
}
