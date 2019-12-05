using System;

namespace BlazorMemoryTest.Data
{
    public class LogPresenter
    {
        public DateTime Occurs { get; } = DateTime.Now;
        public string Message { get; set; } = "";
        public string Source { get; set; } = "SYSTEM";

        public override string ToString() => $"{Occurs:hh:MM:ss (fff)}{Message}{Source}";
    }
}