using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulliganManager : MonoBehaviour
{
    public GameObject mulliganPanel;
    public Text mulliganText;
    public Button mulliganButton;

    public void ShowMulliganUI(bool show)
    {
        mulliganPanel.SetActive(show);
        if(show)
        {
            mulliganText.text = "Seleccione las cartas que desea intercambiar";
            mulliganButton.onClick.RemoveAllListeners();
            mulliganButton.onClick.AddListener(() => OnMulliganOkClicked());
        }
    }

    private void OnMulliganOkClicked()
    {
        mulliganPanel.SetActive(false);
        StartCoroutine(MulliganTimer());
    }

    private IEnumerator MulliganTimer()
    {
        float timer = 10f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
