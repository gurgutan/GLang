using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    public enum ASTType 
    { 
        TeminalEqual = 1,  
        NonterminalEqual = 2, 
        Deadlock=33, 
        MaxDepthReached=34,
        EOFReached=35,
        TerminalNotEqual = 36,
        NonterminalNotEqual = 37,
    };

    public struct TextInfo
    {
        public int IndexStart;
        public int IndexEnd;
        public string Value;
    }

    public class ASTNode
    {
        public TextInfo Text = new TextInfo();
        public ASTType Type;
        public string RuleName;
        public string RuleTerm;
        public ASTNode Parent;
        public Dictionary<string, ASTNode> Childs;

        public ASTNode()
        {
            Childs = new  Dictionary<string, ASTNode>();
        }

        public ASTNode(ASTType _type, string _rule, string _term, int _start, int _end)
        {
            Parent = null;
            Type = _type;
            RuleName = _rule;
            RuleTerm = _term;
            Text.IndexStart = _start;
            Text.IndexEnd = _end;
            Childs = new  Dictionary<string, ASTNode>();
        }

        public ASTNode(ASTType _type, string _rule, string _term, int _start, int _end,  Dictionary<string, ASTNode> _childs)
        {
            Type = _type;
            RuleName = _rule;
            RuleTerm = _term;
            Text.IndexStart = _start;
            Text.IndexEnd = _end;
            Childs = _childs;
        }

        public ASTNode(ASTType _type, string _rule, string _term, int _start, int _end, string _value, Dictionary<string, ASTNode> _childs)
        {
            Type = _type;
            RuleName = _rule;
            RuleTerm = _term;
            Text.IndexStart = _start;
            Text.IndexEnd = _end;
            Text.Value = _value;
            Childs = _childs;
        }

        public ASTNode(ASTType _type, string _rule, string _term, int _start, int _end, string _value)
        {
            Type = _type;
            RuleName = _rule;
            RuleTerm = _term;
            Text.IndexStart = _start;
            Text.IndexEnd = _end;
            Text.Value = _value;
            Childs = new Dictionary<string, ASTNode>();
        }

        public bool AddChild(ASTNode child)
        {
            if (Childs.ContainsKey(child.RuleName)) return false;
            child.Parent = this;
            Childs.Add(child.RuleName, child);
            return true;
        }

        //Функция возвращает истину, если в грамматике grammar есть правило, описывающее текущий узел AST
        public bool IsMatch(BNFGrammar grammar)
        {
            if (!grammar.Rules.ContainsKey(this.RuleName)) return false;
            //Получаем из грамматики правило, указанное в узле
            BNFGrammar.BNFRule rule = grammar.Rules[RuleName];
            return IsMatch(rule);
        }

        //Функция возвращает истину, если rule является правилом грамматики, описывающим текущий узел AST
        public bool IsMatch(BNFGrammar.BNFRule rule)
        {
            if (rule.Name != this.RuleName) return false;
            //Получаем из правила имя варианта
            string v = Childs.Aggregate<KeyValuePair<string, ASTNode>, string>("", (curr, next) => (curr == "") ? next.Value.RuleName : curr + " " + next.Value.RuleName);
            //Возвращаем истину, если такой вариант есть в правиле грамматики rule
            return rule.Terms.ContainsKey(v);
        }

        public override string ToString()
        {
            if (RuleName[0] == '\'') return Text.Value;
            string prefix = Type + ":" + RuleName +
                "[" + Text.IndexStart+ ".." + Text.IndexEnd+ "]";
            string childsString = Childs.
                Select(node => node.ToString()).
                Aggregate("", (curr, next) => curr + next);
            string suffix = (childsString == "") ? "" : childsString;
            return "{" + prefix + suffix + "}";
        }
    }
}
