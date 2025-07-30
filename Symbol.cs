using System;

namespace SymbolPad
{
    public class Symbol
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string HalfWidth { get; set; } = string.Empty;
        public string FullWidth { get; set; } = string.Empty;
        public bool IsPaired { get; set; }
        public bool IsDefault { get; set; }
    }
}
