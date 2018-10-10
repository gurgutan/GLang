using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public class Parser
    {
        private const int MaxRecursionDepth = 1024; //Максимальная глубина рекурсии при разборе правил грамматики

        public string Input;
        public ASTNode ASTRoot;
        public BNFGrammar Grammar;

        public Parser(BNFGrammar _grammar, string _input)
        {
            Input = _input;
            Grammar = _grammar;
        }

        public Parser(BNFGrammar _grammar)
        {
            Grammar = _grammar;
        }

        public ASTNode BuildAST(string _input)
        {
            Input = _input;
            return BuildAST();
        }

        public ASTNode BuildAST()
        {
            if (Input == "" || Input == null) return null;
            ASTRoot = TryParseRule(0, Grammar.Root, 0);
            return ASTRoot;
        }

        //Пропускает все символы разделители: пробелы и управляющие символы. 
        //Возвращает номер символа входной строки Input, следующий за символами разделителями.
        private int PassSeparators(int i)
        {
            while (i < Input.Length && (Char.IsWhiteSpace(Input[i]) || Char.IsControl(Input[i])))
                i++;
            return i;
        }

        //Пропускает комментарий, начинающийся с символов BNFGrammar.BNFCommentPrefix.
        private int PassComments(int i)
        {
            if (i + BNFGrammar.BNFCommentPrefix.Length < Input.Length &&
                Input.Substring(i, BNFGrammar.BNFCommentPrefix.Length) == BNFGrammar.BNFCommentPrefix)
            {
                i += BNFGrammar.BNFCommentPrefix.Length;
                while (i < Input.Length && Input[i] != '\n')
                    i++;
                i = PassSeparators(i);
            }
            return i;
        }

        //TODO: Добавить возможность использовать пробел как разделитель
        private ASTNode TryParseRule(int i, string _rule, int depth)
        {
            if (depth > MaxRecursionDepth)
                return new ASTNode(ASTType.MaxDepthReached, _rule, "Достигнут максимум рекурсии: " + depth, i, i, _rule);
            //Пропускаем все разделители слов
            i = PassSeparators(i);
            i = PassComments(i);
            //Проверяем, является ли _rule терминальным правилом грамматики, - начинается ли со знака BNFTerminalPrefix
            if (_rule.StartsWith(BNFGrammar.BNFTerminalPrefix))
            {
                //Выделяем значение терминального правила - символы после BNFTerminalPrefix
                string terminal = _rule.Substring(BNFGrammar.BNFTerminalPrefix.Length);
                //Если остаток строки входного текста меньше чем длина значения терминального правила, возвращаем неуспех
                if (i + terminal.Length > Input.Length)
                    return new ASTNode(ASTType.EOFReached, _rule, "Конец файла", i, Input.Length - 1, _rule);
                //Выделяем из входного текста строку для сравнения с правилом
                string substring = Input.Substring(i, terminal.Length);
                if (terminal == substring)
                    return new ASTNode(ASTType.TeminalEqual, _rule, terminal, i, i + terminal.Length - 1, substring);
                else
                    return new ASTNode(ASTType.TerminalNotEqual, _rule, terminal, i, i + terminal.Length - 1, substring);
            }
            //Если не существует правила с именем _rule выходим с неудачей
            if (!Grammar.Rules.ContainsKey(_rule)) return null;
            //Если существует правило с именем _rule
            Interpreter.BNFGrammar.BNFRule rule = Grammar.Rules[_rule];
            //Список опробований всех термов правила в виде списка пар <Терм, СписокУзловСимволовТерма>
            List<KeyValuePair<string, List<ASTNode>>> ruleParseResults =
                new List<KeyValuePair<string, List<ASTNode>>>();

            //Перебор всех термов правила rule
            foreach (string term in rule.Terms.Keys)
            {
                KeyValuePair<string, List<ASTNode>> termParseResults =
                    new KeyValuePair<string, List<ASTNode>>(term, TryParseTerm(i, _rule, rule.Terms[term], depth));
                if (termParseResults.Value.Count != 0)
                    ruleParseResults.Add(termParseResults);
            }
            if (ruleParseResults.Count == 0) return null;

            //Проверка и обработка результатов применения термов правила к тексту
            List<KeyValuePair<string, List<ASTNode>>> successfulResults = ruleParseResults.
                Where(pair => pair.Value.
                    All(n => n.Type == ASTType.NonterminalEqual || n.Type == ASTType.TeminalEqual)).
                    OrderByDescending(l => l.Value.Last().Text.IndexEnd).
                    ToList();
            //Если нет успешных результатов проверки данного правила
            if (successfulResults.Count == 0)
            {
                //Выбираем самый длинный из неуспешных вариантов проверок
                KeyValuePair<string, List<ASTNode>> longestResult = ruleParseResults.First();
                ASTNode Node = new ASTNode();
                Node.Type = ASTType.NonterminalNotEqual;
                Node.RuleName = "Ошибка: " + _rule;
                Node.RuleTerm = longestResult.Key;
                Node.Text.Value = "Ошибка: " + _rule;
                Node.Text.IndexStart = i;
                //Node.Text.IndexEnd = longestResult.Value.Last().Text.IndexEnd;
                //Добавляем узлы, на которых споткнулась проверка термов правила
                longestResult.Value.ForEach(c => Node.AddChild(c));
                return Node;
            }
            else
            {
                KeyValuePair<string, List<ASTNode>> longestResult = successfulResults.First();
                ASTNode Node = new ASTNode();
                Node.Type = ASTType.NonterminalEqual;
                Node.RuleName = _rule;
                Node.RuleTerm = longestResult.Key;
                Node.Text.IndexStart = i;
                Node.Text.IndexEnd = longestResult.Value.Last().Text.IndexEnd;
                Node.Text.Value = Input.Substring(i, longestResult.Value.Last().Text.IndexEnd - i + 1);
                longestResult.Value.ForEach(c => { Node.AddChild(c); });
                return Node;
            }
        }

        private List<ASTNode> TryParseTerm(int pos, string rule, BNFGrammar.BNFTerm term, int depth)
        {
            List<ASTNode> termNodes = new List<ASTNode>();
            for (int i = 0; i < term.Symbols.Length; i++)
            {
                string s = term.Symbols[i];
                ASTNode node = TryParseRule(pos, s, depth + 1);
                //Если не получилось применить очередное правило s и результата нет
                if (node == null)
                {
                    break;
                }
                //Если применение правила неуспешное
                else if (
                    node.Type == ASTType.NonterminalNotEqual ||
                    node.Type == ASTType.TerminalNotEqual ||
                    node.Type == ASTType.Deadlock ||
                    node.Type == ASTType.MaxDepthReached ||
                    node.Type == ASTType.EOFReached)
                {
                    //добавляем узел неуспешной проверки, если в этом цикле уже были успешные
                    if (i > 0)
                        termNodes.Add(node);
                    break;
                }
                //Если применение правило успешное
                else if (node.Type == ASTType.NonterminalEqual || node.Type == ASTType.TeminalEqual)
                {
                    pos = node.Text.IndexEnd + 1;   //меняем текущую позицию в тексте
                    termNodes.Add(node);
                }
                else
                    throw new Exception("Неизвестный тип AST узла: " + node.Type.ToString());
            }
            if (termNodes.Count < term.Symbols.Length)
                termNodes.Add(new ASTNode(ASTType.NonterminalNotEqual, rule, term.Name, pos, pos + 1));
            return termNodes;
        }

    }
}
