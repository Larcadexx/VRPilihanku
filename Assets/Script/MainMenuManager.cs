using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Referensi Objek UI")]
    public GameObject playButton;
    public GameObject skipButton; 
    public GameObject lookKanan;
    public GameObject lookKiri;
    public GameObject lookAtas;

    [Header("Referensi Kamera VR")]
    public Transform mainCamera;

    [Header("Referensi Audio")]
    public AudioSource audioSource;
    public AudioClip audioInstruksiKanan;
    public AudioClip audioInstruksiKiri;
    public AudioClip audioInstruksiAtas;
    public AudioClip audioSelesai; 

    private bool isKananSelesai = false;
    private bool isKiriSelesai = false;
    private bool isAtasSelesai = false;

    void Start()
    {
        playButton.SetActive(false);
        lookKanan.SetActive(true);
        lookKiri.SetActive(false);
        lookAtas.SetActive(false);

        PutarAudio(audioInstruksiKanan);
    }

    void Update()
    {
        if (isAtasSelesai) return;

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
            
            playButton.SetActive(true); 
            skipButton.SetActive(false); 

            PutarAudio(audioSelesai);
        }
    }

    private void PutarAudio(AudioClip klip)
    {
        if (audioSource != null && klip != null)
        {
            audioSource.PlayOneShot(klip); 
        }
    }

    public void PindahKeLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}