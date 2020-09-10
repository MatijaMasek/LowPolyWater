using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LowPolyWater : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;

    int[] triangles;

    [Header("Grid Size")]
    [SerializeField] int xSize = 20;
    [SerializeField] int zSize = 20;

    [Header("Noise")]
    [SerializeField] float xNoise = 0.5f;
    [SerializeField] float zNoise = 0.5f;
    [SerializeField] float noisePower = 1f;

    float elapsedTime = 0f;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        elapsedTime = Time.time;

        CreateShape();

        UpdateMesh();

    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        int i = 0;

        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * xNoise + elapsedTime, z * zNoise + elapsedTime) * noisePower;

                vertices[i] = new Vector3(x, y, z);

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
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

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

}