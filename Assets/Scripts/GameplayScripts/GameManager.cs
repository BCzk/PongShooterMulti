using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    private bool _matchStarted;

    private int _redPoints;
    private int _bluePoints;

    //Actualizaciones de UI
    [SerializeField] private Animator timerAnimator;
    [SerializeField] private Animator[] pointAnimators;
    
    public Action ResetThings;
    public Action RoundStarted;
    public Action RoundFinished;
    public Action<string> MatchFinished;

    private void Start()
    {
        //Tiene que estar al tanto de los player
        var players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().PlayerDied += OnPlayerDeath;
        }
        timerAnimator.SetTrigger("StartTimer");
    }

    private void Update()
    {
        //Tiene que revisar que se cumplan condiciones de juego
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            _matchStarted = true;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && _matchStarted)
        {
            _matchStarted = false;
            //Acá iría la excepción y el fin de partida
        }

        if (_matchStarted) //Consultar, por ahora dejo las cosas acá adentro pero ya sé que no va acá
        {
            //Resetea todos los elementos
            //Reinicio pool de balas y pelotas + reinicio posición y escudos de players (Cada quien reacciona al round started por su cuenta)
            ResetThings.Invoke();
            
            //Tiene que avisar que empezó una ronda
            RoundStarted.Invoke();
            
            //Tiene que hacer el contador de cada ronda
            //Tiene que triggerear una animación de una UI
            timerAnimator.SetTrigger("StartTimer");
            
            
            //Tiene que avisar que terminó una ronda
            RoundFinished.Invoke();
            
            
            //Tiene que actualizar la UI
            //Lo hace cuando recibe la muerte de un player
        }
        
        //Match finished
        //Avisa quién ganó y abre canvas
    }

    private void OnPlayerDeath(string deadPlayer)
    {
        //Tiene que llevar la cuenta de las rondas
        
        if (deadPlayer == "Red")
        {
            _bluePoints++;
            //Actualiza UI
            UpdatePointsUI();
            if (_bluePoints >= 3)
            {
                MatchFinished("Blue");
            }
        }
        else if (deadPlayer == "Blue")
        {
            _redPoints++;
            //Actualiza UI
            UpdatePointsUI();
            if (_redPoints >= 3)
            {
                MatchFinished("Red");
            }
        }
        else
        {
            Debug.Log("Error al sumar los puntos");
        }
    }

    private void UpdatePointsUI()
    {
        for (int i = 0; i < _redPoints; i++)
        {
            pointAnimators[i].SetBool("Red", true);
        }

        for (int i = pointAnimators.Length - 1; i > (pointAnimators.Length - 1) - _bluePoints; i--)
        {
            pointAnimators[i].SetBool("Blue", true);
        }
    }
}
