using UnityEngine;
using UnityEngine.UI;

public class LineDrawNode : MonoBehaviour
{
    // Unique identifier for this node, useful for distinguishing between nodes.
    public int nodeID;

    // The value that this node holds, typically displayed on the node.
    public string nodeValue;

    // Reference to the Text component that displays the node's value.
    [SerializeField] private Text nodetext;

    private void Awake()
    {
        // Initialize the node's value from the associated Text component, if available.
        if (nodetext != null)
        {
            nodeValue = nodetext.text; // Set the nodeValue based on the displayed text.
        }
    }
}
