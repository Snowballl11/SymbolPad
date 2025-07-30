using System;
using System.Collections.Generic;

namespace SymbolPad
{
    public class UserSettings
    {
        public bool IsDarkMode { get; set; }
        public List<Guid> SymbolOrder { get; set; } = new List<Guid>();
        public List<Symbol> CustomSymbols { get; set; } = new List<Symbol>();
    }
}
