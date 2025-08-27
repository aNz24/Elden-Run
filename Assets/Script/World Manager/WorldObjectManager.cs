using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager instance;


    [Header("Ojects")]
    [SerializeField] List<NetWorkObjectsSpawner> networkObjectsSpawners;
    [SerializeField] List<GameObject> spawnedInObject;

    [Header("Fog Walls")]
    public List<FogWallInteractable> fogWalls;

    [Header("Site Of Garce")]
    public List<SiteOfGraceInteractable> siteOfGrace;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SpawnObjects(NetWorkObjectsSpawner networkObjectSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            networkObjectsSpawners.Add(networkObjectSpawner);
            networkObjectSpawner.AttempToSpawnCharacter();
        }

    }

    public void AddFogWallToList(FogWallInteractable fogWall)
    {
        if (!fogWalls.Contains(fogWall))
        {
            fogWalls.Add(fogWall);
        }
    }

    public void RemoveFogWallFromList(FogWallInteractable fogWall)
    {
        if (fogWalls.Contains(fogWall))
        {
            fogWalls.Remove(fogWall);
        }
    }


    public void AddSiteOfGraceToList(SiteOfGraceInteractable siteOfGarce)
    {
        if (!siteOfGrace.Contains(siteOfGarce))
        {
            siteOfGrace.Add(siteOfGarce);
        }
    }

    public void RemoveSiteOfGraceFromList(SiteOfGraceInteractable siteOfGarce)
    {
        if (siteOfGrace.Contains(siteOfGarce))
        {
            siteOfGrace.Remove(siteOfGarce);
        }
    }


}
