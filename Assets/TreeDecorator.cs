using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class TreeDecorator : MonoBehaviour
{
    [SerializeField] private GameObjectValueList treeHeads;

    public void Decorate(GameObject g)
    {
        List<GameObject> heads = new List<GameObject>(treeHeads.List);
        if (heads.Count > 0)
        {
            heads.RemoveAt(0);
            GameObject head = heads.GetRandomElement();
            treeHeads.Remove(head);
            Instantiate(g, head.transform.position, head.transform.rotation);
        }
    }
}