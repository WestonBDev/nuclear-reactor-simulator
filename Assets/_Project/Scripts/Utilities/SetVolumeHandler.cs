using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace JapPixel
{
    public class SetVolumeHandler : MonoBehaviour
    {
        public AudioMixer mixer;
        public string paramLabel;
        public Slider volumeSlider;

        public void SetVolume(float volume)
        {
           
            mixer.SetFloat(paramLabel, volume);
        }

        public void Increase()
        {
            mixer.GetFloat(paramLabel, out float value);

            if (value >= 20f) return;

			mixer.SetFloat(paramLabel, value + 10f);
            volumeSlider.value += 10f;
		}

        public void Decrease()
        {
			mixer.GetFloat(paramLabel, out float value);

			if (value <= -80f) return;

			mixer.SetFloat(paramLabel, value - 10f);
			volumeSlider.value -= 10f;
		}
    }
}
