using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] clips;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Step(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioClip clip = GetRandomClip();
            source.PlayOneShot(clip);
        }
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
