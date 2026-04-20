using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class VRSetup : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartXR());
    }

    public IEnumerator StartXR()
    {
        // Inisialisasi XR Loader (Cardboard)
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Gagal memuat Cardboard XR Loader!");
        }
        else
        {
            // Mulai XR
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
}