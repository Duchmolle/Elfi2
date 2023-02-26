using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerFootsteps : MonoBehaviour
{
    float timer = 0.0f;

    [SerializeField]
    float footstepSpeed = 0.3f;
    float walkFootstepSpeed;
    Charcon playerController;
    Rigidbody playerRb;
    private enum CURRENT_TERRAIN { ROCK, WOOD, SAND };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private FMOD.Studio.EventInstance foosteps;

    private void Awake()
    {
        playerController = GetComponent<Charcon>();
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DetermineTerrain();

        if(playerController.onStep)
        {
            if (timer > footstepSpeed)
            {
                SelectAndPlayFootstep();
                timer = 0.0f;
            }

        }
        timer += Time.deltaTime;
    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD;
                break;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Sand"))
            {
                currentTerrain = CURRENT_TERRAIN.SAND;
                break;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Rock"))
            {
                currentTerrain = CURRENT_TERRAIN.ROCK;
                break;
            }
        }
    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.WOOD:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.SAND:
                PlayFootstep(1);
                break;

            case CURRENT_TERRAIN.ROCK:
                PlayFootstep(2);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    private void PlayFootstep(int terrain)
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
        foosteps.setParameterByName("Terrain", terrain);
        foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }
}

