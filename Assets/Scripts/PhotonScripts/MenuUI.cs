using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MenuUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioSource menuAudioSourcePlayer;
    [SerializeField] private AudioClip onExceptionSfx;

    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button errorButton;
    [SerializeField] private TMPro. TMP_InputField createInput;
    [SerializeField] private TMPro. TMP_InputField joinInput;
    [SerializeField] private TMPro. TextMeshProUGUI errorText;

    private void Awake()
    {

        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void OnDestroy()
    {
        createButton.onClick.RemoveAllListeners();
        joinButton.onClick.RemoveAllListeners();
    }

    private void CreateRoom()
    {
        RoomOptions roomConfiguration = new RoomOptions();
        roomConfiguration.MaxPlayers = 2;

        if (createInput.text == "")
        {
            errorButton.gameObject.SetActive(true);
            errorText.text = "Cannot leave room name Empty";
            PlayOneShot(onExceptionSfx);
        }
        else
        {
            PhotonNetwork.CreateRoom(createInput.text, roomConfiguration);
        }
    }

    private void JoinRoom()
    {
        if (joinInput.text == "")
        {
            errorButton.gameObject.SetActive(true);
            errorText.text = "Must Enter a Room to Join";
            PlayOneShot(onExceptionSfx);
        }
        else
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameplayScene");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        errorButton.gameObject.SetActive(true);
        errorText.text = message;
        PlayOneShot(onExceptionSfx);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        errorButton.gameObject.SetActive(true);
        errorText.text = message;
        PlayOneShot(onExceptionSfx);
    }

    public void PlayOneShot(AudioClip clip)
    {
        float volume = menuAudioSourcePlayer.volume;

        if (menuAudioSourcePlayer.isPlaying)
        {
            volume /= 2.0f;
        }

        menuAudioSourcePlayer.PlayOneShot(clip, volume);
    }
}
