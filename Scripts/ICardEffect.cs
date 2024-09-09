using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardEffect //: MonoBehaviour
{
    //metodo para activar el efecto
    void Execute(GameObject activingCard);

    //metodo para finalizar el efecto y continuar con el juego
    void EndEffect(GameObject activingCard);
}
