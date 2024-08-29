using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : MonoBehaviour
{
    [SerializeField] private GameObject[] Ghosts;
    void Start()
    {
        StartCoroutine(ActivateGargoyle());
    }

    IEnumerator ActivateGargoyle()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            int direction = Random.Range(0, 3);
            if (direction == 0)
                StartCoroutine(RotateGargoyle(90, 1));                
            else if (direction == 1)
                StartCoroutine(RotateGargoyle(-90, 1));
            else
                StartCoroutine(RotateGargoyle(180, 1));
        }
    }

    IEnumerator RotateGargoyle(float angle, float duration)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(0, angle, 0);
        float time = 0;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject ghost in Ghosts)
            {
                ghost.GetComponent<Ghost>().CallGhost();
            }
        }
    }
}
