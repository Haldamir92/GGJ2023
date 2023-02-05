using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInstancer : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;

    private void Awake()
    {
        InstanceNode(transform.position, transform.rotation);
    }

    public GameObject InstanceNode(Vector3 position, Quaternion rotation)
    {
        GameObject gObj = Instantiate(nodePrefab, position, rotation);
        gObj.GetComponent<TreeNodeManager>().Instancer = this;
        return gObj;
    }
}