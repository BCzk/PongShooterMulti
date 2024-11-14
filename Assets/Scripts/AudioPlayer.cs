using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class AudioPlayer : MonoBehaviourPun
{
    [SerializeField] private AudioSource sourceSFX;

    [SerializeField] private AudioClip onRoundStartedSfx;
    [SerializeField] private AudioClip onRoundFinishedSfx;
    [SerializeField] private AudioClip onMatchFinishedSfx;
    [SerializeField] private AudioClip onBallBounceSfx;
    [SerializeField] private AudioClip onShootSfx;
    [SerializeField] private AudioClip onShieldHitSfx;
    [SerializeField] private AudioClip onShieldRecoverSfx;

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

        switch (photonEvent.Code)
        {
            default:
                break;
            case EventCodeConsts.ON_ROUND_STARTED_EVENT:
                clipToPlay = onRoundStartedSfx;
                break;
            case EventCodeConsts.ON_ROUND_FINISHED_EVENT:
                clipToPlay = onRoundFinishedSfx;
                break;
            case EventCodeConsts.ON_MATCH_FINISHED_EVENT:
                clipToPlay = onMatchFinishedSfx;
                break;
            case EventCodeConsts.ON_BALL_BOUNCE_EVENT:
                clipToPlay = onBallBounceSfx;
                break;
            case EventCodeConsts.ON_PLAYER_SHOOT_EVENT:
                clipToPlay = onShootSfx;
                break;
            case EventCodeConsts.ON_SHIELD_HIT_EVENT:
                clipToPlay = onShieldHitSfx;
                break;
            case EventCodeConsts.ON_SHIELD_RECOVER_EVENT:
                clipToPlay = onShieldRecoverSfx;
                break;
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
