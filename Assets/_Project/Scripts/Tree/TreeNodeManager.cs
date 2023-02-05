using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNodeManager : MonoBehaviour
{
    [SerializeField] private PrefabSpawner spawner;
    [SerializeField] private float multiplier;
    private int branchesLevel = 50;
    private int nodeSize = 5;
    [SerializeField] private int minNodeGeneration = 2;
    [SerializeField] private int maxNodeGeneration = 3;

    [SerializeField] private GameObject treeNodePrefab;
    private Vector3 direction;

    private int currentLenght;
    private bool canGrow = true;

    private NodeInstancer instancer;
    public NodeInstancer Instancer { set => instancer = value; }

    private void Start()
    {
        branchesLevel = Random.Range(branchesLevel - 50, branchesLevel);
        //Debug.Log(branchesLevel);
    }

    private void IncreaseLenght()
    {
        transform.position += Vector3.up * multiplier;

        if (nodeSize < 10)
        {
            transform.position += direction * multiplier;
        }
    }

    public void SetSize(int size)
    {
        if (size <= 1)
        {
            canGrow = false;
        }
        spawner.SetScale(size);
        nodeSize = size;
    }

    public void OnLenghtIncrease()
    {
        if (canGrow)
        {
            if (currentLenght < branchesLevel)
            {
                currentLenght++;
                IncreaseLenght();
            }
            else
            {
                canGrow = false;
                Branch();
            }
        }
    }

    private void Branch()
    {
        int childrenNumber = Random.Range(minNodeGeneration, maxNodeGeneration + 1);
        GameObject[] nodes = new GameObject[childrenNumber];
        TreeNodeManager[] treeNodeManagers = new TreeNodeManager[childrenNumber];
        float[] deltaDirection = new float[childrenNumber];
        deltaDirection[0] = Random.Range(-3, 0.5f);
        deltaDirection[1] = Random.Range(0.5f, 3f);
        for (int i = 2; i < childrenNumber; i++)
        {
            deltaDirection[i] = Random.Range(-1f, 2f);
        }

        for (int i = 0; i < childrenNumber; i++)
        {
            nodes[i] = instancer.InstanceNode(transform.position, transform.rotation);
            treeNodeManagers[i] = nodes[i].GetComponent<TreeNodeManager>();
            SetNodeStats(treeNodeManagers[i], nodeSize, deltaDirection[i]);
        }
    }

    private void SetNodeStats(TreeNodeManager nodeManager, int nodeSize, float deltaDirection)
    {
        nodeManager.SetSize(nodeSize - 1);
        nodeManager.direction.x += deltaDirection;
    }
}