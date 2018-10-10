﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using DataModelLibrary;

namespace DataModelLibrary
{
    [TestClass]
    public class WordTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            //Проверка создания слова
            Конус word1 = new Конус("Word1");
            Assert.IsNotNull(word1.Элементы);
            Assert.AreEqual(word1.Имя, "Word1");
            //Проверка инициализации из другого слова
            Конус word2 = new Конус(word1);
            Assert.AreEqual(word2.Имя, "Word1");
            Конус[] words = new Конус[] 
            { 
                new Конус("1"), 
                new Конус("2"), 
                new Конус("3") 
            };
            Конус word3 = new Конус(words);
            //Проверка изменения имени при присвоении состава элементов слова
            Assert.AreEqual(word3.Имя, "[1,2,3]");
            //Проверка изменения значения Элементы и соответствующего изменения имени слова
            Конус word4 = new Конус(
                new Конус[] 
                { 
                    word3,
                    new Конус("4"),
                    new Конус("5"),
                    new Конус("6")
                }
            );
            Assert.AreEqual(word4.Имя, "[[1,2,3],4,5,6]");
        }

        [TestMethod]
        public void TestMatching()
        {
            Конус w1 = new Конус("1");
            Конус w2 = new Конус("2");
            Конус w3 = new Конус("3");

            Assert.IsTrue(w1.СоответствуетШаблону(new Конус("_"))); //проверка соответствия шаблону "ЛЮБОЙ"
            Assert.IsTrue(w1.СоответствуетШаблону(new Конус("1"))); //проверка соответствия шаблону "1"
            Assert.IsFalse(w1.СоответствуетШаблону(new Конус("2"))); //проверка несоответствия шаблону "2"

            Конус w4 = new Конус(new Конус[] { w1, w2, w3 });  //слово [1,2,3]
            Конус w5 = new Конус(new Конус[] { w4, w2, w3 });  //слово [[1,2,3],2,3]
            Конус pattern = new Конус(new Конус[] { new Конус("_"), w2, w3 });  //проверка паттерна [_,2,3] для слова [[1,2,3],2,3]
            Assert.IsTrue(w5.СоответствуетШаблону(pattern));
        }

        [TestMethod]
        public void TestWordDomain()
        {
            Конус w1 = new Конус("1");
            Конус w2 = new Конус("2");
            Конус w3 = new Конус("3");
            w1.ДобавитьВКообласть(w2);
            w1.ДобавитьВКообласть(w3);

            Assert.IsTrue(w1.Кообласть.Count == 2);    //проверка добавления двух слов в домен слова w1
            Assert.IsTrue(w2.Область["1"].Имя == "1");    //проверка кодоменов добавляемых слов
            Assert.IsTrue(w3.Область["1"].Имя == "1");

            w1.УдалитьИзКообласти(w2);
            w1.УдалитьИзКообласти(w3);

            Assert.IsTrue(w1.Кообласть.Count == 0);    //проверка удаления слова из домена w1
            Assert.IsTrue(w2.Область.Count == 0);
        }

        [TestMethod]
        public void TestFunctors()
        {
            //"<",    // получает кообласть морфизма
            //">",    // получает область морфизма
            //"*",    // получает произведение морфизмов
            //"+",    // получет сумму морфизмов
            //"-",    // получает разность морфизмов
            //"^",    // получает пересечение морфизмов
            //"-:",    // получает проекцию морфизмов
            //"id",   // тождество
            Конус знакОбласть = new Конус("<");
            Конус знакКообласть = new Конус(">");
            Конус знакПроизведение = new Конус("*");
            Конус знакСумма = new Конус("+");
            Конус знакРазность = new Конус("-");
            Конус знакПересечение = new Конус("^");
            Конус знакПроекция = new Конус("~:");
            Конус знакВыбор = new Конус("~");
            Конус знакТождество = new Конус("id");
            Конус знакЛюбой = new Конус("_");

            Конус w1 = new Конус("1");
            Конус w2 = new Конус("2");
            Конус w1s = new Конус("один");
            Конус w2s = new Конус("два");

            Конус числоЦифрами = new Конус(new Конус("числоЦифрами"));
            числоЦифрами.ДобавитьВКообласть(new Конус[] { w1, w2 });

            Конус числоСловами = new Конус(new Конус("числоСловами"));
            числоСловами.ДобавитьВКообласть(new Конус[] { w1s, w2s });

            Конус числа = new Конус(new Конус("числа"));
            числа.ДобавитьВКообласть(new Конус[] { числоЦифрами, числоСловами });

            //Конус фОбласть = new Конус(new Конус[] { числоЦифрами, знакОбласть });
            //List<Конус> область = DataModelLibrary.Функтор.ВычислитьФунктор(фОбласть);
            //Assert.IsTrue(область.Count==2 && область[0].Имя == "1" && область[1].Имя == "2");

            //Конус фКообласть = new Конус(new Конус[] { числоЦифрами, знакКообласть });
            //List<Конус> кообласть = DataModelLibrary.Функтор.ВычислитьФунктор(фКообласть);
            //Assert.IsTrue(кообласть.Count == 1 && кообласть[0].Имя == "числа");

            //Конус фПроизведение = new Конус(new Конус[] { числоЦифрами, знакПроизведение, числоСловами });
            //List<Конус> произведение = DataModelLibrary.Функтор.ВычислитьФунктор(фПроизведение);
            //Assert.IsTrue(произведение.Count == 1 && произведение[0].Имя == "[числоЦифрами,числоСловами]");

            //Конус фСумма = new Конус(new Конус[] { числоЦифрами, знакСумма, числоСловами });
            //List<Конус> сумма = DataModelLibrary.Функтор.ВычислитьФунктор(фСумма);
            //Assert.IsTrue(сумма.Count == 2 &&
            //    сумма.Any(m=>m.Имя == "числоЦифрами"));

            //Конус фРазность = new Конус(new Конус[] { фСумма, знакРазность, числоСловами });
            //List<Конус> разность = DataModelLibrary.Функтор.ВычислитьФунктор(фРазность);
            //Assert.IsTrue(разность.Count == 1 &&
            //    разность.Any(m => m.Имя == "числоЦифрами"));

            //Конус фПересечение = new Конус(new Конус[] { фСумма, знакПересечение, числоСловами });
            //List<Конус> пересечение = DataModelLibrary.Функтор.ВычислитьФунктор(фПересечение);
            //Assert.IsTrue(пересечение.Count == 1 && пересечение[0].Имя == "числоСловами");

            //Конус шаблон = new Конус(new Конус[] { числоЦифрами, знакЛюбой });
            //Конус фПроекция = new Конус(new Конус[] { фПроизведение, знакПроекция, шаблон });
            //List<Конус> проекция = DataModelLibrary.Функтор.ВычислитьФунктор(фПроекция);
            //Assert.IsTrue(проекция.Count == 1 && проекция[0].Имя == "числоСловами");

            //Конус фКообластьПроизведения = Функтор.СоздатьФунктор(Функтор.Тип.ВычислениеКообласти, произведение.ToArray());
            //List<Конус> кообластьПроизведения = DataModelLibrary.Функтор.ВычислитьФунктор(фКообластьПроизведения);
            //Assert.IsTrue(кообластьПроизведения.Count == 4 &&
            //    кообластьПроизведения.Any(m => m.Имя == "[1,один]") &&
            //    кообластьПроизведения.Any(m => m.Имя == "[1,два]") &&
            //    кообластьПроизведения.Any(m => m.Имя == "[2,один]") &&
            //    кообластьПроизведения.Any(m => m.Имя == "[2,два]"));


        }
    }
}
