using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModelLibrary;
using Interpreter;


namespace ParserWinForm
{
    public partial class MainForm : Form
    {
        Parser parser;
        ExecutionForm execForm = new ExecutionForm();

        // В языке реализованы операции:
        //"<",    // получает кообласть морфизма
        //">",    // получает область морфизма
        //"*",    // получает произведение морфизмов
        //"+",    // получет сумму морфизмов
        //"-",    // получает разность морфизмов
        //"^",    // получает пересечение морфизмов
        //"-:",   // получает проекцию морфизмов
        //"id",   // тождество

        public MainForm()
        {
            InitializeComponent();
            GrammarTextBox.Text =
                //@"Домен = Слово '{ Значения '} | Слово '= '{ Значения '} | Слово ;
                //Значения = Домен ', Значения | Операция ', Значения | Домен | Операция | '{ '} ;
                //Операция = ЛевыйОперанд '-< ЦентральныйОперанд '>- ПравыйОперанд |
                //  ЛевыйОперанд '|> БинарнаяОперация ПравыйОперанд |
                //  ЛевыйОперанд УнарнаяОперация ;
                //ЛевыйОперанд = Значения ;
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
                //Цифра = '0|'1|'2|'3|'4|'5|'6|'7|'8|'9 ;";

@"
Категория = '{ Стрелки '} ;
Стрелка = Слово | Область Слово Кообласть | Слово Кообласть | Область Слово ;
Стрелки = Стрелка ', Стрелки | Стрелка ;
Область = Ноль | '{ Стрелки '} ;
Кообласть = Ноль | '{ Стрелки '} ;
Слова = Слово ', Слова | Слово ;
Слово = '[ Слова '] | Символы ;
Символы = Символ Символы | Символ ;
Символ = Буква | Цифра | Знак ;
Буква = 'a|'b|'c|'d|'e|'f|'g|'h|'i|'j|'k|'l|'m|'n|'o|'p|'q|'r|'s|'t|'u|'v|'w|'x|'y|'z|'A|'B|'C|'D|'E|'F|'G|'H|'I|'J|'K|'L|'M|'N|'O|'P|'Q|'R|'S|'T|'U|'V|'W|'X|'Y|'Z|
'а|'б|'в|'г|'д|'е|'ё|'ж|'з|'и|'й|'к|'л|'м|'н|'о|'п|'р|'с|'т|'у|'ф|'х|'ц|'ч|'ш|'щ|'ь|'ы|'ъ|'э|'ю|'я|'А|'Б|'В|'Г|'Д|'Е|'Ё|'Ж|'З|'И|'Й|'К|'Л|'М|'Н|'О|'П|'Р|'С|'Т|'У|'Ф|'Х|'Ц|'Ч|'Ш|'Ь|'Щ|'Ъ|'Ы|'Э|'Ю|'Я ;
Цифра = '0|'1|'2|'3|'4|'5|'6|'7|'8|'9 ;
Знак = '< | '> | '* | '+ | '- | '~: | '~ | 'id | '_ | '>: | '<: ;
Ноль = '{ '} ;
";
            InputTextBox.Text =
@"{
	
	A{ 1,2,3 },
	B{ 4,5,6 },
	Algo1
	  { 
	  [
		Вход,<,      //Сначала определеим Вход, к которому потом подключим элемент
		A,*,B,*      //Сам алгоритм
	  ]
	  },
	D { a,b,c,d },  //определим значения и отправим их на Вход:
	Вход {D},

	//--------------------------------------------------
	Показать
	{
	//Декартово произведение
	[Algo1,<:]
	}

}";
            RefreshGrammarTree(GrammarTextBox.Text);
            RefreshAST(InputTextBox.Text);
        }

        private void GrammarTextBox_Leave(object sender, EventArgs e)
        {
            RefreshGrammarTree(GrammarTextBox.Text);
            RefreshAST(InputTextBox.Text);
        }

        private void RefreshGrammarTree(string p)
        {
            parser = new Parser(new BNFGrammar(p));
            GrammarTreeView.Nodes.Clear();
            foreach (Interpreter.BNFGrammar.BNFRule rule in parser.Grammar.Rules.Values)
            {
                GrammarTreeView.Nodes.Add(new TreeNode(rule.Name, rule.Terms.Values.
                    Select<BNFGrammar.BNFTerm, TreeNode>(
                    v => new TreeNode(v.ToString(), v.Symbols.
                        Select<string, TreeNode>(s => new TreeNode(s)).ToArray())).
                    ToArray()));
            }
            //GrammarTreeView.ExpandAll();
        }

        private void RefreshAST(string p)
        {
            if (parser != null)
            {
                parser.Input = p;
                ASTTreeView.Nodes.Clear();
                ASTNode root = parser.BuildAST();
                if (root != null)
                {
                    ASTTreeView.Nodes.Add(AddASTNodeToTreeView(root));
                    //ASTTreeView.ExpandAll();
                    //ASTTreeView.Refresh();
                }
            }
            else
                if (InputTextBox.Text.Trim() != "")
                {
                    RefreshGrammarTree(GrammarTextBox.Text);
                }
        }

        private TreeNode AddASTNodeToTreeView(ASTNode astNode)
        {
            if (astNode == null) return null;
            string value = astNode.Text.Value; //astNode.Type +":"+astNode.RuleName+ "(" + astNode.Start + ".." + astNode.End + ")";
            if (astNode.Childs.Count() == 0 || astNode.Type == ASTType.TeminalEqual)
            {
                TreeNode node = new TreeNode(value);
                node.NodeFont = new Font(this.Font, FontStyle.Bold);
                return node;
            }
            else
            {
                TreeNode node = new TreeNode(astNode.RuleName + "[" + astNode.RuleTerm + "]  " + astNode.Text.Value,
                    astNode.Childs.Values.Select<ASTNode, TreeNode>(n => AddASTNodeToTreeView(n)).ToArray());
                Color c = Color.Black;
                if (astNode.Type == ASTType.NonterminalEqual)
                    node.ForeColor = Color.Green;
                else if (astNode.Type == ASTType.TerminalNotEqual ||
                        astNode.Type == ASTType.NonterminalNotEqual ||
                        astNode.Type == ASTType.Deadlock ||
                        astNode.Type == ASTType.MaxDepthReached ||
                        astNode.Type == ASTType.EOFReached)
                        node.ForeColor = Color.Red;
                return node;
            }
        }

        private void ButtonExecute_Click(object sender, EventArgs e)
        {

            //RefreshGrammarTree(GrammarTextBox.Text);
            RefreshAST(InputTextBox.Text);
            if (parser.ASTRoot == null) return;
            if (parser.ASTRoot.Type == ASTType.NonterminalEqual)
            {
                OutputTextBox.Text = "";
                DateTime startCompileTime = DateTime.Now;
                CategoryProcessor p = new CategoryProcessor(parser.Grammar);
                DateTime endCompileTime = DateTime.Now;
                DateTime startTime = DateTime.Now;
                p.Execute(parser.ASTRoot, parser.Grammar);
                DateTime endTime = DateTime.Now;
                OutputTextBox.Text += "Время интерпретации: " + (endCompileTime - startCompileTime) + "\n";
                OutputTextBox.Text += "Время вычисления: " + (endTime - startTime) + "\n";
                if (!Storage.Local.ContainsKey("Показать"))
                {
                    return;
                }

                foreach (Конус w in Storage.Local["Показать"].Кообласть.Values)
                {
                    string content = w.Кообласть.Values.Aggregate<Конус, string>("", (c, n) => c == "" ? n.ToString() : c + "," + n.ToString());
                    OutputTextBox.Text += w.Имя + (content == "" ? "" : "{" + content + "}") + "\n";
                }
                OutputTextBox.Text += "--------------------------------------------\n";
                //foreach (Конус w in Storage.Local.Values)
                //{
                //    string content = w.Кообласть.Values.Aggregate<Конус, string>("", (c, n) => c == "" ? n.ToString() : c + "," + n.ToString());
                //    OutputTextBox.Text += w.Имя + (content=="" ? "" : "{" + content+ "}") +"\n"; 
                //}
                //execForm = new ExecutionForm();
                //execForm.Memory = p.Memory;
                //execForm.Show();
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
