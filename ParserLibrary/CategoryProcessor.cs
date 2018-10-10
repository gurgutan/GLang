using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;

namespace Interpreter
{
    public class CategoryProcessor
    {
        public int Error = 0;
        public CategoryProcessor Parent;
        public BNFGrammar Grammar;

        public CategoryProcessor(BNFGrammar grammar)
        {
            Grammar = grammar;
        }

        public CategoryProcessor(CategoryProcessor parent, BNFGrammar grammar)
        {
            Parent = parent;
            Grammar = grammar;
        }

        public int Execute(ASTNode node, BNFGrammar grammar)
        {
            Grammar = grammar;
            Storage.Local.Clear();
            Storage.Operative.Clear();
            Storage.Output.Clear();
            Translate(node);
            return Error;
        }

        public object Translate(ASTNode node)
        {
            /// 

            //TODO: Добавить управление вычислением: останов вычислений, включение/исключение из результата
            //
            //Категория = '{ Стрелки '} ;
            //Стрелка = Область Слово Кообласть | Слово Кообласть | Область Слово | Слово ;
            //Стрелки = Стрелка ', Стрелки | Стрелка ;
            //Область = Ноль | '{ Стрелки '} ;
            //Кообласть = Ноль | '{ Стрелки '} ;
            //Слова = Слово ', Слова | Слово ;
            //Слово = '[ Слова '] | Символы ;
            //Символы = Символ Символы | Символ ;
            //Символ = Буква | Цифра | Знак ;
            //Буква = 'a|'b|'c|'d|'e|'f|'g|'h|'i|'j|'k|'l|'m|'n|'o|'p|'q|'r|'s|'t|'u|'v|'w|'x|'y|'z|'A|'B|'C|'D|'E|'F|'G|'H|'I|'J|'K|'L|'M|'N|'O|'P|'Q|'R|'S|'T|'U|'V|'W|'X|'Y|'Z|
            //'а|'б|'в|'г|'д|'е|'ё|'ж|'з|'и|'й|'к|'л|'м|'н|'о|'п|'р|'с|'т|'у|'ф|'х|'ц|'ч|'ш|'щ|'ь|'ы|'ъ|'э|'ю|'я|'А|'Б|'В|'Г|'Д|'Е|'Ё|'Ж|'З|'И|'Й|'К|'Л|'М|'Н|'О|'П|'Р|'С|'Т|'У|'Ф|'Х|'Ц|'Ч|'Ш|'Ь|'Щ|'Ъ|'Ы|'Э|'Ю|'Я ;
            //Цифра = '0|'1|'2|'3|'4|'5|'6|'7|'8|'9 ;
            //Знак = '< | '> | '* | '+ | '- | '~: | '~ | 'id | '_ | '>: | '<: ;
            //Ноль = '{ '} ;
            Error = 0;
            if (!node.IsMatch(Grammar) && node.Type == ASTType.NonterminalEqual)
                throw new Exception("Ошибка трансляции: правило " + node.RuleName + " = " + node.RuleTerm + " не соответствует грамматике");
            switch (node.RuleName)
            {
                case "Категория": return Категория(node);
                case "Стрелка": return Стрелка(node);
                case "Стрелки": return Стрелки(node);
                case "Область": return Область(node);
                case "Кообласть": return Кообласть(node);
                case "Слова": return Слова(node);
                //case "Функтор": return Функтор(node);
                case "Слово": return Слово(node);
                case "Символы": return Символы(node);
                case "Символ": return Символ(node);
                case "Буква": return Буква(node);
                case "Цифра": return Цифра(node);
                case "Знак": return Знак(node);
                case "Ноль": return Ноль(node);
                case "'{": return "{";
                case "'}": return "}";
                case "',": return null;
                case "']": return null;
                case "'[": return null;
                case "'<": return "<";
                case "'<:": return "<:";
                case "'>": return ">";
                case "'>:": return ">:";
                case "'<-": return "<-";
                case "'->": return "->";
                case "'+": return "+";
                case "'-": return "-";
                case "'*": return "*";
                case "'~": return "~";
                case "'~:": return "~:";
                case "'^": return "^";
                case "'id": return "id";
                case "'_": return "_";
                default:
                    {
                        Error = 256;
                        throw new ArgumentException("Неизвестныое правило: " + node.RuleName);
                    }
            }
        }

        private object Категория(ASTNode node)
        {
            //Категория = '{ Стрелки '} ;
            Конус стрелка = Translate(node.Childs["Стрелки"]) as Конус;
            return стрелка;
        }

        private object Стрелка(ASTNode node)
        {
            //Стрелка = Область Слово Кообласть | Слово Кообласть | Область Слово | Слово ;
            Конус стрелка = Translate(node.Childs["Слово"]) as Конус;
            switch (node.RuleTerm)
            {
                case "Область Слово Кообласть": 
                    {
                        List<Конус> область = Translate(node.Childs["Область"]) as List<Конус>;
                        List<Конус> кообласть = Translate(node.Childs["Кообласть"]) as List<Конус>;
                        стрелка.ДобавитьВКообласть(кообласть);
                        стрелка.ДобавитьВОбласть(область);
                }; break;
                case "Слово Кообласть":
                    {
                        List<Конус> кообласть = Translate(node.Childs["Кообласть"]) as List<Конус>;
                        стрелка.ДобавитьВКообласть(кообласть);
                    }; break;
                case "Область Слово":
                    {
                        List<Конус> область = Translate(node.Childs["Кообласть"]) as List<Конус>;
                        стрелка.ДобавитьВОбласть(область);
                    }; break;
                case "Слово":
                    {
                        //морфизм "стрелка" уже создан
                    }; break;
                default: throw new ArgumentException("Неизвестный терм " + node.RuleTerm + " правила: " + node.RuleName);
            }
            return стрелка;
        }

        private object Стрелки(ASTNode node)
        {
            //Стрелки = Стрелка ', Стрелки | Стрелка ;
            List<Конус> результат = new List<Конус>();
            switch (node.RuleTerm)
            {
                case "Стрелка ', Стрелки":
                    {
                        Конус стрелка = Translate(node.Childs["Стрелка"]) as Конус;
                        List<Конус> стрелки = Translate(node.Childs["Стрелки"]) as List<Конус>;
                        результат.Add(стрелка);
                        результат.AddRange(стрелки);
                    }; break;
                case "Стрелка":
                    {
                        Конус стрелка = Translate(node.Childs["Стрелка"]) as Конус;
                        результат.Add(стрелка);
                    }; break;
                default: throw new ArgumentException("Неизвестный терм " + node.RuleTerm + " правила: " + node.RuleName);
            }
            return результат;
        }

        private object Область(ASTNode node)
        {
            //Область = Ноль | '{ Стрелки }' ;
            List<Конус> результат = new List<Конус>();
            switch (node.RuleTerm)
            {
                case "Ноль": return результат;
                case "'{ Стрелки }'":
                    {
                        результат.AddRange(Translate(node.Childs["Стрелки"]) as List<Конус>);
                    }; break;
                default: throw new ArgumentException("Неизвестный терм " + node.RuleTerm + " правила: " + node.RuleName);
            }
            return результат;
        }

        private object Кообласть(ASTNode node)
        {
            //Кообласть = Ноль | '{ Стрелки '} ;
            List<Конус> результат = new List<Конус>();
            switch (node.RuleTerm)
            {
                case "Ноль": return результат;
                case "'{ Стрелки '}":
                    {
                        результат.AddRange(Translate(node.Childs["Стрелки"]) as List<Конус>);
                    }; break;
                default: throw new ArgumentException("Неизвестный терм " + node.RuleTerm + " правила: " + node.RuleName);
            }
            return результат;
        }

        private object Слова(ASTNode node)
        {
            //Слова = Слово ', Слова | Слово ;
            List<Конус> результат = new List<Конус>();
            результат.Add(Translate(node.Childs["Слово"]) as Конус);
            switch (node.RuleTerm)
            {
                case "Слово ', Слова": 
                    {
                        результат.AddRange(Translate(node.Childs["Слова"]) as List<Конус>);
                    }; break;
                case "Слово": break;
                default: throw new ArgumentException("Неизвестный терм " + node.RuleTerm + " правила: " + node.RuleName);
            }
            return результат;
        }

        //private object Функтор(ASTNode node)
        //{
        //    //Функтор = '! Слово ;
        //    List<Конус> результат  = new List<Конус>();
        //    switch (node.RuleTerm)
        //    {
        //        case "'! Слово":
        //            {
        //                Конус слово = Translate(node.Childs["Слово"]) as Конус;
        //                результат.AddRange(DataModelLibrary.Функтор.ВычислитьФунктор(слово));
        //            }; break;
        //        default: throw new ArgumentException("Неизвестный терм " + node.RuleTerm + " правила: " + node.RuleName);
        //    }
        //    return результат;
        //}

        private object Слово(ASTNode node)
        {
            //Слово = '[ Слова '] | Символы ;
            switch (node.RuleTerm)
            {
                case "'[ Слова ']": return Storage.AddLocal(Translate(node.Childs["Слова"]) as List<Конус>);
                case "Символы": return Storage.AddLocal(Translate(node.Childs["Символы"]) as string);
                default:  throw new Exception("Неизвестный терм правила " + node.RuleName + ": " + node.RuleTerm);
            }
        }
       
        private object Символы(ASTNode node)
        {
             //Символы = Символ Символы | Символ ;
            StringBuilder s = new StringBuilder();
            node.Childs.ToList().ForEach(childNode => s.Append(Translate(childNode.Value) as string));
            return s.ToString();
        }

        private object Символ(ASTNode node)
        {
            //Символ = Буква | Цифра | Знак ;
            return Translate(node.Childs.First().Value) as string;
        }

        private object Буква(ASTNode node)
        {
            //Буква = 'a|'b|'c|'d|'e|'f|'g|'h|'i|'j|'k|'l|'m|'n|'o|'p|'q|'r|'s|'t|'u|'v|'w|'x|'y|'z|'A|'B|'C|'D|'E|'F|'G|'H|'I|'J|'K|'L|'M|'N|'O|'P|'Q|'R|'S|'T|'U|'V|'W|'X|'Y|'Z|
            //        'а|'б|'в|'г|'д|'е|'ё|'ж|'з|'и|'й|'к|'л|'м|'н|'о|'п|'р|'с|'т|'у|'ф|'х|'ц|'ч|'ш|'щ|'ь|'ы|'ъ|'э|'ю|'я|'А|'Б|'В|'Г|'Д|'Е|'Ё|'Ж|'З|'И|'Й|'К|'Л|'М|'Н|'О|'П|'Р|'С|'Т|'У|'Ф|'Х|'Ц|'Ч|'Ш|'Ь|'Щ|'Ъ|'Ы|'Э|'Ю|'Я|'_ ;
            return node.Childs.First().Value.Text.Value;
        }

        private object Цифра(ASTNode node)
        {
            //Цифра = '0|'1|'2|'3|'4|'5|'6|'7|'8|'9 ;
            return node.Childs.First().Value.Text.Value;
        }

        private object Знак(ASTNode node)
        {
            //Знак = '< | '> | '* | '+ | '- | '~ | '~:'| 'id | '_ | '>:;
            return node.Childs.First().Value.Text.Value;
        }


        private object Ноль(ASTNode node)
        {
            //Ноль = '{ '} ;
            return new List<Конус>();
        }
    }
}
