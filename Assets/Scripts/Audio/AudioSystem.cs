using System;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LeMinhHuy.Audio
{
	public class AudioSystem : MonoBehaviour
	{
		//Inspector
		// [TextArea(0, 2), SerializeField] string description = null;
		// [Header("Settings")]
		[SerializeField] AudioSet audioSet = null;
		[Tooltip("Chance of clip playing. OVERRIDES the magazine's chance")]
		[Range(0, 100), SerializeField] int chance = 50;

		[Space]
		[SerializeField] AudioMixerGroup output = null;
		[Tooltip("Lower values are higher priority")]
		[Range(0, 256), SerializeField] int _priority = 128;
		[Range(0f, 1f), SerializeField] float _volume = 1f;
		[Range(-3f, 3f), SerializeField] float _pitch = 1f;

		// [Header("Inbuilt Audio Set")]

		//Properties
		public int priority
		{
			get => (audioSource != null) ? audioSource.priority : _priority;
			set { if (audioSource) audioSource.priority = value; }
		}
		public float volume
		{
			get => (audioSource != null) ? audioSource.volume : _volume;
			set { if (audioSource) audioSource.volume = value; }
		}
		public float pitch
		{
			get => (audioSource != null) ? audioSource.pitch : _pitch;
			set { if (audioSource) audioSource.pitch = value; }
		}
		public bool isPlaying => audioSource.isPlaying;

		//Members
		AudioSource audioSource = null;

		void Awake()
		{
			//Try find audio source
			if (!audioSource)
				audioSource = GetComponent<AudioSource>();
			//Otherwise create an audio source
			if (!audioSource)
				audioSource = gameObject.AddComponent<AudioSource>();
		}

		void Start()
		{
			//Setup audiosource
			if (output != null)
				audioSource.outputAudioMixerGroup = output;
			audioSource.priority = _priority;
			audioSource.volume = _volume;
			audioSource.pitch = _pitch;
		}

		#region Normal Play
		public void PlayOnce(AudioClip clip) => audioSource.PlayOneShot(clip);
		#endregion

		#region Play by Chance
		/// <summary>
		/// Plays the current loaded magazine according to the chance setting on this audio system
		/// </summary>
		public void ChancePlay()
		{
			if (!audioSet) return;

			//Don't play the same sound over and over again
			while (true)
			{
				AudioClip clipToPlay = audioSet.clips[Random.Range(0, audioSet.clips.Count)];
				if (clipToPlay == audioSet.lastPlayed) continue;

				//Play by chance
				if (Random.Range(0, 100) < audioSet.chance)
				{
					audioSource.PlayOneShot(clipToPlay, audioSet.volume);
					audioSet.lastPlayed = clipToPlay;
				}
				break;
			}
		}
		/// <summary>
		/// Plays the selected clip according to the chance setting of this audio system
		/// </summary>
		/// <param name="clip">The audio clip to play based on chance</param>
		public void ChancePlay(AudioClip clip)
		{
			if (Random.Range(0, 100) < chance)
				audioSource.PlayOneShot(clip);
		}

		/// <summary>
		/// Only play an audio clip from the set by chance if no current sounds are being played
		/// </summary>
		public void NoOverlapChancePlaySet(AudioSet set)
		{
			if (!audioSource.isPlaying)
				ChancePlaySet(set);
		}

		/// <summary> Play one audio clip out of a magazine according to chance </summary>
		/// <param name="set">AudioMagazine scriptable object</param>
		public void ChancePlaySet(AudioSet set, float? volume = null)
		{
			if (!set) return;
			if (set.clips.Count == 0) return;

			//Don't play the same sound over and over again
			//NOTE: With brief testing, the max iterations was 3. Usually 1 or 2. Performance is fine
			int i = 0;
			while (true)
			{
				i++;

				//Infinite loop countermeasure
				if (i > 1000) return;

				AudioClip clipToPlay = set.clips[Random.Range(0, set.clips.Count)];
				if (set.avoidRepetitions &&
					set.clips.Count > 1 && //Need at least 2 clips to avoid repetitions
					clipToPlay == set.lastPlayed) continue;

				//Play by chance
				if (Random.Range(0, 100) < set.chance)
				{
					audioSource.PlayOneShot(clipToPlay, volume.HasValue ? volume.Value : set.volume);
					set.lastPlayed = clipToPlay;
				}
				break;
			}
		}

		/// <summary>
		/// Play a random clip in a magazine by chance using passed in audiosource
		/// ie. in case the sounds need to be routed through a different mixer
		/// </summary>
		public void ChancePlaySetBySource(AudioSet set, AudioSource source)
		{
			//Don't play the same sound over and over again
			//NOTE: With brief testing, the max iterations was 3. Usually 1 or 2. Performance is fine
			while (true)
			{
				AudioClip clipToPlay = set.clips[Random.Range(0, set.clips.Count)];
				if (set.avoidRepetitions &&
					set.clips.Count > 1 && //Need at least 2 clips to avoid repetitions, otherwise infinity loop
					clipToPlay == set.lastPlayed) continue;

				//Play by chance
				if (Random.Range(0, 100) < set.chance)
				{
					source.PlayOneShot(clipToPlay, set.volume);
					set.lastPlayed = clipToPlay;
				}
				break;
			}
		}

		public void Stop() => audioSource.Stop();
		#endregion

		#region Animation Event Handlers
		public void ChancePlayClip_Boxed(Object audioClip)
		{
			AudioClip ac = audioClip as AudioClip;
			if (Random.Range(0, 100) < chance)
				audioSource.PlayOneShot(ac);
		}

		/// <summary>
		///	Play an audio clip out of the set according to change
		/// Set pased will be unboxed to an AudioSet and relayed
		/// </summary>
		/// <param name="audioSet"></param>
		public void ChangePlaySet_Boxed(Object audioSet)
		{
			AudioSet set = audioSet as AudioSet;

			//Unbox
			if (!set)
			{
				print("Not an audio magazine!");
				return;
			}

			//Relay
			ChancePlaySet(set);
		}

		/// <summary> Play one audio clip once </summary>
		/// <param name="audioClip">Single AudioClip</param>
		public void PlayOnce_Boxed(Object audioClip)
		{
			AudioClip ac = audioClip as AudioClip;
			audioSource.PlayOneShot(ac);
		}
		#endregion
	}
}
