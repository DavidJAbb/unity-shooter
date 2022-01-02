using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    // Singleton
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public Player player;
    public UserInterfaceManager hud;
    public DialogueManager dialogManager;
    public ReadableManager readableManager;

    [Header("Layer Masks")]
    public LayerMask shootableLayers;

    [Header("FX Prefabs - Impacts")]
    public GameObject[] bulletHolePrefabs;
    public GameObject[] hitParticlePrefabs;

    [Header("Audio FX - Impacts")]
    public AudioClip[] fleshImpactSounds;
    public AudioClip[] concreteImpactSounds;
    public AudioClip[] metalImpactSounds;
    public AudioClip[] dirtImpactSounds;

    public enum SoundType
    {
        IMPACT_FLESH,
        IMPACT_CONCRETE,
        IMPACT_METAL,
        IMPACT_DIRT
    }

    public GameObject distractionPrefab;

    public bool InMenu { get; set; }


    void Awake()
    {
        dialogManager = GetComponent<DialogueManager>();
        readableManager = GetComponent<ReadableManager>();
    }


    void Update()
    {
        if(readableManager.IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                readableManager.CloseReadable();
            }
        }
    }


    public void SpawnHitFX(RaycastHit hit)
    {
        Quaternion normalRotation = Quaternion.LookRotation(hit.normal);

        if (hit.collider.CompareTag("Flesh"))
        {
            PlaySoundOfTypeAtPosition(SoundType.IMPACT_FLESH, hit.point);
            // SpawnPrefab(bulletHolePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
            // SpawnPrefab(hitParticlePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
        }
        else if (hit.collider.CompareTag("Concrete"))
        {
            PlaySoundOfTypeAtPosition(SoundType.IMPACT_CONCRETE, hit.point);
            SpawnPrefab(bulletHolePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
            SpawnPrefab(hitParticlePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
        }
        else if (hit.collider.CompareTag("Metal"))
        {
            PlaySoundOfTypeAtPosition(SoundType.IMPACT_METAL, hit.point);
            SpawnPrefab(bulletHolePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
            SpawnPrefab(hitParticlePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
        }
        else if (hit.collider.CompareTag("Dirt"))
        {
            PlaySoundOfTypeAtPosition(SoundType.IMPACT_DIRT, hit.point);
            SpawnPrefab(bulletHolePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
            SpawnPrefab(hitParticlePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
        }
        // Fallback...
        else if (hit.collider.CompareTag("Untagged"))
        {
            SpawnPrefab(bulletHolePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
            SpawnPrefab(hitParticlePrefabs[0], hit.point, normalRotation, hit.collider.transform, true);
        }

        // TODO Maybe bullet hole prefab changes depending on type of weapon / bullet?
    }


    public void SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool randomRotation)
    {
        GameObject clone = Instantiate(prefab, position, rotation);
        if(randomRotation)
        {
            clone.transform.RotateAround(clone.transform.position, clone.transform.forward, Random.Range(0, 360));
        }
        clone.transform.SetParent(parent);
    }


    public void SpawnDistraction(int priority, Vector3 origin, float radius)
    {
        GameObject cloneDistraction = Instantiate(distractionPrefab, origin, Quaternion.identity) as GameObject;
        cloneDistraction.GetComponent<Distraction>().priority = priority;
    }


    public void PlaySoundOfTypeAtPosition(SoundType type, Vector3 pos)
    {
        AudioClip clipToPlay;

        switch(type)
        {
            case SoundType.IMPACT_FLESH:
                clipToPlay = fleshImpactSounds[Random.Range(0, fleshImpactSounds.Length)];
                break;
            case SoundType.IMPACT_CONCRETE:
                clipToPlay = concreteImpactSounds[Random.Range(0, concreteImpactSounds.Length)];
                break;
            case SoundType.IMPACT_METAL:
                clipToPlay = metalImpactSounds[Random.Range(0, metalImpactSounds.Length)];
                break;
            case SoundType.IMPACT_DIRT:
                clipToPlay = dirtImpactSounds[Random.Range(0, dirtImpactSounds.Length)];
                break;
            default:
                clipToPlay = concreteImpactSounds[0]; // Default sound...
                break;
        }

        AudioSource.PlayClipAtPoint(clipToPlay, pos);
    }
}
