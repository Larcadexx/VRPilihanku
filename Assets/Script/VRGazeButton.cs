using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.UI; 
public class VRGazeButton : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    public float dwellTime = 2f; 
    
    [Header("Referensi Visual (Opsional)")]
    public Image loadingRing; 

    [Header("Perintah yang Dijalankan")]
    public UnityEvent onClick; 

    private float timer;
    private bool isHovered;

    void Update()
    {
        if (isHovered)
        {
            timer += Time.deltaTime;

            if (loadingRing != null)
            {
                loadingRing.fillAmount = timer / dwellTime;
            }

            if (timer >= dwellTime)
            {
                ExecuteClick();
            }
        }
    }

    public void OnPointerEnter()
    {
        isHovered = true;
    }

    public void OnPointerExit()
    {
        ResetGaze();
    }

    private void ExecuteClick()
    {
        onClick.Invoke(); 
        
        ResetGaze(); 
    }

    private void ResetGaze()
    {
        isHovered = false;
        timer = 0f;
        
        if (loadingRing != null)
        {
            loadingRing.fillAmount = 0f;
        }
    }
}