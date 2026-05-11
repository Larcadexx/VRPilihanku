using UnityEngine;

public class AnimasiLevel1 : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    public Vector3 targetPosition; // Titik tujuan berjalan (X, Y, Z)
    public float walkSpeed = 1.5f; // Kecepatan berjalan
    
    [Header("Pengaturan Rotasi")]
    public Vector3 targetRotationEuler; // Sudut putar setelah berhenti, misal (0, -90, 0)
    public float turnSpeed = 150f; // Kecepatan berputar

    private Animator animator;
    
    // Status karakter saat ini
    private enum CharState { Walking, Turning, Idle }
    private CharState currentState = CharState.Walking;

    void Start()
    {
        // Mengambil komponen Animator yang ada pada char_mahasiswa
        animator = GetComponent<Animator>();
        
        // Memulai animasi jalan (pastikan Anda sudah membuat parameter "isWalking" di Animator)
        if(animator != null) 
        {
            animator.SetBool("isWalking", true);
        }
    }

    void Update()
    {
        // LOGIKA 1: BERJALAN KEARAH TARGET
        if (currentState == CharState.Walking)
        {
            // Bergerak menuju targetPosition
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

            // Cek apakah sudah sampai di tujuan (jarak sangat dekat)
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                currentState = CharState.Turning; // Pindah ke status berputar
                
                if(animator != null) 
                {
                    animator.SetBool("isWalking", false); // Matikan jalan, memicu transisi ke Idle
                }
            }
        }
        // LOGIKA 2: BERHENTI & MENGHADAP KIRI
        else if (currentState == CharState.Turning)
        {
            // Ubah nilai XYZ rotasi menjadi format Quaternion
            Quaternion targetRot = Quaternion.Euler(targetRotationEuler);
            
            // Berputar secara perlahan menuju target rotasi
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);

            // Cek apakah karakter sudah selesai berputar (menghadap arah yang benar)
            if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)
            {
                currentState = CharState.Idle; // Selesai, masuk ke status Idle penuh
            }
        }
        // LOGIKA 3: IDLE
        else if (currentState == CharState.Idle)
        {
            // Karakter diam di tempat, biarkan animasi Idle berjalan secara loop
        }
    }
}