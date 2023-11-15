using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    public OctreeNode rootNode; // The root node of the octree.

    // Constructor for the Octree class.
    public Octree(GameObject[] worldObjects, float minNodeSize)
    {
        Bounds bounds = new Bounds(); // Represents the bounds of the entire octree.

        // Iterate over every game object provided and encapsulate their bounds.
        foreach(GameObject go in worldObjects)
        {
            go.AddComponent(typeof(BoxCollider)); // Add a BoxCollider to the GameObject if it doesn't already have one.
            bounds.Encapsulate(go.GetComponent<Collider>().bounds); // Expand the main bounds to include the object's bounds.
        }

        // To ensure the bounds is a cube, we take the largest dimension and set all three dimensions to that value.
        float maxSize = Mathf.Max(new float[] {bounds.size.x, bounds.size.y, bounds.size.z});
        Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 0.5f; // Half-size vector for convenience.
        
        // Adjust the bounds to be a perfect cube centered around the original bounds' center.
        bounds.SetMinMax(bounds.center - sizeVector, bounds.center + sizeVector);

        // Create the root node of the octree with these bounds.
        rootNode = new OctreeNode(bounds, minNodeSize);

        // Add all the world objects to the octree.
        AddObjects(worldObjects);
    }

    // Method to add multiple objects to the octree.
    public void AddObjects(GameObject[] worldObjects)
    {
        foreach (GameObject go in worldObjects)
        {
            rootNode.AddObject(go); // Add each object to the root node, which will handle further subdivisions.
        }
    }
}

