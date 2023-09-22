using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SoundPlayer : MonoBehaviour {
    [SerializeField] private AudioClip _background;
    [SerializeField] private SoundData[] _sounds;

    public static SoundPlayer Instance;

    private void Start() {
        Instance = this;

        _aS = new GameObject().AddComponent<AudioSource>();

        gameObject.AddComponent<AudioSource>().clip = _background;
        gameObject.GetComponent<AudioSource>().loop = true;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void OnExplodeSound() {
        PlaySound("Explosion");
    }

    public void StopBackgroundMusic() {
        gameObject.GetComponent<AudioSource>().Stop();
    }

    private AudioSource _aS;
    public void PlaySound(string name) {
        _aS.clip = _sounds.First(x => x.SoundName == name).SoundClip;
        _aS.volume = _sounds.First(x => x.SoundName == name).Volume;
        _aS.Play();
    }
}

[System.Serializable]
class SoundData {
    public string SoundName;
    [Range(0, 1)] public float Volume = 1;
    public AudioClip SoundClip;
}