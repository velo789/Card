using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager1 : MonoBehaviour
{
    private static InputManager1 instance;
    public static InputManager1 Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectCard();
        }
    }

    private void TrySelectCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            ParticleManager.Instance.PlayClickEffect(hitInfo.point);

            Card clickedCard = hitInfo.transform.GetComponent<Card>();

            if (clickedCard != null)
            {
                GameManager1.Instance.ProcessCardClick(clickedCard);
            }
        }
    }
}
