using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetWorkObjectsSpawner : MonoBehaviour
{
    [Header("Oject")]
    [SerializeField] GameObject networkGamObject;
    [SerializeField] GameObject instantiatedGamObject;

    private void Awake()
    {
    }

    private void Start()
    {
        WorldObjectManager.instance.SpawnObjects(this);
        gameObject.SetActive(false);

    }

    public void AttempToSpawnCharacter()
    {
        if (networkGamObject != null)
        {
            instantiatedGamObject = Instantiate(networkGamObject);
            instantiatedGamObject.transform.position = transform.position;
            instantiatedGamObject.transform.rotation = transform.rotation;
            instantiatedGamObject.GetComponent<NetworkObject>().Spawn();

        }
    }
}
