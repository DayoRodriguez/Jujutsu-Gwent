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
        
    }

}
