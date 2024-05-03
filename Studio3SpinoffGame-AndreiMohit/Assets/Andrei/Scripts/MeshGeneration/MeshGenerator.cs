using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [Header("Internal")]
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    Vector3[] vertices;
    int[] triangles;
    Vector3[] normals;

    [Header("Mesh Properties")]
    [SerializeField] int xSize;
    [SerializeField] int zSize;

    [SerializeField] Material material;

    [SerializeField] float heightMagnitude;
    [SerializeField] private Texture2D heightMap;

    void Start()
    {
        mesh = new Mesh();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.material = material;

        meshFilter.mesh = mesh;

        CreateShape();
        UpdateMesh();
        AddCollider();
    }

    void Update()
    {
        
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int index = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {

                //Color pixel = heightMap.GetPixel(-x, -z);
                //float y = pixel.a * heightMagnitude;

                float y = Mathf.PerlinNoise(x * .3f, z * .2f) * heightMagnitude;

                if((x % xSize) == 0 || (z % zSize) == 0)
                    y = 0f;

                vertices[index] = new Vector3(x, y, z);
                index++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }

        normals = new Vector3[vertices.Length];

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int vertIndexA = triangles[i];
            int vertIndexB = triangles[i + 1];
            int vertIndexC = triangles[i + 2];

            Vector3 edge1 = vertices[vertIndexB] - vertices[vertIndexA];
            Vector3 edge2 = vertices[vertIndexC] - vertices[vertIndexA];
            Vector3 faceNormal = Vector3.Cross(edge1, edge2).normalized;

            normals[vertIndexA] += faceNormal;
            normals[vertIndexB] += faceNormal;
            normals[vertIndexC] += faceNormal;
        }

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

       
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        //mesh.RecalculateNormals();
    }

    void AddCollider()
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        meshCollider.sharedMesh = meshFilter.mesh;
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + new Vector3(xSize / 2, 0f, zSize / 2), new Vector3(xSize, 1f, zSize));
    }
}
