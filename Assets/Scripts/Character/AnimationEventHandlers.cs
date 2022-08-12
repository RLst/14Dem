using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace LeMinhHuy.Character
{
	public class AnimationEventHandlers : MonoBehaviour
	{
		[Range(0f, 1f)][SerializeField] float footstepAudioVolume = 0.5f;
		[SerializeField] AudioClip landingAudioClip;
		[SerializeField] AudioClip[] footstepAudioClips;

		//Members
		CharacterController controller;

		void Awake()
		{
			controller = GetComponent<CharacterController>();
		}

		void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
			{
				if (footstepAudioClips.Length > 0)
				{
					var index = Random.Range(0, footstepAudioClips.Length);
					AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(controller.center), footstepAudioVolume);
				}
			}
		}

		void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
			{
				AudioSource.PlayClipAtPoint(landingAudioClip, transform.TransformPoint(controller.center), footstepAudioVolume);
			}
		}
	}
}