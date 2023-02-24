using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEditor;
using FMODUnity;
using FMOD.Studio;

public class LvlManager : MonoBehaviour
{
    public ScriptableRendererFeature SRF;
    [SerializeField] ParticleSystem BellPS;
    GameObject[] soundZones;
    public int FoundSound;
    public float LvlCompletion;
    [SerializeField] TextMeshProUGUI score, text, areaCompleted;

    [SerializeField] private int nostalgiaAreaMax;
    [SerializeField] private int serenityAreaMax;

    [SerializeField] StudioEventEmitter backgroundMusicInstance;
    [SerializeField] GameObject linkBetweenSerenityAndNostalgiaGO;

    private int intensityValue;
    private int nostalgiaAreaCounter;
    private int serenityAreaCounter;
    private bool nostalgiaAreaComplete;
    private bool serenityAreaComplete;

    // Start is called before the first frame update
    void Start()
    {
        PlayStateChange.SRF = SRF;
        List<GameObject> childObjects = new List<GameObject>();
        foreach (Transform child in GetComponentsInChildren<Transform>( ))
        {
            if(child != transform)
            {          
                child.gameObject.GetComponent<SoundTrigger>().lvlManager = this;
                childObjects.Add(child.gameObject);
            }
  
        }
        soundZones = childObjects.ToArray();

        SRF.SetActive(false);
        score.SetText(FoundSound.ToString() + "/" + soundZones.Length.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForAreaCompletion(string tag)
    {
        switch(tag)
        {
            case "NostalgiaArea":
                nostalgiaAreaCounter++;
                if(nostalgiaAreaCounter >= nostalgiaAreaMax)
                {
                    intensityValue++;
                    backgroundMusicInstance.SetParameter("Intensity", intensityValue);
                    areaCompleted.text = intensityValue + "/4";
                    nostalgiaAreaComplete = true;
                }
                break;

            case "SerenityArea":
                serenityAreaCounter++;
                if (serenityAreaCounter >= serenityAreaMax)
                {
                    intensityValue++;
                    backgroundMusicInstance.SetParameter("Intensity", intensityValue);
                    areaCompleted.text = intensityValue + "/4";
                    serenityAreaComplete = true;
                }
                break;
        }
        Debug.Log(intensityValue);
        linkBetweenSerenityAndNostalgiaGO.SetActive(nostalgiaAreaComplete && serenityAreaComplete);
    }

    public void SoundFound()
    {
        FoundSound++;
        score.SetText(FoundSound.ToString() + "/" + soundZones.Length.ToString());
        LvlCompletion = (float)FoundSound / soundZones.Length;
    }

    public void LvlFinished()
    {
        foreach(GameObject _soundZone in soundZones)
        {
            _soundZone.SetActive(false);
        }
        SRF.SetActive(true);
        BellPS.gameObject.SetActive(true);
    }

    public void enterSoundZone(int Cases)
    {
        switch(Cases)
        {
            case 0:
                text.SetText("Je crois entendre quelque chose \n (presse Espace)");
                break;
            case 1:
                text.SetText("Connais tous tes sons et la cloche sonnera");
                break;
            case 2:
                text.SetText("Faire sonner la cloche ? \n (presse Espace)");
                break;
        }
    }
    public void exitSoundZone()
    {
        text.SetText("");
    }
}

[InitializeOnLoad]
static public class PlayStateChange
{
    static public ScriptableRendererFeature SRF;

    static PlayStateChange()
    {
        EditorApplication.playModeStateChanged += ModeChanged;
    }

    static void ModeChanged(PlayModeStateChange playModeState)
    {
        if (playModeState == PlayModeStateChange.ExitingPlayMode)
        {
            SRF.SetActive(true);
        }
    }
}
