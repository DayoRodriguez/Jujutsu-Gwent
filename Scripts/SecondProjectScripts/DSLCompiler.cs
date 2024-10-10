using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DSLCompiler : MonoBehaviour
{
    public GameObject CodeArea;
    public GameObject Console;

    public TMP_InputField input;
    public TMP_InputField consoleText;

    public Button compilerButton;
    public Button exitButton;

    private void Compile()
    {
        string code = input.text;
        string console_code = consoleText.text;
        DSL.Compile(code);
    }
}
