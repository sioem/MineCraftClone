using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // ť�� �������� ������ ����
    public int numberOfCubes = 10; // ������ ť�� ��
    public float minDistance = 1.0f; // �ּ� ����
    public int width;
    public int height;

    private List<Vector3> cubePositions = new List<Vector3>(); // ť�� ��ġ�� ������ ����Ʈ

    void Start()
    {
        // ť�긦 ������ ��ġ�� ����
        for (int i = 0; i < numberOfCubes; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(cubePrefab, randomPosition, Quaternion.identity);
        }
    }

    // ������ ��ġ�� ��ȯ�ϸ� �ּ� ������ ����
    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition;
        bool validPosition = false;

        do
        {
            // ������ ��ġ ����
            randomPosition = new Vector3(Random.Range(0, this.width), 0f, Random.Range(0, this.height));

            validPosition = true;

            // ������ ��ġ�� �ٸ� ť��� ���� �Ÿ� Ȯ��
            foreach (Vector3 position in cubePositions)
            {
                if (Vector3.Distance(randomPosition, position) < minDistance)
                {
                    validPosition = false;
                    break;
                }
                else
                {
                    Debug.Log(Vector3.Distance(randomPosition, position));
                }
            }


        } while (!validPosition);

        // ��ġ�� ����Ʈ�� �߰��ϰ� ��ȯ
        cubePositions.Add(randomPosition);
        return randomPosition;
    }
}
