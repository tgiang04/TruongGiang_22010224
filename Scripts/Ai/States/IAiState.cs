using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAiState
{
    void OnStateEnter (string previousState, string newState);
    void OnStateExit (string previousState, string newState);
    void TriggerEnter (Collider2D my, Collider2D other);
    void TriggerStay (Collider2D my, Collider2D other);
    void TriggerExit (Collider2D my, Collider2D other);
}
