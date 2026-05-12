using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 
using TMPro; 

public class LevelOneManager : MonoBehaviour
{
    private enum CharState { Walking, Turning, Idle, Sitting }

    [Header("======================================")]
    [Header("1. PENGATURAN KARAKTER (BERJALAN)")]
    [Header("======================================")]
    [Space(5)]
    
    public Transform mahasiswaTransform;
    public Animator mahasiswaAnimator;
    public Vector3 mahasiswaTargetPos;
    public Vector3 mahasiswaTargetRot;
    public float mahasiswaWalkSpeed = 1.5f;
    private CharState stateMahasiswa = CharState.Walking;

    [Space(10)]
    public Transform bumilTransform;
    public Animator bumilAnimator;
    public Vector3 bumilTargetPos;
    public Vector3 bumilTargetRot;
    public float bumilWalkSpeed = 1.0f;
    private CharState stateBumil = CharState.Walking;
    
    [Space(5)]
    public float turnSpeed = 150f;

    [Space(20)]
    [Header("======================================")]
    [Header("2. NARASI CERITA AWAL")]
    [Header("======================================")]
    [Space(5)]
    public float jedaAntarDialog = 0.5f;

    public AudioClip audioBumil1;
    [TextArea] public string teksBumil1; 
    public AudioClip audioMahasiswa1;
    [TextArea] public string teksMahasiswa1; 
    public AudioClip audioMahasiswa2;
    [TextArea] public string teksMahasiswa2; 
    public AudioClip audioBumil2;
    [TextArea] public string teksBumil2; 
    public AudioClip audioBumil3;
    [TextArea] public string teksBumil3; 

    [Space(20)]
    [Header("======================================")]
    [Header("3. PENGATURAN ADEGAN DUDUK & KAMERA")]
    [Header("======================================")]
    [Space(5)]

    public Transform cameraRig;
    public Vector3 cameraSitPos; 
    public Vector3 cameraSitRot; 

    [Space(10)]
    [Header("- Kondisi Opsi Bumil")]
    public Vector3 bumilSitPos; 
    public Vector3 bumilSitRot; 
    public AudioClip audioBumil4;
    [TextArea] public string teksBumil4;
    public AudioClip audioBumil5;
    [TextArea] public string teksBumil5;

    [Space(10)]
    [Header("- Kondisi Opsi Mahasiswa")]
    public Vector3 mahasiswaSitPos; 
    public Vector3 mahasiswaSitRot; 
    public AudioClip audioMahasiswa3;
    [TextArea] public string teksMahasiswa3;

    [Space(20)]
    [Header("======================================")]
    [Header("4. UI, TOMBOL, DAN FEEDBACK")]
    [Header("======================================")]
    [Space(5)]

    public AudioSource audioSource; 
    public TextMeshProUGUI subtitleText; 
    public GameObject subtitlePanel; 

    [Space(10)]
    public GameObject panelInstruksi; 
    public AudioClip audioInstruksi;  
    
    [Space(10)]
    public GameObject btnOpsiBumil;
    public GameObject btnOpsiMahasiswa;
    public float jedaTungguInput = 5f; 
    public float jedaTungguInputKedua = 8f;
    private bool isOpsiDipilih = false; 

    [Space(10)]
    [Header("- Audio Penutup & Feedback")]
    public AudioClip audioLevel1Selesai; 
    [Space(5)]
    public GameObject panelFeedbackBumil;
    public AudioClip audioFeedbackBumil;
    [Space(5)]
    public GameObject panelFeedbackMahasiswa;
    public AudioClip audioFeedbackMahasiswa;

    [Space(10)]
    [Header("- Navigasi")]
    public GameObject btnNextLevel;
    public AudioClip audioInstruksiNextLevel; 


    void Start()
    {
        if (mahasiswaAnimator != null) mahasiswaAnimator.SetBool("isWalking", true);
        if (bumilAnimator != null) bumilAnimator.SetBool("isWalking", true);

        btnOpsiBumil.SetActive(false);
        btnOpsiMahasiswa.SetActive(false);
        if (btnNextLevel != null) btnNextLevel.SetActive(false);
        
        if (panelInstruksi != null) panelInstruksi.SetActive(false);
        if (panelFeedbackBumil != null) panelFeedbackBumil.SetActive(false);
        if (panelFeedbackMahasiswa != null) panelFeedbackMahasiswa.SetActive(false);
        
        TutupDialog();
        StartCoroutine(UrutanCerita());
    }

    void Update()
    {
        UpdateGerakan(mahasiswaTransform, mahasiswaAnimator, mahasiswaTargetPos, mahasiswaTargetRot, mahasiswaWalkSpeed, ref stateMahasiswa);
        UpdateGerakan(bumilTransform, bumilAnimator, bumilTargetPos, bumilTargetRot, bumilWalkSpeed, ref stateBumil);
    }

    private void UpdateGerakan(Transform karakter, Animator anim, Vector3 targetPos, Vector3 targetRotEuler, float speed, ref CharState currentState)
    {
        if (karakter == null || currentState == CharState.Sitting) return;

        if (currentState == CharState.Walking)
        {
            karakter.position = Vector3.MoveTowards(karakter.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(karakter.position, targetPos) < 0.05f)
            {
                currentState = CharState.Turning;
                if (anim != null) anim.SetBool("isWalking", false);
            }
        }
        else if (currentState == CharState.Turning)
        {
            Quaternion targetRot = Quaternion.Euler(targetRotEuler);
            karakter.rotation = Quaternion.RotateTowards(karakter.rotation, targetRot, turnSpeed * Time.deltaTime);
            if (Quaternion.Angle(karakter.rotation, targetRot) < 0.1f)
            {
                currentState = CharState.Idle;
            }
        }
    }

    IEnumerator UrutanCerita()
    {
        yield return new WaitUntil(() => stateMahasiswa == CharState.Idle && stateBumil == CharState.Idle);
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(PlayDialogWait(audioBumil1, teksBumil1));
        yield return StartCoroutine(PlayDialogWait(audioMahasiswa1, teksMahasiswa1));

        TutupDialog();

        btnOpsiBumil.SetActive(true);
        btnOpsiMahasiswa.SetActive(true);
        if (panelInstruksi != null) panelInstruksi.SetActive(true);
        
        float durasiInstruksi = 0f;
        if (audioSource != null && audioInstruksi != null)
        {
            audioSource.PlayOneShot(audioInstruksi); 
            durasiInstruksi = audioInstruksi.length; 
        }

        float timer = 0f;
        while (timer < (durasiInstruksi + jedaTungguInput) && !isOpsiDipilih)
        {
            timer += Time.deltaTime; 
            yield return null; 
        }

        if (!isOpsiDipilih)
        {
            yield return StartCoroutine(PlayDialogWait(audioMahasiswa2, teksMahasiswa2));
            yield return StartCoroutine(PlayDialogWait(audioBumil2, teksBumil2));
            TutupDialog();

            timer = 0f; 
            while (timer < jedaTungguInputKedua && !isOpsiDipilih)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (!isOpsiDipilih)
            {
                yield return StartCoroutine(PlayDialogWait(audioBumil3, teksBumil3));
                TutupDialog();
            }
        }
    }

    IEnumerator PlayDialogWait(AudioClip clip, string text)
    {
        if (clip == null) yield break;
        MainkanDialog(clip, text);
        yield return new WaitForSeconds(clip.length);
        yield return new WaitForSeconds(jedaAntarDialog); 
    }

    IEnumerator SequenceDudukBumil()
    {
        stateBumil = CharState.Sitting;
        if (bumilTransform != null)
        {
            bumilTransform.position = bumilSitPos;
            bumilTransform.rotation = Quaternion.Euler(bumilSitRot);
        }
        if (cameraRig != null)
        {
            cameraRig.position = cameraSitPos;
            cameraRig.rotation = Quaternion.Euler(cameraSitRot);
        }
        if (bumilAnimator != null) bumilAnimator.SetBool("isSitting", true);

        yield return StartCoroutine(PlayDialogWait(audioBumil4, teksBumil4));
        yield return StartCoroutine(PlayDialogWait(audioBumil5, teksBumil5));

        TutupDialog(); 
        if (audioLevel1Selesai != null)
        {
            audioSource.PlayOneShot(audioLevel1Selesai);
            yield return new WaitForSeconds(audioLevel1Selesai.length);
        }

        if (panelFeedbackBumil != null) panelFeedbackBumil.SetActive(true);
        
        if (audioFeedbackBumil != null)
        {
            audioSource.PlayOneShot(audioFeedbackBumil);
            yield return new WaitForSeconds(audioFeedbackBumil.length);
        }

        yield return new WaitForSeconds(1.5f);

        if (btnNextLevel != null) btnNextLevel.SetActive(true);
        if (audioInstruksiNextLevel != null)
        {
            audioSource.PlayOneShot(audioInstruksiNextLevel);
        }
    }

    IEnumerator SequenceDudukMahasiswa()
    {
        stateMahasiswa = CharState.Sitting;
        if (mahasiswaTransform != null)
        {
            mahasiswaTransform.position = mahasiswaSitPos;
            mahasiswaTransform.rotation = Quaternion.Euler(mahasiswaSitRot);
        }
        if (cameraRig != null)
        {
            cameraRig.position = cameraSitPos;
            cameraRig.rotation = Quaternion.Euler(cameraSitRot);
        }
        if (mahasiswaAnimator != null) mahasiswaAnimator.SetBool("isSitting", true);

        yield return StartCoroutine(PlayDialogWait(audioMahasiswa3, teksMahasiswa3));

        TutupDialog(); 

        if (audioLevel1Selesai != null)
        {
            audioSource.PlayOneShot(audioLevel1Selesai);
            yield return new WaitForSeconds(audioLevel1Selesai.length);
        }

        if (panelFeedbackMahasiswa != null) panelFeedbackMahasiswa.SetActive(true);
        
        if (audioFeedbackMahasiswa != null)
        {
            audioSource.PlayOneShot(audioFeedbackMahasiswa);
            yield return new WaitForSeconds(audioFeedbackMahasiswa.length);
        }

        yield return new WaitForSeconds(1.5f);

        if (btnNextLevel != null) btnNextLevel.SetActive(true);
        if (audioInstruksiNextLevel != null)
        {
            audioSource.PlayOneShot(audioInstruksiNextLevel);
        }
    }

    void MainkanDialog(AudioClip klip, string teks)
    {
        if (subtitlePanel != null) subtitlePanel.SetActive(true);
        if (subtitleText != null) subtitleText.text = teks; 
        if (audioSource != null && klip != null)
        {
            audioSource.clip = klip;
            audioSource.Play(); 
        }
    }

    void TutupDialog()
    {
        if (subtitlePanel != null) subtitlePanel.SetActive(false);
        if (subtitleText != null) subtitleText.text = "";
        if (audioSource != null) audioSource.Stop();
    }

    public void PilihOpsiBumil()
    {
        if (isOpsiDipilih) return;
        isOpsiDipilih = true;
        StopAllCoroutines(); 
        TutupDialog(); 
        btnOpsiBumil.SetActive(false);
        btnOpsiMahasiswa.SetActive(false);
        if (panelInstruksi != null) panelInstruksi.SetActive(false); 
        StartCoroutine(SequenceDudukBumil());
    }

    public void PilihOpsiMahasiswa()
    {
        if (isOpsiDipilih) return;
        isOpsiDipilih = true;
        StopAllCoroutines(); 
        TutupDialog();
        btnOpsiBumil.SetActive(false);
        btnOpsiMahasiswa.SetActive(false);
        if (panelInstruksi != null) panelInstruksi.SetActive(false); 
        StartCoroutine(SequenceDudukMahasiswa());
    }

    public void PindahKeLevel2()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.PindahScene("Level2");
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager nggak ketemu! Langsung load scene biasa.");
            SceneManager.LoadScene("Level2");
        }
    }
}