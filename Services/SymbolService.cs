using System;
using System.Collections.Generic;
using System.Linq;

namespace SymbolPad.Services
{
    public class SymbolService
    {
        private readonly List<Symbol> _defaultSymbols;
        private UserSettings _userSettings;

        public SymbolService()
        {
            _defaultSymbols = InitializeDefaultSymbols();
            _userSettings = StorageService.LoadSettings();
        }

        public List<Symbol> GetSymbols()
        {
            var allSymbols = new List<Symbol>(_defaultSymbols);
            var defaultIds = new HashSet<Guid>(_defaultSymbols.Select(s => s.Id));
            var customSymbols = _userSettings.CustomSymbols.Where(cs => !defaultIds.Contains(cs.Id)).ToList();
            
            allSymbols.AddRange(customSymbols);

            var orderedSymbols = new List<Symbol>();
            var symbolMap = allSymbols.ToDictionary(s => s.Id);

            foreach (var id in _userSettings.SymbolOrder)
            {
                if (symbolMap.TryGetValue(id, out var symbol))
                {
                    orderedSymbols.Add(symbol);
                    symbolMap.Remove(id);
                }
            }
            
            orderedSymbols.AddRange(symbolMap.Values.OrderBy(s => s.Name));

            return orderedSymbols;
        }

        public void SaveSettings(UserSettings settings)
        {
            _userSettings = settings;
            StorageService.SaveSettings(_userSettings);
        }

        private List<Symbol> InitializeDefaultSymbols()
        {
            return new List<Symbol>
            {
                // --- 括号 (Brackets) ---
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000001"), Name = "圆括号", HalfWidth = "()", FullWidth = "（）", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000002"), Name = "方括号", HalfWidth = "[]", FullWidth = "［］", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000003"), Name = "花括号", HalfWidth = "{}", FullWidth = "｛｝", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000004"), Name = "中括号", HalfWidth = "【】", FullWidth = "【】", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000005"), Name = "角括号", HalfWidth = "<>", FullWidth = "〈〉", IsPaired = true, IsDefault = true },

                // --- 引号 (Quotes) ---
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000006"), Name = "书名号", HalfWidth = "<<>>", FullWidth = "《》", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000007"), Name = "单引号", HalfWidth = "''", FullWidth = "‘’", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000008"), Name = "双引号", HalfWidth = "\"\"", FullWidth = "“”", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000009"), Name = "直角引号", HalfWidth = "「」", FullWidth = "「」", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000010"), Name = "双直角引号", HalfWidth = "『』", FullWidth = "『』", IsPaired = true, IsDefault = true },

                // --- 公式与特殊符号 (Formula & Special Symbols) ---
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000011"), Name = "LaTeX公式", HalfWidth = "$$", FullWidth = "$$", IsPaired = true, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000012"), Name = "数字符号", HalfWidth = "No.", FullWidth = "№", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000013"), Name = "倒感叹号", HalfWidth = "¡", FullWidth = "¡", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000014"), Name = "倒问号", HalfWidth = "¿", FullWidth = "¿", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000015"), Name = "乘号", HalfWidth = "x", FullWidth = "×", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000016"), Name = "除号", HalfWidth = "/", FullWidth = "÷", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000017"), Name = "序号标识(阳)", HalfWidth = "o", FullWidth = "º", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000018"), Name = "序号标识(阴)", HalfWidth = "a", FullWidth = "ª", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000019"), Name = "千分号", HalfWidth = "%o", FullWidth = "‰", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000020"), Name = "万分号", HalfWidth = "%oo", FullWidth = "‱", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000021"), Name = "正负号", HalfWidth = "+-", FullWidth = "±", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000022"), Name = "负正号", FullWidth = "∓", HalfWidth = "-+", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000023"), Name = "角分符号", HalfWidth = "'", FullWidth = "′", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000024"), Name = "角秒符号", HalfWidth = "\"\"", FullWidth = "″", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000025"), Name = "段落符号", HalfWidth = "P", FullWidth = "¶", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000026"), Name = "分节符号", HalfWidth = "S", FullWidth = "§", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000027"), Name = "参考标记", HalfWidth = "*", FullWidth = "※", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000028"), Name = "剑标", HalfWidth = "+", FullWidth = "†", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000029"), Name = "双剑标", HalfWidth = "++", FullWidth = "‡", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000030"), Name = "三剑标", HalfWidth = "+++", FullWidth = "⹋", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000031"), Name = "版权符号", HalfWidth = "(C)", FullWidth = "©", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000032"), Name = "注册商标符号", HalfWidth = "(R)", FullWidth = "®", IsPaired = false, IsDefault = true },
                new Symbol { Id = Guid.Parse("00000001-0000-0000-0000-000000000033"), Name = "商标符号", HalfWidth = "TM", FullWidth = "™", IsPaired = false, IsDefault = true },
            };
        }
    }
}