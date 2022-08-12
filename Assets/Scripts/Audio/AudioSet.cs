using System;
using System.Collections.Generic;
using UnityEngine;

namespace LeMinhHuy.Audio
{
	[CreateAssetMenu(menuName = "WalkieTalkie/Audio Set", fileName = "New Audio Set")]
	public class AudioSet : ScriptableObject
	{
		[Range(0f, 1f)] public float volume = 1f;
		[Range(0, 100)] public int chance = 100;
		public bool avoidRepetitions = true;
		public List<AudioClip> clips;

		//Members
		internal AudioClip lastPlayed = null;
	}
}
