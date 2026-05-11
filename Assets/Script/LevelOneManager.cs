using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 
using TMPro; 

public class LevelOneManager : MonoBehaviour
{
    // Status karakter
    private enum CharState { Walking, Turning, Idle }

    [Header("--- KARAKTER MAHASISWA ---")]
    public Transform mahasiswaTransform;
    public Animator mahasiswaAnimator;
    public Vector3 mahasiswaTargetPos;
    public Vector3 mahasiswaTargetRot;
    public float mahasiswaWalkSpeed = 1.5f;
    private CharState stateMahasiswa = CharState.Walking;

    [Header("--- KARAKTER IBU HAMIL ---")]
    public Transform bumilTransform;
    public Animator bumilAnimator;
    public Vector3 bumilTargetPos;
    public Vector3 bumilTargetRot;
    public float bumilWalkSpeed = 1.0f;
    private CharState stateBumil = CharState.Walking;

    [Header("--- PENGATURAN UMUM GERAKAN ---")]
    public float turnSpeed = 150f;

    [Header("--- PENGATURAN UI & AUDIO ---")]
    public AudioSource audioSource; 
    public TextMeshProUGUI subtitleText; 
    public GameObject subtitlePanel; 

    [Header("--- TOMBOL OPSI ---")]
    public GameObject btnOpsiBumil;
    public GameObject btnOpsiMahasiswa;
    private bool isOpsiDipilih = false; 

    [Header("--- KLIP AUDIO ---")]
    public AudioClip audioBumil1;
    public AudioClip audioMahasiswa1;
    public AudioClip audioMahasiswa2;
    public AudioClip audioBumil2;
    public AudioClip audioBumil3;

    [Header("--- TEKS SUBTITLE ---")]
    [TextArea] public string teksBumil1 = "Teks Bumil 1 (5 Detik)"; // Tadi 4
    [TextArea] public string teksMahasiswa1 = "Teks Mahasiswa 1 (6 Detik)"; // Tadi 5
    [TextArea] public string teksMahasiswa2 = "Teks Mahasiswa 2 (4 Detik)"; // Tadi 3
    [TextArea] public string teksBumil2 = "Teks Bumil 2 (4 Detik)"; // Tadi 3
    [TextArea] public string teksBumil3 = "Teks Bumil 3 (8 Detik)"; // Tadi 7

    void Start()
    {
        if (mahasiswaAnimator != null) mahasiswaAnimator.SetBool("isWalking", true);
        if (bumilAnimator != null) bumilAnimator.SetBool("isWalking", true);

        btnOpsiBumil.SetActive(false);
        btnOpsiMahasiswa.SetActive(false);
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
        if (karakter == null) return;

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
        // Tunggu animasi jalan dan muter selesai
        yield return new WaitUntil(() => stateMahasiswa == CharState.Idle && stateBumil == CharState.Idle);
        yield return new WaitForSeconds(1f);

        // MUNCUL BUMIL 1 (Kini 5 Detik)
        MainkanDialog(audioBumil1, teksBumil1);
        yield return new WaitForSeconds(5f);

        // MUNCUL MAHASISWA 1 (Kini 6 Detik)
        MainkanDialog(audioMahasiswa1, teksMahasiswa1);
        yield return new WaitForSeconds(6f);

        TutupDialog();

        btnOpsiBumil.SetActive(true);
        btnOpsiMahasiswa.SetActive(true);

        // Tunggu 5 detik untuk cek tombol ditekan atau tidak
        float timer = 0f;
        while (timer < 5f && !isOpsiDipilih)
        {
            timer += Time.deltaTime; 
            yield return null; 
        }

        if (!isOpsiDipilih)
        {
            // MUNCUL MAHASISWA 2 (Kini 4 Detik)
            MainkanDialog(audioMahasiswa2, teksMahasiswa2);
            yield return new WaitForSeconds(4f);

            // MUNCUL BUMIL 2 (Kini 4 Detik)
            MainkanDialog(audioBumil2, teksBumil2);
            yield return new WaitForSeconds(4f);

            TutupDialog();

            // Tunggu 8 detik untuk cek tombol
            timer = 0f; 
            while (timer < 8f && !isOpsiDipilih)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (!isOpsiDipilih)
            {
                // MUNCUL BUMIL 3 (Kini 8 Detik)
                MainkanDialog(audioBumil3, teksBumil3);
                yield return new WaitForSeconds(8f);
                
                TutupDialog();
            }
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
        isOpsiDipilih = true;
        TutupDialog(); 
        btnOpsiBumil.SetActive(false);
        btnOpsiMahasiswa.SetActive(false);
        Debug.Log("Pemain memilih Bumil");
    }

    public void PilihOpsiMahasiswa()
    {
        isOpsiDipilih = true;
        TutupDialog();
        btnOpsiBumil.SetActive(false);
        btnOpsiMahasiswa.SetActive(false);
        Debug.Log("Pemain memilih Mahasiswa");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}