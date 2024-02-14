using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // 큐브 프리팹을 연결할 변수
    public int numberOfCubes = 10; // 생성할 큐브 수
    public float minDistance = 1.0f; // 최소 간격
    public int width;
    public int height;

    private List<Vector3> cubePositions = new List<Vector3>(); // 큐브 위치를 저장할 리스트

    void Start()
    {
        // 큐브를 무작위 위치에 생성
        for (int i = 0; i < numberOfCubes; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(cubePrefab, randomPosition, Quaternion.identity);
        }
    }

    // 무작위 위치를 반환하며 최소 간격을 유지
    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition;
        bool validPosition = false;

        do
        {
            // 무작위 위치 생성
            randomPosition = new Vector3(Random.Range(0, this.width), 0f, Random.Range(0, this.height));

            validPosition = true;

            // 생성된 위치와 다른 큐브들 간의 거리 확인
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

        // 위치를 리스트에 추가하고 반환
        cubePositions.Add(randomPosition);
        return randomPosition;
    }
}
