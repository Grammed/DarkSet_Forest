using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
	// 사운드 오브젝트 풀
	private GunSoundPool pool;

	// 사운드가 플레이되는 소스
	[SerializeField]
	private AudioSource source;

	// 발사 사운드
    public AudioClip nowClip;

	// 클립 재생 시간
	private float playTime;

	private bool inPool = true;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		pool = GetComponentInParent<GunSoundPool>();
	}

	private void OnEnable()
	{
		inPool = false;
		playTime = source.clip.length;
		StartCoroutine(DisableThis());
	}

	public void SetClip(AudioClip clip)
	{
		nowClip = clip;
	}

	IEnumerator DisableThis()
	{
		// 클립 재생 시간이 지나면 Disable
		yield return new WaitForSeconds(playTime);
		inPool = true;
		pool.Push(gameObject);
		gameObject.SetActive(false);
	}
}
