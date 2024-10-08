using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

public class EditorCardController : MonoBehaviour
{
    public GameObject editorPanel;
    public Button editorButton;
    public Button compilerButton;
    public Button exitButton;
    public TMP_InputField inputField;
    public TMP_InputField console;

    void Start()
    {
        editorButton.onClick.AddListener(() => editorPanel.SetActive(true));
        exitButton.onClick.AddListener(() => editorPanel.SetActive(false));
        compilerButton.onClick.AddListener(CompilerCode);
    }

    void CompilerCode()
    {
        string code = inputField.text;
        var parser = new Parser(code);
        while(true)
        {
            foreach(var tk in parser.Diagnostics)
            {
                //PrettyPrint(tk);
            }
            break;
            //Debug.Log("La token es " + tk.Value + " es una token de tipo " + tk.Type);
        }
    }

    // void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = false)
    // {
    //     var marker = isLast ? "|--":"|----";

    //     Debug.Log(indent);
    //     Debug.Log(marker);
    //     Debug.Log(node.Type);

    //     if(node is SyntaxToken t && t.Value != null)
    //     {
    //         Debug.Log(" ");
    //         Debug.Log(t.Value);
    //     }

    //     indent += isLast ? "  " : "|  ";
        
    //     var lastChild = node.GetChildren();

    //     foreach(var child in node.GetChildren())
    //     {
    //         PrettyPrint(child, indent, child == lastChild);
    //     } 
    // }
}
