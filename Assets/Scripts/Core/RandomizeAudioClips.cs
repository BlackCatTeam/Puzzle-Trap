using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Core {
	public class RandomizeAudioClips : MonoBehaviour
	{
		[SerializeField]AudioClip[] audiosToRandomize;
		[SerializeField] float minPitchValue;
		[SerializeField] float maxPitchValue;
		AudioSource audioSource;

		

        private void Start()
        {
            
			AudioClip clip = RandomClip();
			if (clip != null)
				GetAudioSource().clip = RandomClip();
			RandomizeAudioPitch(minPitchValue, maxPitchValue);

		}


		AudioSource GetAudioSource()
        {
			if (audioSource == null)
				audioSource = GetComponent<AudioSource>();
			return audioSource;
		}
        private AudioClip RandomClip()
		{
			if (audiosToRandomize.Length == 0) return null;
			System.Random random = new System.Random();
			return audiosToRandomize[random.Next(0, audiosToRandomize.Length)];
        }
		private void RandomizeAudioPitch(double min = 0f , double max = 1f)
        {
			double range = (double)max - (double)min;
			System.Random random= new System.Random();
			double sample = random.NextDouble();
			double scaled = (sample * range) + min;
			GetAudioSource().pitch = (float)scaled;
			return;
        }

		public void PlayAudio()
		{
			if (audiosToRandomize.Length == 0) return;

			GetAudioSource().clip = RandomClip();			
			GetAudioSource().Play();
		}


    }
}
