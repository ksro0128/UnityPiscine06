using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    private bool isOpen = false;
    private Animator animator;
    [SerializeField] private bool isEndDoor = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEndDoor)
        {
            if (!isOpen)
            {
                animator.SetBool("IsOpen", true);
                isOpen = true;
                StartCoroutine(CloseDoor());

            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isEndDoor)
        {
            if (!isOpen)
            {
                animator.SetBool("IsOpen", true);
                isOpen = true;
                StartCoroutine(CloseDoor());

            }
        }
    }

    public void OpenDoor()
    {
        if (isOpen)
            return;
        animator.SetBool("IsOpen", true);
        isOpen = true;
        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(3);
        animator.SetBool("IsOpen", false);
        isOpen = false;
    }   


}
