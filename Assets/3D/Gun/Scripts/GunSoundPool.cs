using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunSoundPool : MonoBehaviour
{
    // 사운드 풀에 담길 오브젝트의 프리팹
    [SerializeField]
    private GameObject soundPrefab;

    // 실제 사운드 오브젝트가 담길 주축(자료구조 큐)
    private Queue<GameObject> queue = new();

	private void Awake()
	{
        // 이 오브젝트 풀의 자식을 모두 큐에 넣는다
		foreach (Transform t in transform)
        {
            queue.Enqueue(t.gameObject);
            t.gameObject.SetActive(false);
        }
	}

    /// <summary> 큐에서 사운드 오브젝트를 꺼낸다. </summary>
    /// <returns> goOut(사운드가 담긴 오브젝트)를 반환한다. </returns>
	public GameObject Pop()
    {
        GameObject goOut;
        queue.TryDequeue(out goOut);

        if (goOut != null)
        {
            goOut.SetActive(true);
            return goOut;
        } else
        {
            goOut = Instantiate(soundPrefab, transform);
            return goOut;
        }
    }

    /// <summary> 사운드 오브젝트를 큐에 도로 넣는다. </summary>
    /// <param name="item"></param>
    public void Push(GameObject item)
    {
        queue.Enqueue(item);
    }
}
