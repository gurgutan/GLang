﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModelLibrary
{
    public static class Engine
    {
        public static class Config
        {
            public const int MaxDepth = 256;
        }

    }

    //TODO: Добавить сериализацию
    public static class Storage
    {
        public static Dictionary<string, Конус> Operative = new Dictionary<string, Конус>();
        public static Dictionary<string, Конус> Local = new Dictionary<string, Конус>();
        public static Dictionary<string, Конус> Output = new Dictionary<string, Конус>();

        public static void Clear()
        {
            Operative.Clear();
            Local.Clear();
            Output.Clear();
        }

        public static Конус AddLocal(Конус word)
        {
            //Пробуем вычислить морфизм как функтор
            if (Функтор.ЭтоФунктор(word))
            {
                //TODO: определить условия вычисления функтора: если он был ранее вычислен, нужно ли его вычислять заново?
                //Вероятно нужно определить функторы, которые зависят от области и кообласти - их вычислять надо всегда.
                //Остальные функторы вычислять можно один раз.
                Dictionary<string, Конус> результат = Функтор.ВычислитьФунктор(word);
                foreach (string key in результат.Keys)
                    Storage.AddLocal(результат[key]);
                word.ДобавитьВКообласть(результат.Values);
                Local[word.Имя] = word;
            }
            if (!Local.ContainsKey(word.Имя))
                Local[word.Имя] = word;
            return Local[word.Имя];
        }

        public static Конус AddLocal(string _name)
        {
            if (Local.ContainsKey(_name)) return Storage.Local[_name];
            else
            {
                Local[_name] = new Конус(_name);
                return Local[_name];
            };
        }

        public static Конус AddLocal(IEnumerable<Конус> _elements)
        {
            Конус word = new Конус(_elements);
            return AddLocal(word);
        }
    }

    /// <summary>
    /// Тип Конус - основной класс для определения морфизма в категории. Так как данный класс содержит определение
    /// области и кообласти как коллекции объектов (а не одного объекта области и одного объекта кообласти, как в классической 
    /// теории категорий), то данный класс можно считать абстракцией для категории (множества объектов и множества морфизмов).
    /// Однако данная категория состоит из множества объектов и всего одного морфизма.
    /// Конус имеет уникальный идентификатор - Имя. В зависимости от структуры морфизма, Имя может быть простым (односложным), состоящим из 
    /// одного символа, или сложным - состоящим из упорядоченных имен других морфизмов.
    /// Составной морфизм имеет структуру направленного графа - элементы данного морфизма - морфизмы, которые также могут быть составными и
    /// в составе иметь другие морфизмы, в том числе и данный (т.е. граф может иметь циклы).
    /// Конус имеет Область(область определения) - множество объектов (морфизмов) из которых есть стрелка (морфизм) в данный, и 
    /// Кообласть (множество значений) - множество объектов в которые есть стрелка из данного.
    /// </summary>
    public class Конус
    {
        public static string Разделитель = ",";
        public static string ЛеваяСкобка = "[";
        public static string ПраваяСкобка = "]";
        public static string ЛюбойСимвол = "_";

        private int RecursionDepth = 0;

        private readonly string имя;
        public string Имя => имя;

        private readonly Конус[] элементы;
        public Конус[] Элементы => элементы;

        private readonly Dictionary<string, Конус> кообласть;
        public Dictionary<string, Конус> Кообласть => кообласть;

        private readonly Dictionary<string, Конус> область;
        public Dictionary<string, Конус> Область => область;

        //Конструкторы
        public Конус(string _name)
        {
            if (_name.Contains(Разделитель) || _name.Contains(ЛеваяСкобка) || _name.Contains(ПраваяСкобка))
                throw new ArgumentException("Имя слова не должно содержать символов разделителей и ограничителей: " + _name);
            имя = _name;
            элементы = new Конус[] { };
            кообласть = new Dictionary<string, Конус>();
            область = new Dictionary<string, Конус>();
        }

        public Конус(Конус w)
        {
            имя = w.Имя;
            элементы = new Конус[w.Элементы.Length];
            w.Элементы.CopyTo(элементы, 0);
            кообласть = w.Кообласть.Values.ToDictionary(v => v.Имя);
            область = w.Область.Values.ToDictionary(v => v.Имя);
        }

        public Конус(IEnumerable<Конус> _elements)
        {
            if (_elements.Count() == 1)
            {
                элементы = new Конус[] { };
                имя = _elements.First().Имя;
            }
            else
            {
                элементы = _elements.ToArray();
            };
            имя = ПолучитьИмя();
            кообласть = new Dictionary<string, Конус>();
            область = new Dictionary<string, Конус>();
        }

        public bool Equals(Конус w)
        {
            return w.Имя == Имя;
        }

        public override int GetHashCode()
        {
            return Имя.GetHashCode();
        }

        //Атом - морфизм, не имеющий элементов
        public bool ЭтоАтом()
        {
            return (Элементы.Length == 0);
        }

        public bool ЭтоШаблон()
        {
            if (Элементы.Length == 0 && Имя == "_") return true;
            return Элементы.Any(e => e.Имя == "_");
        }

        //Добавление слова w в домен Кообласть
        public void ДобавитьВКообласть(Конус w)
        {
            Кообласть[w.Имя] = w;
            w.Область[Имя] = this;
        }

        //Добавление коллекции слов в Кообласть
        public void ДобавитьВКообласть(IEnumerable<Конус> words)
        {
            if (words == null) return;
            foreach (Конус w in words)
                ДобавитьВКообласть(w);
        }

        public void ДобавитьВОбласть(Конус w)
        {
            Область[w.Имя] = w;
            w.Кообласть[Имя] = this;
        }

        public void ДобавитьВОбласть(IEnumerable<Конус> words)
        {
            if (words == null) return;
            foreach (Конус w in words)
                ДобавитьВОбласть(w);
        }

        //Удаление слова из кообласти
        public void УдалитьИзКообласти(Конус w)
        {
            if (Кообласть.Remove(w.Имя))
                w.Область.Remove(Имя);
        }

        public void УдалитьИзКообласти(IEnumerable<Конус> words)
        {
            foreach (Конус w in words)
                УдалитьИзКообласти(w);
        }

        public void ОчиститьКообласть()
        {
            УдалитьИзКообласти(Кообласть.Values);
        }

        //Удаление слова из области
        public void УдалитьИзОбласти(Конус w)
        {
            if (Область.Remove(w.Имя))
                w.Кообласть.Remove(Имя);
        }

        public void УдалитьИзОбласти(IEnumerable<Конус> words)
        {
            foreach (Конус w in words)
                УдалитьИзКообласти(w);
        }

        public void ОчиститьОбласть()
        {
            УдалитьИзОбласти(Область.Values);
        }

        //Специальные методы для контроля рекурсии
        //Метод возвращает истину, если в данном объекте не достигнута максимальная глубина рекурсии и ложь - в противном случае.
        public bool IsRecursionAllowed()
        {
            if (RecursionDepth > Engine.Config.MaxDepth) return false;
            return true;
        }

        //Метод вызывается при вызове любого метода объекта, увеличивающего глубину рекурсии
        public bool RecursionStarted()
        {
            RecursionDepth++;
            return true;
        }

        public bool RecursionEnded()
        {
            RecursionDepth--;
            return true;
        }

        //Функция сравнения с шаблоном слова возвращает истину если:
        // - шаблон w - это символ '_' ("любой");
        // - все элементы шаблона совпадают кроме '_'.
        public bool СоответствуетШаблону(Конус w)
        {
            if (Функтор.ЭтоФунктор(w))
            {
                Dictionary<string, Конус> шаблоны = Функтор.ВычислитьФунктор(w);
                return СоответствуетШаблону(шаблоны.Values);
            }
            else
            {
                if (w.Имя == ЛюбойСимвол) return true;   //для символа шаблона "любой" функция возвращает истину
                if (Элементы.Length != w.Элементы.Length) return false;  // Если количество элементов в словах не равно, то слова не равны
                if (Имя == w.Имя) return true;        //слова равны, если их строковые представления равны
                if (Элементы.Length == 0) return false; //если слово односложное и имена не равны, то слова не равны
                // Поэлементное сравнение 
                for (int i = 0; i < Элементы.Length; i++)
                    if (!Элементы[i].СоответствуетШаблону(w.Элементы[i])) return false;
                return true;    //Поэлементное сравнение дало положительный ответ
            }
        }

        //Возвращает истину, если слово соответствует одному из шаблонов words
        public bool СоответствуетШаблону(IEnumerable<Конус> words)
        {
            if (words.Count() == 0) return true;
            return words.Any(p => СоответствуетШаблону(p));
        }

        //Возвращает шаблоны, для которых найдено соответствие. Если не найдено - возвращет пустую коллекцию
        public IEnumerable<Конус> СоответствующиеШаблоны(IEnumerable<Конус> words)
        {
            return words.Where(p => СоответствуетШаблону(p));
        }

        public override string ToString()
        {
            return Имя;
        }

        private string ПолучитьИмя()
        {
            if (элементы.Length == 0) return имя;
            return ЛеваяСкобка +
                   элементы.Aggregate<Конус, string>("", (curr, next) => curr == "" ? next.Имя : curr + Разделитель + next.Имя) +
                   ПраваяСкобка;
        }

        //Функция составляет список слов, которые являются кодоменами для слова word
        public Dictionary<string, Конус> ПолучитьОбласть()
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>
            {
                //TODO: Единичный морфизм (id) не всегда полезен, так как оставляет соответствующий морфизм в пределе и копределе, а
                //поиск в диаграмме, часто, должен оставлять только "терминальные" объекты. Нужно решить как по желанию отказываться
                //от id - в синтаксисе, через спец-символ (например, @ - абстрактный) или оставить добавление id каждый раз на усмотрение
                //пользователя. При этом, отсутствие id в конусе может привести к "обнулению" декартова произведения конусов, что не даст
                //создать фильтр конуса (поиск нужных морфизмов в кообластях и областях конусов).

                { Имя, this }
            };
            ;  //предполагаем, что единичный морфизм всегда существует
            //Если у слова область не пустой, добавляем его к результату
            if (Область.Count > 0) foreach (string key in Область.Keys) результат[key] = Область[key];
            //Если слово - символ, то есть односложное, то возвращаем результат (дальнейшие вычисления не нужны)
            if (Элементы.Length == 0) return результат;
            //Создаем массив массивов для хранения наборов областей каждого элемента слова
            Конус[][] области = new Конус[Элементы.Length][];
            for (int k = 0; k < Элементы.Length; k++)
            {
                //Создаем массив кодоменов k-го элемента слова
                Dictionary<string, Конус> _область = Элементы[k].ПолучитьОбласть();
                //Если создание массива областей для k-го элемента неуспешное (у k-го элемента нет областей), 
                //то и создание областей всего слова неуспешно - возвращаем пустой список.
                if (_область.Count() == 0)
                {
                    return результат;
                }
                else
                    //Добавляем в массив новый массив областей для k-го элемента
                    области[k] = _область.Values.ToArray();
            }
            IEnumerable<Конус> произведение = ДекартовоПроизведение(области);
            foreach (Конус m in произведение) результат[m.Имя] = m;
            return результат;
        }

        //Функция составляет список слов, которые являются кодоменами для слова word
        public Dictionary<string, Конус> ПолучитьКообласть()
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>
            {
                //Предполагаем, что единичный морфизм всегда существует
                { Имя, this }
            };
            //Если у слова кообласть не пустая, добавляем её к результату
            if (Кообласть.Count > 0)
                foreach (string key in Кообласть.Keys) результат[key] = Кообласть[key];
            //Если слово - символ, то есть односложное, то возвращаем результат (дальнейшие вычисления не нужны)
            if (Элементы.Length == 0) return результат;
            //Создаем массив массивов для хранения наборов кообластей каждого элемента слова
            Конус[][] кообласти = new Конус[Элементы.Length][];
            for (int k = 0; k < Элементы.Length; k++)
            {
                //Создаем массив кодоменов k-го элемента слова
                Dictionary<string, Конус> _кообласть = Элементы[k].ПолучитьКообласть();
                //Если создание массива кодоменов для k-го элемента неуспешное (у k-го элемента нет кообластей), 
                //то и создание кодоменов всего слова неуспешно - возвращаем пустой список.
                if (_кообласть.Count() == 0) return результат;
                //Добавляем в массив новый массив кообластей для k-го элемента
                кообласти[k] = _кообласть.Values.ToArray();
            }
            IEnumerable<Конус> произведение = ДекартовоПроизведение(кообласти);
            foreach (Конус m in произведение) результат[m.Имя] = m;
            return результат;
        }

        private List<Конус> ДекартовоПроизведение(Конус[][] области)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            //Для навигации по массиву наборов областей нужен массив индексов - регистр[].
            //Значение элемента массива - регистр[i] - это номер области i-го элемента слова.
            //Максимальное значение регистр[i] - количество кодоменов в области[i].
            //Следующий цикл обеспечивает перебор всех вариантов компоновок областей. 
            //Алгоритм изменения регистра подобен увеличению на единицу числа в многоосновной позиционной системе исчисления,
            //в которой i-й разряд увеличивается при достижении (i+1)-го разряда максимального значения (которое задается количеством кодоменов i+1-го элемента).
            int[] регистр = new int[области.Length]; //массив разрядов - регистр (инициализирован нулями)
            int i = 0;    //номер разряда
            do
            {
                //Увеличиваем номер разряда, двигаясь к самому последнему (младшему) разряду регистра
                if (i < области.Length - 1)
                    i++;
                else
                    //Если мы дошли до последнего (младшего) разряда и его можно увеличить на 1
                    if (регистр[i] < области[i].Length)
                {
                    //Создадим слово из элементов с текущими значениями разрядов
                    Конус w = Storage.AddLocal(регистр.Select((c, j) => области[j][c]));
                    //Добавим слово в коллекцию результата
                    результат[w.Имя] = w;
                    //Увеличиваем последний разряд на 1
                    регистр[i]++;
                }
                else //Если последний разряд регистра увеличить нельзя
                {
                    do
                    {
                        регистр[i] = 0;          //обнуляем последний разряд
                        i--;                     //возвращаемся к старшему разряду
                        if (i >= 0) регистр[i]++;   //увеличиваем (i-1)-й разряд на 1
                    } while (i >= 0 && регистр[i] == области[i].Length);
                }
            } while (i >= 0);
            //Если результат не содержит ни одного кодомена - возвращаем пустой список
            return результат.Values.ToList();
        }
    }

    //-------------------------------------------------------------------------------------------------------
    //Функтор (эндофунктор) - морфизм, определенный на морфизмах.
    public static class Функтор
    {
        //TODO: Конструирование функторов
        private static readonly string[] именаФункторов = new string[]
        {
            ">",    // получает область морфизма
            "<",    // получает кообласть морфизма
            "*",    // получает произведение морфизмов
            "+",    // получет сумму морфизмов
            "-",    // получает разность морфизмов
            "^",    // получает пересечение морфизмов
            "~",    // получает выбор морфизмов по шаблону
            "~:",   // получает проекцию морфизмов по шаблону
            ">:",   // получает копредел
            "<:",   // получает предел
            "id",   // тождество
            //"_",    // любой
        };

        public enum Тип
        {
            ВычислениеОбласти = 0,
            ВычислениеКообласти = 1,
            ДекартовоПроизведение = 2,
            Сумма = 3,
            Разность = 4,
            Пересечение = 5,
            ВыборПоШаблону = 6,
            Копредел = 7,
            Предел = 8,
            ПроекцияПоШаблону = 9,
            Тождество = 10,
            // Любой = 1,
        }

        private static readonly Dictionary<string, Конус> ФункторПоСимволу = new Dictionary<string, Конус>(
            именаФункторов.Select(n => new Конус(n)).ToDictionary(m => m.Имя));
        private static readonly Dictionary<Тип, Конус> ФункторПоТипу = new Dictionary<Тип, Конус>(
            именаФункторов.Select((n, i) => new { морфизм = new Конус(n), индекс = i }).
            ToDictionary(m => (Тип)m.индекс, m => m.морфизм));

        //private static readonly Конус[] ШаблоныФункторов = new Конус[]
        //    {
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.ВычислениеОбласти],new Конус("_") }),  // шаблон [_,>,_]
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.ВычислениеКообласти],new Конус("_") }),// шаблон [_,<,_] - кообласть
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.ДекартовоПроизведение],new Конус("_") }), // шаблон [_,*,_] - произведение
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.Сумма],new Конус("_") }),    // шаблон [_,+,_] - сумма
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.Разность],new Конус("_") }),    // шаблон [_,-,_] - разность
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.Пересечение],new Конус("_") }),    // шаблон [_,^,_] - пересечение
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.ВыборПоШаблону],new Конус("_") }),    // шаблон [_,~,_] - выбор по шаблону
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.ПроекцияПоШаблону],new Конус("_") }), // шаблон [_,~:,_] - выбор по шаблону
        //        new Конус(new Конус[] { new Конус("_"), ФункторПоТипу[Тип.Тождество]}),          // шаблон [_,id] - тождество
        //    };

        //Метод возвращает true, если данный морфизм является эндофунктором. 
        public static bool ЭтоФунктор(Конус m)
        {
            return m.Элементы.Any(p => ЭтоИмяФунктора(p.Имя));
        }

        public static bool ЭтоИмяФунктора(string _name)
        {
            return именаФункторов.Contains(_name);
        }

        public static Конус СоздатьФунктор(Функтор.Тип тип, Конус[] морфизмы)
        {
            switch (тип)
            {
                case Тип.ВычислениеКообласти: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.ВычислениеКообласти], морфизмы[1] });
                case Тип.ВычислениеОбласти: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.ВычислениеОбласти], морфизмы[1] });
                case Тип.ДекартовоПроизведение: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.ДекартовоПроизведение], морфизмы[1] });
                case Тип.Сумма: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.Сумма], морфизмы[1] });
                case Тип.Разность: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.Разность], морфизмы[1] });
                case Тип.Пересечение: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.Пересечение], морфизмы[1] });
                case Тип.ВыборПоШаблону: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.ВыборПоШаблону], морфизмы[1] });
                case Тип.ПроекцияПоШаблону: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.ПроекцияПоШаблону], морфизмы[1] });
                case Тип.Тождество: return new Конус(new Конус[] { морфизмы[0], ФункторПоТипу[Тип.Тождество], морфизмы[1] });
                default: throw new ArgumentException("Неизвестный тип функтора: " + тип.ToString());
            }
        }

        //Результатом вычисления кообласти является коллекция морфизмов, полученная в результате применения функуторов,
        //являющихся элементами морфизма, другим элементам морфизма.
        //public static List<Конус> Вычислить(Конус f)
        //{
        //    List<Конус> результат = new List<Конус>();
        //    Конус шаблон = ШаблоныФункторов.FirstOrDefault(p => f.СоответствуетШаблону(p));
        //    //Если соответствующий f шаблон функтора не найден, возвращаем морфизм как есть (соответствует тождественному функтору)
        //    if (шаблон == null)
        //    {
        //        результат.Add(f);
        //        return результат;
        //    }
        //    //Здесь используется инфиксная для 2-функторов и постфиксная для 1-функторов запись.
        //    //Поэтому второй элемент шаблона (с индексом 1) содержит символ типа функтора.
        //    switch (шаблон.Элементы[1].Имя)
        //    {
        //        case "id":
        //            {
        //                результат = Вычислить(f.Элементы[0]);
        //            } break;
        //        case ">":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьОбласть(левыйОперанд, правыйОперанд);
        //            } break;
        //        case "<":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьКообласть(левыйОперанд, правыйОперанд);
        //            }; break;
        //        case "*":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьПроизведение(левыйОперанд, правыйОперанд);
        //            }; break;
        //        case "+":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьСумму(левыйОперанд, правыйОперанд);
        //            }; break;
        //        case "-":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьРазность(левыйОперанд, правыйОперанд);
        //            }; break;
        //        case "^":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьПересечение(левыйОперанд, правыйОперанд);
        //            }; break;
        //        case "~:":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьПроекцию(левыйОперанд, правыйОперанд);
        //            }; break;
        //        case "~":
        //            {
        //                List<Конус> левыйОперанд = Вычислить(f.Элементы[0]);
        //                List<Конус> правыйОперанд = Вычислить(f.Элементы[2]);
        //                результат = ВычислитьВыбор(левыйОперанд, правыйОперанд);
        //            }; break;
        //        default:
        //            {
        //                результат.Add(f);
        //            }; break;
        //    }
        //    return результат;
        //}

        //Метод использует слово как ленту конвеера для выполнения вычислений над словами-элементами исходного слова
        public static Dictionary<string, Конус> ВычислитьФунктор(Конус f)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            if (f.ЭтоАтом() || f.ЭтоШаблон())
            {
                результат.Add(f.Имя, f);
                return результат;
            }
            Dictionary<string, Конус>[] результаты = new Dictionary<string, Конус>[f.Элементы.Length];   //массив списков результатов
            for (int i = 0; i < f.Элементы.Length; i++)
            {
                Конус e = f.Элементы[i];
                результаты[i] = new Dictionary<string, Конус>();
                //Если i-й элемент слова не распознается как функтор
                if (!ЭтоИмяФунктора(e.Имя))
                {
                    //и если значение для i-го элемента еще не вычислено, то вычисляем значение рекурсивно
                    Dictionary<string, Конус> результатЭлемента = ВычислитьФунктор(e);
                    foreach (string key in результатЭлемента.Keys)
                        результаты[i][key] = результатЭлемента[key];
                }
                else
                {
                    if (i == 0) continue;
                    switch (e.Имя)
                    {
                        case "id":
                            {
                                foreach (string key in результаты[i].Keys)
                                    результаты[i][key] = результаты[i - 1][key];
                            }; break;
                        case ">":
                            {
                                Dictionary<string, Конус> результатЭлемента = ВычислитьОбласть(результаты[i - 1]);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "<":
                            {
                                Dictionary<string, Конус> результатЭлемента = ВычислитьКообласть(результаты[i - 1]);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "*":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 1) результатЭлемента = ВычислитьПроизведение(результаты[i - 2], результаты[i - 1]);
                                else if (i > 0) результатЭлемента = результаты[i - 1];
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "+":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 1) результатЭлемента = ВычислитьСумму(результаты[i - 2], результаты[i - 1]);
                                else if (i > 0) результатЭлемента = результаты[i - 1];
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "-":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 1) результатЭлемента = ВычислитьРазность(результаты[i - 2], результаты[i - 1]);
                                else if (i > 0) результатЭлемента = результаты[i - 1];
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "^":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 1) результатЭлемента = ВычислитьПересечение(результаты[i - 2], результаты[i - 1]);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "~:":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 1) результатЭлемента = ВычислитьПроекцию(результаты[i - 2].Values, результаты[i - 1].Values).ToDictionary(m => m.Имя);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "~":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 1) результатЭлемента = ВычислитьВыбор(результаты[i - 2], результаты[i - 1]);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case ">:":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 0) результатЭлемента = ВычислитьКопредел(результаты[i - 1]);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        case "<:":
                            {
                                Dictionary<string, Конус> результатЭлемента = new Dictionary<string, Конус>();
                                if (i > 0) результатЭлемента = ВычислитьПредел(результаты[i - 1]);
                                foreach (string key in результатЭлемента.Keys)
                                    результаты[i][key] = результатЭлемента[key];
                            }; break;
                        default:
                            {
                                throw new Exception("Неизвестный функтор:" + e.Имя);
                            };
                    }
                }
            }
            return результаты[f.Элементы.Length - 1];
        }

        //Получает список элементов - кообласти всех кообластей рекурсивно
        private static Dictionary<string, Конус> ВычислитьПредел(Dictionary<string, Конус> list)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            Dictionary<string, Конус> кообласть = new Dictionary<string, Конус>(list);
            int приращение;
            do
            {
                приращение = 0;
                кообласть = Функтор.ВычислитьКообласть(кообласть);
                foreach (string key in кообласть.Keys)
                    if (!результат.ContainsKey(key))
                    {
                        //Если здесь добавить  результат.Add(m), то будет не предел, а фильтр - все элементы будут включены в список результатов
                        приращение++; //количество добавленных в итерации элементов конуса
                    }
                результат = кообласть;
            } while (приращение > 0);
            return результат;
        }

        private static Dictionary<string, Конус> ВычислитьКопредел(Dictionary<string, Конус> list)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            Dictionary<string, Конус> область = new Dictionary<string, Конус>(list);
            int приращение;
            do
            {
                приращение = 0;
                область = Функтор.ВычислитьОбласть(область);
                foreach (string key in область.Keys)
                    if (!результат.ContainsKey(key))
                    {
                        //Если здесь добавить  результат.Add(m), то будет не предел, а фильтр - все элементы будут включены в список результатов
                        приращение++; //количество добавленных в итерации элементов конуса
                    }
                результат = область;
            } while (приращение > 0);
            return результат;
        }

        //Вычисляет Область морфизмов из l такую, что каждый морфизм этой области удовлетворяет хотя-бы одному из шаблонов r
        private static Dictionary<string, Конус> ВычислитьОбласть(Dictionary<string, Конус> l/*, List<Конус> r*/)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            IEnumerable<Конус> области = l.SelectMany(m => m.Value.ПолучитьОбласть().Values);
            foreach (Конус c in области)
                результат[c.Имя] = c;
            return результат;
        }

        //Вычисляет Область морфизмов из l такую, что каждый морфизм этой области удовлетворяет хотя-бы одному из шаблонов r
        private static Dictionary<string, Конус> ВычислитьКообласть(Dictionary<string, Конус> l/*, List<Конус> r*/)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            IEnumerable<Конус> кообласти = l.SelectMany(m => m.Value.ПолучитьКообласть().Values);
            foreach (Конус c in кообласти)
                результат[c.Имя] = c;
            return результат;
        }

        //Вычисляет декартово произведение морфизмов l X r
        private static Dictionary<string, Конус> ВычислитьПроизведение(Dictionary<string, Конус> l, Dictionary<string, Конус> r)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>();
            foreach (Конус left in l.Values)
                foreach (Конус right in r.Values)
                {
                    Конус конус = Storage.AddLocal(new Конус(new Конус[] { left, right }));
                    результат[конус.Имя] = конус;
                }
            return результат;
        }

        //Сумма вычисляется как объекдинение морфизмов (теоретико-множественное)
        private static Dictionary<string, Конус> ВычислитьСумму(Dictionary<string, Конус> l, Dictionary<string, Конус> r)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>(l);
            return результат.Union(r).ToDictionary(m => m.Key, m => m.Value);
        }

        //Разность вычисляется как морфизмы из l, которые не содержаться в r (теоретико-множественная)
        private static Dictionary<string, Конус> ВычислитьРазность(Dictionary<string, Конус> l, Dictionary<string, Конус> r)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>(l);
            return результат.Except(r).ToDictionary(m => m.Key, m => m.Value);
        }

        //Персечение вычисляется как морфизмы, одновременно содержащиеся и в l и в r
        private static Dictionary<string, Конус> ВычислитьПересечение(Dictionary<string, Конус> l, Dictionary<string, Конус> r)
        {
            Dictionary<string, Конус> результат = new Dictionary<string, Конус>(l);
            return результат.Intersect(r).ToDictionary(m => m.Key, m => m.Value);
        }

        //Метод возвращет морфизмы, являющиеся проекциями морфизмов l по шаблонам r. В проекцию попадают только элементы
        //для которых в соответствующем шаблоне на соответствующих местах были символы "_".
        //Например, 
        //для коллекции l = { [0,1,a], [1,0,b], [1,1,c] } применение метода с шаблоном r = { [1,_,_] } вернет { [0,b], [1,c] };
        //для коллекции l = { [a,a], [a,b], [a,c] } применение метода с шаблоном r = { [a,_] } вернет { a, b, c };
        //для коллекции l = { [a,a], [a,b], [a,c] } применение метода с шаблоном r = { _ } вернет { [a,a], [a,b], [a,c] };
        private static List<Конус> ВычислитьПроекцию(IEnumerable<Конус> l, IEnumerable<Конус> r)
        {
            List<Конус> результат = l.
                Select(left => new { морфизм = left, шаблоны = left.СоответствующиеШаблоны(r) }).    //получаем список пар {морфизм, соответствующие шаблоны}
                Where(pair => pair.шаблоны.Count() > 0).            //отбираем только те, которые прошли тест на соответствие
                Select(p => Storage.AddLocal(                       //создаем список проекций морфизмов
                    p.морфизм.Элементы.                             //для проекции морфизма отбираем только элементы,
                    Where((элемент, индекс) => p.шаблоны.           //которые в соответствующих шаблонах имели 
                        Any(m => m.Имя == "_" || (m.Элементы[индекс].Имя == "_")))     //на том же месте символ "_"
                    )
                ).Distinct().
                ToList();
            return результат;
        }

        //Вычисляет морфизмы l, удовлетворяющие шаблонам из r
        private static Dictionary<string, Конус> ВычислитьВыбор(Dictionary<string, Конус> l, Dictionary<string, Конус> r)
        {
            return l.Where(m => m.Value.СоответствуетШаблону(r.Values)).ToDictionary(c => c.Key, c => c.Value);
        }


    }

}
