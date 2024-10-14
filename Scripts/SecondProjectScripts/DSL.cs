using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEditor;

public static class DSL
{   
    //static DSLCompiler console = GameObject.Find("ButtonCompile").GetComponent<DSLCompiler>();
    static EditorCardController controller = GameObject.Find("EditorCardController").GetComponent<EditorCardController>();
    static bool hasError = false;

    static string path = @"Assets/SOCard/Resources/Cards";

    public static void AddToConsole(string message)
    {   
        controller.console.text += message + "\n";
    }
    public static void BreakCompilation()
    {
        Debug.LogError("Invalid code\n");
        AddToConsole("Invalid code");
    }
    public static void Compile(string code)
    {   
        //console = GameObject.Find("ButtonCompile").GetComponent<DSLCompiler>();
     
        controller.console.text = "";

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
                    newCard = ScriptableObject.CreateInstance<UnitCard>();
                    newCard.Initialize(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultUnitCardImage"), Resources.Load<Sprite>("CardBack"), (int)card.power, Tools.GetCardPositions(card.position), card.activation, 100, false);
                    AssetDatabase.CreateAsset(newCard, path+$"/{card.name}.asset");
                    break;
                case Card.Types.Increase:
                    newCard = ScriptableObject.CreateInstance<EspecialCard>();
                    newCard.Initialize(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultIncraseImage"), Resources.Load<Sprite>("CardBack"),0, Tools.GetCardPositions(card.position), card.activation, 100, false);
                    AssetDatabase.CreateAsset(newCard, path+$"/{card.name}.asset");
                    break;
                case Card.Types.Climate:
                    newCard = ScriptableObject.CreateInstance<EspecialCard>();
                    newCard.Initialize(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultWeatherImage"), Resources.Load<Sprite>("CardBack"), 0, Tools.GetCardPositions(card.position), card.activation, 100, false);
                    AssetDatabase.CreateAsset(newCard, path+$"/{card.name}.asset");
                    break;
                case Card.Types.Dump:
                    newCard = ScriptableObject.CreateInstance<EspecialCard>();
                    newCard.Initialize(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultDumpImage"), Resources.Load<Sprite>("CardBack"), 0, Tools.GetCardPositions(card.position), card.activation, 100, false);
                    AssetDatabase.CreateAsset(newCard, path+$"/{card.name}.asset");
                    break;
                case Card.Types.Leader:
                    newCard = ScriptableObject.CreateInstance<UnitCard>();
                    newCard.Initialize(50 + DataManager.myStringList.Count, card.name, null, card.faction, card.type, Resources.Load<Sprite>("DefaultLeaderCardImage"), Resources.Load<Sprite>("CardBack"), (int)card.power, Tools.GetCardPositions(card.position), card.activation, 100, true);
                    AssetDatabase.CreateAsset(newCard, path+$"/{card.name}.asset");
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