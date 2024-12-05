using UnityEngine;
using UnityEngine.UI;

//This is class is reponsible for storing data related to nodes
public class LineDrawNode : MonoBehaviour
{
    public int nodeID; // Unique identifier for this node
    public string nodeValue; //The value each node holds

    [SerializeField] private Text nodetext;

    private void Awake()
    {
        if (nodetext != null)
        {
            nodeValue = nodetext.text;
        }
    }

}