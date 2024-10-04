using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;

public class ASTNode
{
    public string Name{get; set;}
    public Dictionary<string, object> Paramters = new Dictionary<string, object>();
    public List<ASTNode> children = new List<ASTNode>();
}
public abstract class AST{
    public abstract object Eval();
}

public class PropertyAST : AST
{
    public string PropetyName {get;set;}
    private AST Value{get;set;}
    public PropertyAST(string PropetyName,AST Value){
        this.PropetyName = PropetyName;
        this.Value = Value;
    }
    public override object Eval()
    {
        return Value;
    }
}
public class AtomicLiteralAST:AST{
    string Value{get;set;}
    public AtomicLiteralAST(string Value){
        this.Value = Value;
    }
    public override object Eval()
    {
        return Value;
    }
}