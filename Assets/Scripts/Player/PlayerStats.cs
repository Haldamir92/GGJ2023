using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class PlayerStats : MonoBehaviour
{
    public void Die()
    {
        gameObject.SetActive(false);
    }
}
