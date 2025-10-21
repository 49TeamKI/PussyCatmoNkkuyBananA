using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneToggle : MonoBehaviour
{
    
    public KeyCode pushToTalkKey = KeyCode.V;

    private AudioSource audioSource;
    private string microphoneDevice;
    private bool isRecording = false;

    
    private int maxClipLengthSeconds = 10;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone detected!");
            return;
        }

        
        microphoneDevice = Microphone.devices[0];
        Debug.Log("Using microphone: " + microphoneDevice);
    }

    void Update()
    {
        
        if (microphoneDevice == null) return;

        

        
        if (Input.GetKeyDown(pushToTalkKey))
        {
            StartRecording();
        }

        
        if (Input.GetKeyUp(pushToTalkKey))
        {
            StopRecordingAndPlay();
        }
    }

    
    public void StartRecording()
    {
        if (isRecording) return;

        isRecording = true;
        Debug.Log("Recording started...");

        
        audioSource.clip = Microphone.Start(microphoneDevice, true, maxClipLengthSeconds, AudioSettings.outputSampleRate);
    }

  
    public void StopRecordingAndPlay()
    {
        if (!isRecording) return; 

        isRecording = false;
        Debug.Log("Recording stopped.");

        
        Microphone.End(microphoneDevice);

        
        Debug.Log("Playing back recorded audio...");
        audioSource.Play();
    }
}