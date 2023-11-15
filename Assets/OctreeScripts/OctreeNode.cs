using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode 
{
    Bounds nodeBounds; // The bounds of the current node in 3D space.
    float minSize; // The minimum size for any node in the tree.
    Bounds[] childBounds; // Bounds for potential children nodes.
    OctreeNode[] children = null; // Array of child nodes.

    // Constructor for the OctreeNode class.
    public OctreeNode(Bounds b, float minNodeSize)
    {
        nodeBounds = b;
        minSize = minNodeSize;

        // Divide current bounds into 8 smaller bounds for the child nodes.
        float quarter = nodeBounds.size.x / 4.0f;
        float childLength = nodeBounds.size.y/ 2;
        Vector3 childSize = new Vector3(childLength, childLength, childLength);
        childBounds = new Bounds[8];

        // Each child bounds is offset from the center of the current node.
        // These offsets ensure all 8 children fill the space of the current node.
        // The ordering is somewhat arbitrary but typically follows a consistent pattern.
        // These particular offsets start from the top-back-left and go in a clockwise pattern for both top and bottom layers.
        // ... (continue for all 8 children)
    }

    // Method to add a GameObject to the octree.
    public void AddObject(GameObject go)
    {
        DivideAndAdd(go);
    }

    // This method attempts to add a GameObject to the deepest appropriate node.
    public void DivideAndAdd(GameObject go)
    {
        if(nodeBounds.size.y <= minSize) // If current node size is the smallest allowable, don't further divide.
        {
            return;
        }
        
        if (children == null) // If children aren't already created, initialize them.
        {
            children = new OctreeNode[8];
        }
        
        bool dividing = false; // Flag to check if we've added the object to any child.
        
        for(int i = 0; i < 8; i++)
        {
            if(children[i] == null)
            {
                children[i] = new OctreeNode(childBounds[i], minSize); // Create a child node if it doesn't exist yet.
            }

            // If the GameObject's bounds intersects with the child's bounds, try to add to that child.
            if(childBounds[i].Intersects(go.GetComponent<Collider>().bounds))
            {
                dividing = true;
                children[i].DivideAndAdd(go);
            }
        }

        if(dividing == false)
        {
            children = null; // If object didn't fit into any child, reset the children.
        }
    }

    // Method to visualize the octree in Unity's scene view using Gizmos.
    public void Draw()
    {
        Gizmos.color = new Color(0, 1, 0); // Set Gizmo color to green.
        Gizmos.DrawWireCube(nodeBounds.center, nodeBounds.size); // Draw the current node's bounds.
        
        if(children != null)
        {
            for(int i = 0; i < 8; i++) // Recursively draw all child nodes.
            {
                if (children[i] != null)
                {
                    children[i].Draw();
                }
            }
        }
    }
}