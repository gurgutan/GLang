using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModelLibrary;


namespace Interpreter
{
    //TODO: Проработать вопрос памяти процессора. Должна быть локальная память, постоянное хранилище,состояния для выполнения запроса

    //TODO: Процессор оперирует двумя грамматиками: грамматика описания и грамматика исполнения запроса. Одна распознающая,
    //другая - порождающая. Эти грамматики связаны функтором. Функтор задает связь от каждого правила распознающей грамматики
    //к правилам порождающей. Сделать возможным строить описание функтора между грамматиками.
    public class Processor
    {
        public int Error = 0;
        public Processor Parent;
        public BNFGrammar Grammar;

        public Processor(BNFGrammar grammar)
        {
            Grammar = grammar;
        }

        public Processor(Processor parent, BNFGrammar grammar)
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
            //TODO: Проработать вопрос удаления слов, избавиться от лишних скобок
            //Домен = Слово '{ Значения '} | Слово '= '{ Значения '} | Слово ;
            //Значения = Домен ', Значения |'{ Операция '} ', Значения | Домен | '{ Операция '} | '{ '} ;
            //Операция = ЛевыйОперанд '-< ЦентральныйОперанд '>- ПравыйОперанд |
            //  ЛевыйОперанд БинарнаяОперация ПравыйОперанд |
            //  ЛевыйОперанд УнарнаяОперация 
            // ;
            //ЛевыйОперанд = '|> Значения ;
            //ПравыйОперанд = Значения ;
            //ЦентральныйОперанд = Значения ;
            //БинарнаяОперация = '<- | '-> | '+ | '- | '* | '~ | '^;
            //УнарнаяОперация = '>: | '<: | '< УнарнаяОперация | '> УнарнаяОперация | '< | '> ;
            //Слово = '[ ЭлементыСлова ']  | Символ | '_ ;
            //ЭлементыСлова = Слово ', ЭлементыСлова | Слово ; 
            //Символ = Буква Символ | Цифра Символ | Буква | Цифра ;
            //Буква = 'a|'b|'c|'d|'элементы|'f|'g|'h|'i|'j|'k|'l|'m|'n|'o|'p|'q|'r|'s|'t|'u|'v|'w|'x|'y|'z|
            //'A|'B|'C|'D|'E|'F|'G|'H|'I|'J|'K|'L|'M|'N|'O|'P|'Q|'R|'S|'T|'U|'V|'W|'X|'Y|'Z|
            //'а|'б|'в|'г|'д|'е|'ё|'ж|'з|'и|'й|'к|'л|'м|'н|'о|'п|'р|'с|'т|'у|'ф|'х|'ц|'ч|'ш|'щ|'ь|'ы|'ъ|'э|'ю|'я|
            //'А|'Б|'В|'Г|'Д|'Е|'Ё|'Ж|'З|'И|'Й|'К|'Л|'М|'Н|'О|'П|'Р|'С|'Т|'У|'Ф|'Х|'Ц|'Ч|'Ш|'Ь|'Щ|'Ъ|'Ы|'Э|'Ю|'Я|
            //'_;
            //Цифра = '0|'1|'2|'3|'4|'5|'6|'7|'8|'9 ;
            Error = 0;
            if (!node.IsMatch(Grammar) && node.Type == ASTType.NonterminalEqual)
                throw new Exception("Ошибка трансляции: правило " + node.RuleName + " = " + node.RuleTerm + " не соответствует грамматике");
            switch (node.RuleName)
            {
                case "Домен": return ToDomain(node);
                case "Значения": return ToValues(node);
                case "Операция": return ToOperation(node);
                case "ЛевыйОперанд": return ToLeftOperand(node);
                case "ПравыйОперанд": return ToRightOperand(node);
                case "ЦентральныйОперанд": return ToCentralOperand(node);
                case "БинарнаяОперация": return ToBinaryOperation(node);
                case "УнарнаяОперация": return ToUnaryOperation(node);
                case "Слово": return ToWord(node);
                case "ЭлементыСлова": return ToWordElements(node);
                case "Символ": return ToSymbol(node);
                case "Буква": return ToLetter(node);
                case "Цифра": return ToDigit(node);
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
                case "'^": return "^";
                case "'_": return "_";
                default:
                    {
                        Error = 256;    //Неизвестный токен
                        throw new ArgumentException("Неизвестныое правило: " + node.RuleName);
                    }
            }
        }

        private Конус ToDomain(ASTNode node)
        {
            //Домен = Слово '{ Значения '} | Слово '= '{ Значения '} | Слово ;
            Конус w = Translate(node.Childs["Слово"]) as Конус;
            if (node.RuleTerm == "Слово '{ Значения '}")    //Добавление слов в домен
            {
                IEnumerable<Конус> words = Translate(node.Childs["Значения"]) as IEnumerable<Конус>;
                w.ДобавитьВКообласть(words);
            }
            else if (node.RuleTerm == "Слово '= '{ Значения '}")    //Замена домена слова
            {
                IEnumerable<Конус> words = Translate(node.Childs["Значения"]) as IEnumerable<Конус>;
                w.Кообласть.Clear();
                w.ДобавитьВКообласть(words);
            }
            return w;
        }

        private IEnumerable<Конус> ToValues(ASTNode node)
        {
            //Значения = Домен ', Значения | '{ Операция '} ', Значения | Домен | '{ Операция '} | '{ '} ;
            List<Конус> words = new List<Конус>();
            if (node.RuleTerm == "Домен ', Значения")
            {
                words.Add(Translate(node.Childs["Домен"]) as Конус);
                words.AddRange(Translate(node.Childs["Значения"]) as IEnumerable<Конус>);
            }
            else if (node.RuleTerm == "Операция ', Значения")
            {
                words.AddRange(Translate(node.Childs["Операция"]) as IEnumerable<Конус>);
                words.AddRange(Translate(node.Childs["Значения"]) as IEnumerable<Конус>);
            }
            else if (node.RuleTerm == "Домен")
            {
                words.Add(Translate(node.Childs["Домен"]) as Конус);
            }
            else if (node.RuleTerm == "Операция")
            {
                words.AddRange(Translate(node.Childs["Операция"]) as IEnumerable<Конус>);
            }
            else if (node.RuleTerm == "'{ '}")
            {
                //Оставляет список слов пустым
            }
            return words;
        }

        private IEnumerable<Конус> ToOperation(ASTNode node)
        {
            //Операция = ЛевыйОперанд '-< ЦентральныйОперанд '>- ПравыйОперанд |
            //  ЛевыйОперанд БинарнаяОперация ПравыйОперанд |
            //  ЛевыйОперанд УнарнаяОперация | 
            //  Значения ;
            IEnumerable<Конус> result = new List<Конус>();
            //Тернарная операция Join соединяет домен ЛевыйОперанд с доменом ПравыйОперанд. ЦентральныйОперанд задает
            //условие соединения, которое может быть список слов или шаблоном
            if (node.RuleTerm == "ЛевыйОперанд '-< ЦентральныйОперанд '>- ПравыйОперанд")
            {
                IEnumerable<Конус> left = Translate(node.Childs["ЛевыйОперанд"]) as IEnumerable<Конус>;
                IEnumerable<Конус> center = Translate(node.Childs["ЦентральныйОперанд"]) as IEnumerable<Конус>;
                IEnumerable<Конус> right = Translate(node.Childs["ПравыйОперанд"]) as IEnumerable<Конус>;
                //Перечечение двух таблиц left и right. В новую таблицу попадают слова, которые соответствуют шаблону center
                result = left.Join(right,
                    l => l.СоответствуетШаблону(center) ? "-" : "А",
                    r => r.СоответствуетШаблону(center) ? "-" : "Б",
                    (l, r) => Storage.AddLocal(new Конус[] { l, r }));
            }
            else if (node.RuleTerm == "Значения")
            {
                result = Translate(node.Childs["Значения"]) as IEnumerable<Конус>;
            }
            else if (node.RuleTerm == "ЛевыйОперанд '|> БинарнаяОперация ПравыйОперанд")
            {
                IEnumerable<Конус> left = Translate(node.Childs["ЛевыйОперанд"]) as IEnumerable<Конус>;
                IEnumerable<Конус> right = Translate(node.Childs["ПравыйОперанд"]) as IEnumerable<Конус>;
                string operation = Translate(node.Childs["БинарнаяОперация"]) as string;
                switch (operation)
                {
                    case "+": result = left.Union(right); break;    //Объединение
                    case "-": result = left.Except(right); break;   //Разность
                    case "*":
                        {
                            result = left.Join(right, l => true, r => true, (l, r) => Storage.AddLocal(new Конус[] { l, r }));
                        }; break;  //Произведение
                    case "^": result = left.Intersect(right); break;    //Пересечение
                    case "~": result = left.Where(w => w.СоответствуетШаблону(right)); break; //Слова домена ЛевыйОперанд по шаблону ПравыйОперанд
                    case "<-": throw new NotImplementedException();
                    case "->": throw new NotImplementedException();
                    default: throw new Exception("Неизвестная операция: " + operation + " в правиле " + node.RuleName + ":" + node.RuleTerm);
                }
            }
            else if (node.RuleTerm == "ЛевыйОперанд УнарнаяОперация")
            {
                IEnumerable<Конус> left = Translate(node.Childs["ЛевыйОперанд"]) as IEnumerable<Конус>;
                string operation = Translate(node.Childs["УнарнаяОперация"]) as string;
                List<Конус> words = new List<Конус>();
                words.AddRange(left);
                foreach (string o in operation.Split(','))
                {
                    switch (o)
                    {
                        case "<":
                            {
                                words = words.SelectMany(d => d.ПолучитьКообласть().Values).ToList();
                            }; break;
                        case ">":
                            {
                                words = words.SelectMany(d => d.ПолучитьОбласть().Values).ToList();
                            }; break;
                        case "<:":
                            {
                                words = words.SelectMany(d => d.ПолучитьОбласть().Values).ToList();
                            }; break;
                        case ">:":
                            {
                                words = words.SelectMany(d => d.ПолучитьОбласть().Values).ToList();
                            }; break;
                        default: throw new Exception("Неизвестная операция: " + operation + " в правиле " + node.RuleName + ":" + node.RuleTerm);
                    }
                }
                result = words;
            }
            return result;
        }

        private string ToUnaryOperation(ASTNode node)
        {
            //УнарнаяОперация = '< УнарнаяОперация | '> УнарнаяОперация | '< | '> ;
            string suffix = "";
            if (node.Childs.ContainsKey("УнарнаяОперация"))
            {
                suffix = "," + ToUnaryOperation(node.Childs["УнарнаяОперация"]) as string;
            }
            return (Translate(node.Childs.Values.First()) as string) + suffix;
        }

        private string ToBinaryOperation(ASTNode node)
        {
            //БинарнаяОперация = '<- | '-> | '+ | '- | '* | '~ | '^ ;
            return Translate(node.Childs.Values.First()) as string;
        }

        private IEnumerable<Конус> ToCentralOperand(ASTNode node)
        {
            return Translate(node.Childs["Значения"]) as IEnumerable<Конус>;
        }

        private IEnumerable<Конус> ToRightOperand(ASTNode node)
        {
            //ПравыйОперанд = Значения ;
            return Translate(node.Childs["Значения"]) as IEnumerable<Конус>;
        }

        private IEnumerable<Конус> ToLeftOperand(ASTNode node)
        {
            //ЛевыйОперанд = Значения ;
            return Translate(node.Childs["Значения"]) as IEnumerable<Конус>;
        }

        private Конус ToWord(ASTNode node)
        {
            //Слово = '[ ЭлементыСлова '] | Символ | '_ ;
            Конус w;
            if (node.RuleTerm == "'[ ЭлементыСлова ']")
            {
                w = Storage.AddLocal(ToWordElements(node.Childs["ЭлементыСлова"]));
            }
            else if (node.RuleTerm == "Символ")
            {
                w = Storage.AddLocal(ToSymbol(node.Childs["Символ"]));
            }
            else if (node.RuleTerm == "'_")
            {
                w = Storage.AddLocal("_");
            }
            else throw new Exception("Неизвестный вариант правила " + node.RuleName + ": " + node.RuleTerm);
            //w = Associate(Memory.Local, w);         //Все созданные слова записываем в локальную память
            return w;
        }

        private List<Конус> ToWordElements(ASTNode node)
        {
            //ЭлементыСлова = Слово ', ЭлементыСлова | Слово  ; 
            List<Конус> e = new List<Конус>();
            if (node.RuleTerm == "Слово")
            {
                e.Add(ToWord(node.Childs["Слово"]));
            }
            else if (node.RuleTerm == "Слово ', ЭлементыСлова")
            {
                e.Add(ToWord(node.Childs["Слово"]));
                e.AddRange(ToWordElements(node.Childs["ЭлементыСлова"]));
            }
            else throw new Exception("Неизвестный вариант правила " + node.RuleName + ": " + node.RuleTerm);
            return e;
        }

        private string ToSymbol(ASTNode node)
        {
            //Символ = Буква Символ | Цифра Символ | Буква | Цифра ;
            StringBuilder s = new StringBuilder();
            node.Childs.ToList().ForEach(childNode => s.Append(Translate(childNode.Value) as string));
            return s.ToString();
        }

        private string ToLetter(ASTNode node)
        {
            return node.Childs.First().Value.Text.Value;
        }

        private string ToDigit(ASTNode node)
        {
            return node.Childs.First().Value.Text.Value;
        }

    }
}
