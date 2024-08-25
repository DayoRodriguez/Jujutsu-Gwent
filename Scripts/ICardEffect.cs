using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardEffect //: MonoBehaviour
{
    //metodo para activar el efecto
    void Execute(GameObject activingCard);

    //metodo para inicializar el efecto con la carta que lo activa
    void Initialize(Card card);

    //Metodo para mostrar el panel del efecto
    void ShowMessagePanel(string sms);

    //metodo para evaluar si el efecto se puede activar
    bool CanActive();

    //metodo para finalizar el efecto y continuar con el juego
    void EndEffect(GameObject activingCard);
}
