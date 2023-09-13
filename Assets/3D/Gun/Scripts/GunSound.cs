using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
	private GunSoundPool pool;
	[SerializeField]
	private AudioSource source;
    public AudioClip nowClip;
	private float playTime;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		pool = GetComponentInParent<GunSoundPool>();
	}

	private void OnEnable()
	{
		source.Play();
		playTime = source.clip.length;
		StartCoroutine(DisableThis());
	}

	public void SetClip(AudioClip clip)
	{
		nowClip = clip;
	}

	IEnumerator DisableThis()
	{
		yield return new WaitForSeconds(playTime);
		pool.Push(gameObject);
		gameObject.SetActive(false);
	}
}
