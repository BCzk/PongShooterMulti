using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;
    
    void Start()
    {
        _playerModel = GetComponent<PlayerModel>();
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            _playerModel.Move(Input.GetAxis("Vertical"));
            ClampPlayerMoveToScreen();
        }
    }
    
    private void ClampPlayerMoveToScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.y = Mathf.Clamp(pos.y, 0.2f, 0.8f); //Valores arbitrarios
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
