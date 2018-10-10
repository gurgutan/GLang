using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Interpreter
{
    public class BNFGrammar
    {
        //Управляющие символы для описания грамматики. Описаны в виде регулярных выражений
        public static string BNFSymbolsSeparators = @"[\s\n\t\r]";
        public static string BNFRulesSeparator = @";";
        public static string BNFCommentPrefix = @"//";
        public static string BNFTermsSeparator = @"\|";
        public static string BNFLeftToRightSeparator = @"=";
        public static string BNFTerminalPrefix = @"'";
        public static string BNFNoneTerminal = @"(?<!" + BNFTerminalPrefix + @")";

        public string Name;
        public string Root;
        public Dictionary<string, BNFRule> Rules = new Dictionary<string, BNFRule>();

        public class BNFTerm
        {
            public string Name;

            private string[] symbols;
            public string[] Symbols
            {
                get
                {
                    return symbols;
                }
                set
                {
                    symbols = value;
                    Name = this.ToString();
                }
            }

            public BNFTerm(string _var)
            {
                Symbols = Regex.Split(_var, BNFNoneTerminal + BNFSymbolsSeparators).
                    Select(s => s.Trim()).
                    Where(s => s.Length > 0).
                    ToArray();
                if (Symbols.Length == 0) throw new Exception("Ошибка: вариант " + _var + " пустой.");
                Name = GetName(); 
            }

            public override bool Equals(object obj)
            {
                BNFTerm v = obj as BNFTerm;
                return v.Name == Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public override string ToString()
            {
                return GetName();
            }

            public string GetName()
            {
                return symbols.Aggregate("", (curr, next) => (curr == "") ? next : curr + " " + next);
            }

            public static bool operator <(BNFTerm left, BNFTerm right)
            {
                return left.Symbols.Count() < right.Symbols.Count();
            }
            public static bool operator >(BNFTerm left, BNFTerm right)
            {
                return left.Symbols.Count() > right.Symbols.Count();
            }
        }

        public class BNFRule
        {
            public string Name;
            public Dictionary<string, BNFTerm> Terms;

            public BNFRule(string _rule)
            {
                _rule = _rule.Trim(BNFSymbolsSeparators.ToCharArray());
                //Разбиваем строку правила на две части по знаку '='
                string[] parts = Regex.Split(_rule, BNFNoneTerminal + BNFLeftToRightSeparator).
                    Select(s => s.Trim()).
                    Where(s => s.Length > 0).ToArray();                                                             
                if (parts.Length == 0)
                {
                    throw new Exception("Ошибка в правиле: " + _rule + " - пустая строка");
                }
                else if (parts.Length > 2)
                {
                    throw new Exception("Ошибка в правиле: в строке \n " + _rule + "\n более одного символа '='");
                }
                else if (parts.Length == 1)
                {
                    Name = _rule;
                    Terms = null;
                }
                if (parts[0] == "") throw new Exception("Ошибка в правиле: в строке \n " + _rule + "\n пустая левая часть");
                if (parts[1] == "") throw new Exception("Ошибка в правиле: в строке \n " + _rule + "\n пустая правая часть");
                Name = parts[0];            //Левая часть
                Terms = Regex.Split(parts[1], BNFNoneTerminal + BNFTermsSeparator).
                    Select(s => s.Trim()).                          // удаляем пробелы вначале и в конце
                    Where(s => s.Length > 0).                       // удаляем пустые строки
                    Select(s => new BNFTerm(s)).                 // пеобразуем в BNFVariant
                    ToDictionary(v => v.ToString());
                    //OrderByDescending(v => v.Symbols.Count()).      // сортировка по количеству символов в варианте
            }

            public override string ToString()
            {
                return Name + BNFLeftToRightSeparator + (
                    (Terms == null) ? "" :
                        Terms.Select(v => v.Value.Name).
                        Aggregate("", (curr, next) => (curr == "") ? next : curr + "|" + next)
                    );
            }
        }

        //TODO: При инициализации грамматики сделать проверку наличия левой рекурсии (цикла между нетерминальными символами правил)
        public BNFGrammar(string _rules, string _root)
        {
            //  Набор правил состоит из строк, разделенных символом ';'
            Rules = BuildRulesFromString(_rules);                                                    // список правил преобразуем в словарь
            if (Rules.Count() == 0) throw new Exception("Ошибка инициализации грамматики: нет правил для инициализации");
            if (!Rules.ContainsKey(_root)) throw new Exception("Ошибка инициализации грамматики: в грамматике отсутствует правило " + _root);
            Root = _root;
        }

        public BNFGrammar(string _rules)
        {
            Rules = BuildRulesFromString(_rules);
            if (Rules.Count() == 0) throw new Exception("Ошибка инициализации грамматики: нет правил для инициализации");
            Root = Rules.Keys.First();
        }

        private Dictionary<string, BNFRule> BuildRulesFromString(string _rules)
        {
            _rules = _rules.Replace("\r", "");
            _rules = _rules.Replace("\n", "");
            Dictionary<string, BNFRule> result = Regex.Split(_rules, BNFNoneTerminal + BNFRulesSeparator).      // разделяем правила по символу ;
                Where(s => s.Length > 0).                                                       // оставляем только ненулевые строки
                Select<string, BNFRule>(r => new BNFRule(r.Trim(BNFSymbolsSeparators.ToCharArray()))).          // преобразуем в правила грамматики
                ToDictionary(r => r.Name);   // список правил преобразуем в словарь
            return result;
        }

        public override string ToString()
        {
            return Rules.Values.Aggregate<BNFRule, string>("", 
                (curr, next) => (curr == "") ? next.ToString() : curr + BNFRulesSeparator + "\n" + next.ToString());
        }

    }
}
