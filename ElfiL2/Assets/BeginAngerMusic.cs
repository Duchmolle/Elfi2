using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginAngerMusic : MonoBehaviour
{
    [SerializeField] StudioEventEmitter backgroundMusicInstance;
    [SerializeField] LvlManager levelManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            backgroundMusicInstance.SetParameter("BeginAngerMusic", 1);
            backgroundMusicInstance.SetParameter("Intensity", 0);
            levelManager.intensityValue = 0;
        }
    }
}
