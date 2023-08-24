using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    [SerializeField]private AudioClipRefsSO audioClipRefsSO;

    public static SoundManager Instance;
    [HideInInspector]
    public float sfxVolume;

    private void Awake() {
        sfxVolume = 0.5f;
        Instance = this;
    }

    
    private void Start() {
        OptionsMenuUI.Instance.OnOptionsUpdated += Instance_OnOptionsUpdated;
        DeliveryManager.instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    //public void UpdateSoundLevels(float sfxVolume) { this.sfxVolume = sfxVolume; }

    private void Instance_OnOptionsUpdated(object sender, OptionsMenuUI.OnOptionsUpdatedEventArgs e) {
        sfxVolume = e.sfxVolume;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        BaseCounter activeCounter= sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, activeCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter activeCuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipRefsSO.chop, activeCuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.deliveryFailed, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.deliverySucess, DeliveryCounter.Instance.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)],position,volume);
    }
    private void PlaySound(AudioClip audioClip,Vector3 position,float volume=1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, sfxVolume);
    }


    public void PlayFootStepsSound(Vector3 position,float volume=1f) {
        PlaySound(audioClipRefsSO.footstep, position, sfxVolume);
    }
}
