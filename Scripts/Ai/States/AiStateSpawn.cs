﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows tower to spawn new obects with cooldown.
/// </summary>
public class AiStateSpawn : MonoBehaviour, IAiState
{
    // Cooldown for between spawns
    public float cooldown = 10f;
    // Max number of spawned obects in buffer
    public int maxNum = 2;
    // Spawned obects will be alive after AI destroing
    public float delayAfterDestroy = 2f;
    // Spawned object prefab
    public GameObject prefab;
    // Position for spawning
    public Transform spawnPoint;
    // Go to this state if agressive event occures
    public string agressiveAiState;
    // Go to this state if passive event occures
    public string passiveAiState;

    // AI behavior of this object
    private AiBehavior aiBehavior;
    // Defend points for this tower
    private DefendPoint defPoint;
    // Counter for cooldown calculation
    private float cooldownCounter;
    // Buffer with spawned objects
    private Dictionary<GameObject, Transform> defendersList = new Dictionary<GameObject, Transform>();

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
    }

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake ()
    {
        cooldownCounter = cooldown;
        aiBehavior = GetComponent<AiBehavior>();
        defPoint = transform.parent.GetComponentInChildren<DefendPoint>();
        Debug.Assert (aiBehavior && spawnPoint && defPoint, "Wrong initial parameters");
    }

    /// <summary>
    /// Raises the state enter event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
    public void OnStateEnter (string previousState, string newState)
    {

    }

    /// <summary>
    /// Raises the state exit event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
    public void OnStateExit (string previousState, string newState)
    {

    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update ()
    {
        cooldownCounter += Time.deltaTime;
        if (cooldownCounter >= cooldown)
        {
            // Try to spawn new object on cooldown
            if (TryToSpawn() == true)
            {
                cooldownCounter = 0f;
            }
            else
            {
                cooldownCounter = cooldown;
            }
        }
    }

    /// <summary>
    /// Gets the free defend position if it is.
    /// </summary>
    /// <returns>The free defend position.</returns>
    /// <param name="index">Index.</param>
    private Transform GetFreeDefendPosition()
    {
        Transform res = null;
        List<Transform> points = defPoint.GetDefendPoints();
        foreach (Transform point in points)
        {
            // If this point not busy already
            if (defendersList.ContainsValue(point) == false)
            {
                res = point;
                break;
            }
        }
        return res;
    }

    /// <summary>
    /// Try to spawn new object.
    /// </summary>
    /// <returns><c>true</c>, if to spawn was tryed, <c>false</c> otherwise.</returns>
    private bool TryToSpawn()
    {
        bool res = false;
        // If spawned objects number less then max allowed number
        if ((prefab != null) && (defendersList.Count < maxNum))
        {
            Transform position = GetFreeDefendPosition();
            // If there are free defend position
            if (position != null)
            {
                // Create new obect
                GameObject obj = Instantiate<GameObject>(prefab, spawnPoint.position, spawnPoint.rotation);
                // Set destination position
                obj.GetComponent<AiStateMove>().destination = position;
                // Add spawned object to buffer
                defendersList.Add(obj, position);
                res = true;
            }
        }
        return res;
    }

    /// <summary>
    /// Raises on every unit die.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void UnitDie (GameObject obj, string param)
    {
        // If this is object from my spawn buffer
        if (defendersList.ContainsKey(obj) == true)
        {
            // Remove it from buffer
            defendersList.Remove(obj);
        }
    }

    /// <summary>
    /// Triggers the enter.
    /// </summary>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
    public void TriggerEnter(Collider2D my, Collider2D other)
    {

    }

    /// <summary>
    /// Triggers the stay.
    /// </summary>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
    public void TriggerStay(Collider2D my, Collider2D other)
    {

    }

    /// <summary>
    /// Triggers the exit.
    /// </summary>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
    public void TriggerExit(Collider2D my, Collider2D other)
    {

    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
        foreach (KeyValuePair<GameObject, Transform> defender in defendersList)
        {
            // Destroy all spawned objects after delay
            Destroy(defender.Key, delayAfterDestroy);
        }
    }
}
