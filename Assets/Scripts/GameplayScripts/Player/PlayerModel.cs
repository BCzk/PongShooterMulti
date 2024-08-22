using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private float speed;
    
    public void Move(float dirY)
    {
        float movement = dirY * speed * Time.deltaTime;
        transform.Translate(0, movement,0);
    }

    public void Hit()
    {
        //Acá capaz que tire una animación y que le avise al que sea que controle la puntuación que el otro suma punto
    }
}
