using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalControllerX : MonoBehaviour
{
    public int goals = 0;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            goals++;
        }
    }
}
