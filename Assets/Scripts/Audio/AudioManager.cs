using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Random = System.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return;} // Singleton
        Instance = this;
        
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    public void PlayOneShot(EventReference sound, Vector2 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.getPitch(out float pitch);
        eventInstance.setPitch(pitch + UnityEngine.Random.Range(-1.0f, 1.0f));
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterObject)
    {
        StudioEventEmitter emitter = emitterObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
