using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksController : MonoBehaviour
{
    [SerializeField] AudioSource _asBGM;
    [SerializeField] Phase[] _phases;

    List<GameObject> _liParticlaList = new List<GameObject>();
    
    private void Start()
    {
        if (_asBGM != null)
        {
            _asBGM.volume = 0.5f;
            _asBGM.Play();
        }

        StartCoroutine(PlayBGM(_asBGM.clip.length));
        StartCoroutine(PlayPhases());
    }

    IEnumerator PlayBGM(float time)
    {
        yield return new WaitForSecondsRealtime(time + 10);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator PlayPhases()
    {
        float totalTime = 0;

        foreach (Phase phase in _phases)
        {
            yield return new WaitForSecondsRealtime(phase._fStartTime - totalTime);
            totalTime = phase._fStartTime;

            PlayCurrentParticleSystems(phase);
        }
    }

    void PlayCurrentParticleSystems(Phase currentPhase)
    {
        _liParticlaList.ForEach(p => p.GetComponent<ParticleSystem>().Stop());

        foreach (ParticleSystem particleSystem in currentPhase._pFireworks)
        {
            Vector3 originalPosition = particleSystem.transform.position;
            GameObject particleSystemObj = Instantiate(particleSystem.gameObject, originalPosition, Quaternion.identity);
            SoundManager.instance.PlaySFX("sfx_01");
            _liParticlaList.Add(particleSystemObj);
            particleSystemObj.GetComponent<ParticleSystem>().Play();
        }
    }
}

[Serializable]
public class Phase
{
    public float _fStartTime;
    public ParticleSystem[] _pFireworks;
}