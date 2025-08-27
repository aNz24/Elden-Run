using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEngine.TextCore.Text;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Character")]
    [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> spawnedInCharacters;
    private Coroutine spawnAllCharacterCoroutine;
    private Coroutine despawnAllCharacterCoroutine;
    private Coroutine resetAllCharacterCoroutine;

    [Header("Loading")]
    public bool isPerformingLoadingOperation = false;

    [Header("Boss")]
    [SerializeField] List<AIBossCharacterManager> spawnedInBosses;

    [Header("Patrol Path")]
    [SerializeField] List<AIPatrolPath> aiPatrolPaths = new List<AIPatrolPath>(); 

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttempToSpawnCharacter();
        }

    }

    public void AddCharacterToSpawnedCharacterList(AICharacterManager character)
    {
        if (spawnedInCharacters.Contains(character))
            return;
        spawnedInCharacters.Add(character);

        AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

        if(bossCharacter != null)
        {
            if (spawnedInBosses.Contains(bossCharacter))
                return;
            spawnedInBosses.Add(bossCharacter);
        }
    }

    public AIBossCharacterManager GetBossCharacterByID(int id)
    {
        return spawnedInBosses.FirstOrDefault(boss => boss.bossID == id);
    }

    public void SpawnAllCharacter()
    {
        isPerformingLoadingOperation = true;
        
        if(spawnAllCharacterCoroutine != null)
            StopCoroutine(spawnAllCharacterCoroutine);

        spawnAllCharacterCoroutine = StartCoroutine(SpawnAllCharacterCoroutine());
    }

    private IEnumerator SpawnAllCharacterCoroutine()
    {
        for (int i = 0; i < aiCharacterSpawners.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            aiCharacterSpawners[i].AttempToSpawnCharacter();

            yield return null;
        }

        isPerformingLoadingOperation = false;

        yield return null ;
    }

    public void ResetAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (resetAllCharacterCoroutine != null)
            StopCoroutine(resetAllCharacterCoroutine);

        resetAllCharacterCoroutine = StartCoroutine(ResetAllCharacterCoroutine());
    }

    private IEnumerator ResetAllCharacterCoroutine()
    {
        for (int i = 0; i < aiCharacterSpawners.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            aiCharacterSpawners[i].ResetCharacter();

            yield return null;
        }

        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void DespawnAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (despawnAllCharacterCoroutine != null)
            StopCoroutine(despawnAllCharacterCoroutine);

        despawnAllCharacterCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());

    }

    public void DisableAllBossFights()
    {
        for (int i = 0; i < spawnedInBosses.Count; i++)
        {
            if (spawnedInBosses[i] == null)
                continue;

            spawnedInBosses[i].bossFightIsActive.Value = false;
        }
    }

    private IEnumerator DespawnAllCharactersCoroutine()
    {

        for (int i = 0; i < spawnedInCharacters.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            spawnedInCharacters[i].GetComponent<NetworkObject>().Despawn();

            yield return null;
        }

        spawnedInCharacters.Clear();

        isPerformingLoadingOperation = false;

        yield return null;
    }

    private void DisableAllCharacters()
    {

    }

    //PATROL PATHS
    public void AddPatrolPathToList(AIPatrolPath patrolPath)
    {
        if(aiPatrolPaths.Contains(patrolPath))
            return;

        aiPatrolPaths.Add(patrolPath);
    }

    public AIPatrolPath GetAIPatrolPathByID(int patrolPathID)
    {
        AIPatrolPath patrolPath =null;
        for (int i = 0; i < aiPatrolPaths.Count; i++)
        {
            if (aiPatrolPaths[i].patrolPathID == patrolPathID)
                patrolPath = aiPatrolPaths[i];
        }
        return patrolPath;
    }
}
