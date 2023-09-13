using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunSoundPool : MonoBehaviour
{
    [SerializeField]
    private GameObject soundPrefab;
    private Queue<GameObject> queue = new();

	private void Awake()
	{
		foreach (Transform t in transform)
        {
            queue.Enqueue(t.gameObject);
            t.gameObject.SetActive(false);
        }
	}

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

    public void Push(GameObject item)
    {
        queue.Enqueue(item);
    }
}
