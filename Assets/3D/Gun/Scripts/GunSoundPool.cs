using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunSoundPool : MonoBehaviour
{
    // ���� Ǯ�� ��� ������Ʈ�� ������
    [SerializeField]
    private GameObject soundPrefab;

    public AudioClip sound;

    // ���� ���� ������Ʈ�� ��� ��(�ڷᱸ�� ť)
    private Queue<GameObject> queue = new();

	private void Awake()
	{
        // �� ������Ʈ�� Ʈ�������� �ڽ��� ��� ť�� �ִ´�
		foreach (Transform t in transform)
        {
            queue.Enqueue(t.gameObject);
            t.gameObject.SetActive(false);
        }

        soundPrefab = transform.GetChild(0).gameObject;
	}

    /// <summary> ť���� ���� ������Ʈ�� ������. </summary>
    /// <returns> goOut(���尡 ��� ������Ʈ)�� ��ȯ�Ѵ�. </returns>
	public GameObject Pop()
    {
        GameObject goOut;
        queue.TryDequeue(out goOut);

        if (goOut != null)
        {
            goOut.SetActive(true);
            AudioSource source = goOut.GetComponent<AudioSource>();
            source.clip = sound;
            source.Play();
            return goOut;
        } else
        {
            goOut = Instantiate(soundPrefab, transform);
            return goOut;
        }
    }

    /// <summary> ���� ������Ʈ�� ť�� ���� �ִ´�. </summary>
    /// <param name="item"></param>
    public void Push(GameObject item)
    {
        queue.Enqueue(item);
    }
}
