using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {

	[SerializeField]
	private AudioClip placementSound;

	[SerializeField]
	private AudioClip movementSound;

	[SerializeField]
	private AudioClip executeSound;

	[SerializeField]
	private List<AudioClip> planetConquered;

	[SerializeField]
	private List<AudioClip> planetLost;

	private AudioSource _source;

    void Awake() {
		_source = GetComponent<AudioSource>();
    }

	public void PlacementSound()
	{
		_source.PlayOneShot(placementSound);
	}

	public void MovementSound()
	{
		_source.PlayOneShot(movementSound);
	}

	public void ExecuteSound()
	{
		_source.PlayOneShot(executeSound);
	}

	public void PlanetConquered()
	{
		_source.PlayOneShot(planetConquered.PickRandom());
	}

	public void PlanetLost()
	{
		_source.PlayOneShot(planetLost.PickRandom());
	}

}