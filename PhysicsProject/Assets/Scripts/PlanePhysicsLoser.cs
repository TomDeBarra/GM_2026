using UnityEngine;
using TMPro;

public class PlanePhysicsLoser : PlanePhysics
{
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI winText;

    public float timeToWin = 10f;

    private float timer = 0f;
    private bool gameOver = false;

    void Start()
    {
        if (loseText != null)
            loseText.gameObject.SetActive(false);

        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
            return;

        foreach (SpherePhysics sphere in FindObjectsByType<SpherePhysics>(FindObjectsSortMode.None))
        {
            Collider planeCollider = GetComponent<Collider>();
            Collider sphereCollider = sphere.GetComponent<Collider>();

            if (planeCollider != null && sphereCollider != null &&
                planeCollider.bounds.Intersects(sphereCollider.bounds))
            {
                gameOver = true;

                if (loseText != null)
                {
                    loseText.text = "You Lost";
                    loseText.color = Color.red;
                    loseText.gameObject.SetActive(true);
                }

                return;
            }
        }

        timer += Time.deltaTime;

        if (timer >= timeToWin)
        {
            gameOver = true;

            if (winText != null)
            {
                winText.text = "You Win!";
                winText.color = Color.green;
                winText.gameObject.SetActive(true);
            }
        }
    }
}