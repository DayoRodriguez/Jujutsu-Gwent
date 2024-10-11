using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public static class DSL
{   
    static DSLCompiler console = GameObject.Find("ButtonCompile").GetComponent<DSLCompiler>();

    static bool hasError = false;
    public static void AddToConsole(string message)
    {   
        console.consoleText.text += message + "\n";
    }
    public static void BreakCompilation()
    {
        Debug.LogError("Invalid code\n");
        AddToConsole("Invalid code");
    }
    public static void Compile(string code)
    {   
        console = GameObject.Find("ButtonCompile").GetComponent<DSLCompiler>();
     
        console.consoleText.text = "";

        hasError = false;

        if (code == "")
        {
            Debug.LogError("Empty code");
            AddToConsole("Empty code");
            BreakCompilation();
            return;
        }

        Lexer lexer = new Lexer(code);

        var tokens = lexer.GetTokens();

        if(hasError)
        {
            BreakCompilation();
            return;
        }

        Parser parser = new Parser(tokens);
        
        var nodes = parser.ParseProgram();
        if(hasError){
            BreakCompilation();
            return;
        }

        SemanticCheck check = new SemanticCheck(nodes);
        check.CheckProgram(check.AST);
        if(hasError){
            BreakCompilation();
            return;
        }

        Debug.Log("Successfull Compilation");

        foreach(var effect in nodes.nodes.Where(n => n is EffectDefinition).Select(n => (EffectDefinition)n)) {
            
            GlobalEffects.effects[effect.name] = effect;
            
        }

        foreach(var card in nodes.nodes.Where(n => n is CardNode).Select(n => (CardNode)n)){
            Card newCard = null;
            switch(card.type){
                case Card.Types.Silver:
                case Card.Types.Golden:
                    newCard = new UnitCard(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultUnitCardImage"), Resources.Load<Sprite>("CardBack"), (int)card.power, Tools.GetCardPositions(card.position), card.activation, 100, false);
                    // newcard = new UnitCard(50 + DataManager.myStringList.Count, null, card.name, Resources.Load<Sprite>("DefaultImage"), card.type, card.activation.activations[0].effect.definition, card.faction, Tools.GetCardPositions(card.position), card.activation, (int)card.power, Tools.GetCardRow(Tools.GetCardPositions(card.position)), 100);
                    break;
                case Card.Types.Increase:
                    newCard = new EspecialCard(50 + DataManager.myStringList.Count, card.name, null, card.type, card.faction, Resources.Load<Sprite>("DefaultImage"), Resources.Load<Sprite>("CardBack"), Tools.GetCardPositions(card.position), card.activation, 100);
                    // newCard = new EspecialCard(50 + DataManager.myStringList.Count, null, card.name, Resources.Load<Sprite>("DefaultImage"), card.type, card.activation.activations[0].effect.definition, card.faction, Tools.GetCardPositions(card.position), card.activation, 0, Tools.GetCardRow(Tools.GetCardPositions(card.position)), 100);
                    break;
                case Card.Types.Weather:
                    newCard = new EspecialCard(50 + DataManager.myStringList.Count, card.name, null, card.type, card.faction, Resources.Load<Sprite>("DefaultImage"), Resources.Load<Sprite>("CardBack"), Tools.GetCardPositions(card.position), card.activation, 100);
                    break;
                case Card.Types.Dump:
                    newCard = new EspecialCard(50 + DataManager.myStringList.Count, card.name, null, card.type, card.faction, Resources.Load<Sprite>("DefaultImage"), Resources.Load<Sprite>("CardBack"), Tools.GetCardPositions(card.position), card.activation, 100);
                    break;
                case Card.Types.Leader:
                    newCard = new UnitCard(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultUnitCardImage"), Resources.Load<Sprite>("CardBack"), (int)card.power, Tools.GetCardPositions(card.position), card.activation, 100, true);
                    break;    
            }
            if(card.activation.activations[0].effect.definition != "DrawCard"){
                if(card.activation.activations[0].effect.parameters.parameters.Count > 0)
                {
                    int x = (int)card.activation.activations[0].effect.parameters.parameters["Amount"];
                    DataManager.myAmountList[card.activation.activations[0].effect.definition] = x;
                }
            }
            DataManager.myStringList.Add(newCard);
        }
    }

    
    public static void Report(int line, int column, string where, string message)
    {
        Debug.LogError($"[Line {line}, Column {column}] {where} Error: " + message);
        AddToConsole($"[Line {line}, Column {column}] {where} Error: " + message);
        hasError = true;
    }
    public static void Error(Token token, string message)
    {
        if (token.type == TokenType.EOF) Report(token.line, token.column, "at end", message);
        else Report(token.line, token.column, $"at'{token.lexeme}'", message);
    }
}