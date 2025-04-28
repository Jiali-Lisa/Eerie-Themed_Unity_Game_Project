using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GeneratePaper : MonoBehaviour
{
    [SerializeField] private int size = 1;
    [SerializeField] private int resolution = 7;

    private MeshFilter filter;
    private MeshRenderer meshRenderer;

    private int currentResolution;
    private int currentSize;

    private void Start()
    {
        GenerateInitalMesh();
    }

    private void OnValidate()
    {
        if (!filter || !meshRenderer)
            GenerateInitalMesh();

        if (resolution != currentResolution || currentSize != size)
            UpdatePaperMesh(filter, resolution, size);

        currentResolution = resolution;
        currentSize = size;
    }

    private void GenerateInitalMesh()
    {
        filter = this.GetComponent<MeshFilter>();
        meshRenderer = this.GetComponent<MeshRenderer>();

        currentResolution = resolution;
        currentSize = size;

        GeneratePaperMesh(meshRenderer, filter, resolution, size);
    }

    private Mesh GeneratePaperMesh(MeshRenderer renderer, MeshFilter filter, int resolution, int size)
    {
        Mesh paperMesh = UpdatePaperMesh(filter, resolution, size);

        return paperMesh;
    }

    private Mesh UpdatePaperMesh(MeshFilter filter, int resolution, int size)
    {
        Mesh paperMesh = new Mesh();

        int vertexPerRow = GetVertexPerRow(resolution);
        int numberOfVertices = vertexPerRow * vertexPerRow;
        Vector3[] vertices = GetVertices(vertexPerRow, numberOfVertices, size);
        Vector2[] uvs = GetUVs(vertexPerRow, numberOfVertices, size);

        int[] triangles = GetTriangles(vertexPerRow, resolution);

        paperMesh.vertices = vertices;
        paperMesh.triangles = triangles;
        paperMesh.uv = uvs;

        paperMesh.RecalculateNormals();

        filter.mesh = paperMesh;

        return paperMesh;
    }

    private int GetVertexPerRow(int resolution)
    {
        int vertexCount = 2;

        for (int i = 0; i < resolution; i++)
        {
            vertexCount += (int)Mathf.Pow(2, i);
        }

        return vertexCount;
    }

    private int[] GetTriangles(int vertexPerRow, int resolution)
    {
        int vertexCount = vertexPerRow * vertexPerRow;

        int spaceNeededToStoreATriangle = 6;
        int trianglesNumber = (2 * ((int)Mathf.Pow(2, 2 * resolution))) * 3;
        int[] triangles = new int[trianglesNumber * spaceNeededToStoreATriangle];

        int triangleCounter = 0;

        for (int i = 0; triangleCounter < (vertexCount - vertexPerRow); i += 6)
        {
            if (i != 0 && ((i / 6) + 1) % vertexPerRow == 0)
            {
                triangleCounter++;
                continue;
            }

            triangles[i] = triangleCounter;
            triangles[i + 1] = triangleCounter + vertexPerRow + 1;
            triangles[i + 2] = triangleCounter + vertexPerRow;

            triangles[i + 3] = triangleCounter;
            triangles[i + 4] = triangleCounter + 1;
            triangles[i + 5] = triangleCounter + vertexPerRow + 1;

            triangleCounter++;
        }
        return triangles;
    }

    private Vector3[] GetVertices(int vertexPerRow, int numberOfVertices, int size)
    {
        Vector3[] vertices = new Vector3[numberOfVertices];

        float vertexOffset = (float)size / (vertexPerRow - 1);

        float xStartOffset = -((float)size / 2);
        float zStartOffset = 0;

        for (int i = 0; i < vertexPerRow; i++)
        {
            for (int j = 0; j < vertexPerRow; j++)
            {
                float xPosition = xStartOffset + (vertexOffset * j);
                vertices[j + i * vertexPerRow] = new Vector3(xPosition, 0f, zStartOffset);
            }

            zStartOffset -= vertexOffset;
        }
        return vertices;
    }

    private Vector2[] GetUVs(int vertexPerRow, int numberOfVertices, int size)
    {
        Vector2[] vertices = new Vector2[numberOfVertices];

        float vertexOffset = (float)size / (vertexPerRow - 1);

        float xStartOffset = 0;
        float zStartOffset = (float)size;

        for (int i = 0; i < vertexPerRow; i++)
        {
            for (int j = 0; j < vertexPerRow; j++)
            {
                float xPosition = xStartOffset + (vertexOffset * j);
                vertices[j + i * vertexPerRow] = new Vector3(xPosition, zStartOffset);
            }

            zStartOffset -= vertexOffset;
        }
        return vertices;
    }
}
