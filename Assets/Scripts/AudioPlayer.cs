using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class AudioPlayer : MonoBehaviourPun
{
    [SerializeField] private AudioSource sourceSFX;

    [SerializeField] private AudioClip onShootSfx;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        AudioClip clipToPlay = null;

        if (photonEvent.Code == EventCodeConsts.ON_PLAYER_SHOOT_EVENT)
        {
            clipToPlay = onShootSfx;
        }

        if (clipToPlay != null)
        {
            PlayOneShot(clipToPlay);
        }
    }

    public void PlayOneShot(AudioClip clip, float volume = 0.75f)
    {
        sourceSFX.volume = volume;

        if (sourceSFX.isPlaying)
        {
            sourceSFX.volume = volume / 2.0f;
        }

        sourceSFX.PlayOneShot(clip);
    }
}
