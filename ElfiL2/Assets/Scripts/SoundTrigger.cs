using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class SoundTrigger : MonoBehaviour
{
    [HideInInspector] public LvlManager lvlManager;
    [SerializeField] bool isLastTrigger;
    bool playerIn;
    [SerializeField] Animator animator;
    [SerializeField] float soundWaveRadius;
    [SerializeField] string eventToPlayAfterActivation;

    FMOD.Studio.EventInstance eventInstance;

    private void Start()
    {
        var main = GetComponentInChildren<ParticleSystem>(true).main;
        if(!isLastTrigger)
        {
            main.startSize = soundWaveRadius;

        }

        eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventToPlayAfterActivation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            if(isLastTrigger)
            {
                if(lvlManager.LvlCompletion == 1)
                {
                    lvlManager.enterSoundZone(2);
                }
                else
                {
                    lvlManager.enterSoundZone(1);
                }
            }
            else
            {
                lvlManager.enterSoundZone(0);

            }
            playerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            lvlManager.exitSoundZone();
            playerIn = false;
        }
    }

    private void Update()
    {
        if(playerIn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<StudioEventEmitter>().Stop();
                RuntimeManager.AttachInstanceToGameObject(eventInstance, transform);
                eventInstance.start();

                if (isLastTrigger && lvlManager.LvlCompletion == 1)
                {
                    lvlManager.LvlFinished();
                    animator.enabled = true;
                    GetComponent<Collider>().enabled = false;
                    playerIn = false;
                    lvlManager.exitSoundZone();

                }
                if (!isLastTrigger)
                {
                    lvlManager.SoundFound();
                    transform.GetChild(0).gameObject.SetActive(true);
                    GetComponent<Collider>().enabled = false;
                    playerIn = false;
                    lvlManager.exitSoundZone();
                    lvlManager.CheckForAreaCompletion(transform.tag);

                }
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, soundWaveRadius/2);
    }
}