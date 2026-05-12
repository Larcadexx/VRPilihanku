using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Referensi Umum")]
    public Transform mainCamera;
    public AudioSource audioSource;

    [Header("1. Pembukaan")]
    public AudioClip audioPembukaan;
    [Tooltip("Masukkan 4 image pembukaan berurutan ke sini")]
    public GameObject[] imagePembukaan; 

    [Header("2. Instruksi Arah")]
    public GameObject lookKanan;
    public AudioClip audioInstruksiKanan;
    public GameObject lookKiri;
    public AudioClip audioInstruksiKiri;
    public GameObject lookAtas;
    public AudioClip audioInstruksiAtas;

    [Header("3. Instruksi Penggunaan Tombol")]
    public GameObject imageInstruksiButton;
    public GameObject buttonTombol; // Tambahan: Button yang muncul saat instruksi
    public AudioClip audioInstruksiButton;

    [Header("4. Penutup")]
    public AudioClip audioPenutup; 
    public GameObject imagePenutup; 
    public GameObject playButton;
    public GameObject skipButton; 

    private bool isTutorialAktif = false; 
    private bool isKananSelesai = false;
    private bool isKiriSelesai = false;
    private bool isAtasSelesai = false;

    void Start()
    {
        SembunyikanSemuaUI();
        StartCoroutine(SequencePembukaan());
    }

    void Update()
    {
        if (!isTutorialAktif || isAtasSelesai) return;

        float rotasiY = Mathf.DeltaAngle(0, mainCamera.eulerAngles.y);
        float rotasiX = Mathf.DeltaAngle(0, mainCamera.eulerAngles.x);

        if (!isKananSelesai && rotasiY > 75f)
        {
            isKananSelesai = true;
            lookKanan.SetActive(false);
            lookKiri.SetActive(true);
            PutarAudio(audioInstruksiKiri);
        }

        if (isKananSelesai && !isKiriSelesai && rotasiY < -75f)
        {
            isKiriSelesai = true;
            lookKiri.SetActive(false);
            lookAtas.SetActive(true);
            PutarAudio(audioInstruksiAtas);
        }

        if (isKiriSelesai && !isAtasSelesai && rotasiX < -45f)
        {
            isAtasSelesai = true;
            lookAtas.SetActive(false);
            StartCoroutine(SequencePenutup());
        }
    }

    IEnumerator SequencePembukaan()
    {
        PutarAudio(audioPembukaan);

        if (imagePembukaan.Length > 0) imagePembukaan[0].SetActive(true);
        yield return new WaitForSeconds(4f);
        if (imagePembukaan.Length > 0) imagePembukaan[0].SetActive(false);

        if (imagePembukaan.Length > 1) imagePembukaan[1].SetActive(true);
        yield return new WaitForSeconds(7f);
        if (imagePembukaan.Length > 1) imagePembukaan[1].SetActive(false);

        if (imagePembukaan.Length > 2) imagePembukaan[2].SetActive(true);
        yield return new WaitForSeconds(5f);
        if (imagePembukaan.Length > 2) imagePembukaan[2].SetActive(false);

        if (imagePembukaan.Length > 3) imagePembukaan[3].SetActive(true);
        yield return new WaitForSeconds(6f);
        if (imagePembukaan.Length > 3) imagePembukaan[3].SetActive(false);

        // Hide Skip Button saat pembukaan selesai
        skipButton.SetActive(false); 

        lookKanan.SetActive(true);
        PutarAudio(audioInstruksiKanan);
        isTutorialAktif = true; 
    }

    IEnumerator SequencePenutup()
    {
        // Munculkan Image Instruksi DAN Button Tombol secara bersamaan
        imageInstruksiButton.SetActive(true);
        if (buttonTombol != null) buttonTombol.SetActive(true); 
        
        PutarAudio(audioInstruksiButton);
        yield return new WaitForSeconds(10f);
        
        // Sembunyikan keduanya setelah 10 detik
        imageInstruksiButton.SetActive(false);
        if (buttonTombol != null) buttonTombol.SetActive(false);

        // Masuk ke tahap penutup
        PutarAudio(audioPenutup);
        
        if (imagePenutup != null) imagePenutup.SetActive(true);
        yield return new WaitForSeconds(3f); 
        if (imagePenutup != null) imagePenutup.SetActive(false);

        playButton.SetActive(true); 
        // Skip Button tetap hide
    }

    private void PutarAudio(AudioClip klip)
    {
        if (audioSource != null && klip != null)
        {
            audioSource.PlayOneShot(klip); 
        }
    }

    private void SembunyikanSemuaUI()
    {
        playButton.SetActive(false);
        skipButton.SetActive(true); 
        lookKanan.SetActive(false);
        lookKiri.SetActive(false);
        lookAtas.SetActive(false);
        
        if (imageInstruksiButton != null) imageInstruksiButton.SetActive(false);
        if (buttonTombol != null) buttonTombol.SetActive(false); // Pastikan button tombol hide di awal
        if (imagePenutup != null) imagePenutup.SetActive(false);

        foreach (var img in imagePembukaan) { if (img != null) img.SetActive(false); }
    }

    public void PindahKeLevel1()
    {
        SceneTransitionManager.Instance.PindahScene("Level1");
    }

    public void ButtonTombol()
    {
        Debug.Log("Button Tombol berhasil diklik!"); 
    }
}