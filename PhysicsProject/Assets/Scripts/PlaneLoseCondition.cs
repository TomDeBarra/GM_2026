using UnityEngine;
using UnityEngine.UI; // Use "using TMPro;" instead if you are using TextMeshPro
using TMPro; // Use "using TMPro;" instead if you are using TextMeshPro
public class PlaneLoseCondition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide the text when the game starts
        if (loseText != null)
        {
            loseText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("UI Reference")]
    public TextMeshProUGUI loseText; // Drag your UI Text component here in the Inspector
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object touching the plane is a sphere
        // (Either by checking its primitive name or a component like SpherePhysics)
        if (collision.gameObject.name.Contains("Sphere") || collision.gameObject.GetComponent<SpherePhysics>() != null)
        {
            TriggerLoss();
        }
    }
    void TriggerLoss()
    {
        if (loseText != null)
        {
            loseText.text = "You Lost";
            loseText.color = Color.red;
            loseText.gameObject.SetActive(true); // Make sure it's visible
            Debug.Log("Sphere touched the plane! You Lost.");
        }
        else
        {
            Debug.LogWarning("Lose Text reference is missing on the Plane script!");
        }
    }
}
