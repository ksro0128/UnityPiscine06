using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleHandler : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask obstacleLayer;
    private bool isFPS = false;

    private List<Renderer> previousObstacles = new List<Renderer>();

    void LateUpdate()
    {
        if (isFPS)
            return;
        Vector3 direction = playerTransform.position - cameraTransform.position;
        RaycastHit[] hits = Physics.RaycastAll(cameraTransform.position, direction.normalized, direction.magnitude, obstacleLayer);

        List<Renderer> currentObstacles = new List<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                currentObstacles.Add(renderer);
                SetTransparency(renderer, 0.3f);
            }
        }

        foreach (Renderer renderer in previousObstacles)
        {
            if (!currentObstacles.Contains(renderer))
            {
                SetTransparency(renderer, 1f);
            }
        }

        previousObstacles = currentObstacles;
    }

     void SetTransparency(Renderer renderer, float alpha)
    {
        foreach (Material material in renderer.materials)
        {
            Color color = material.color;
            color.a = alpha;
            material.color = color;

            if (alpha < 1f)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHABLEND_ON");
                material.renderQueue = -1;
            }
        }
    }

    public void SwitchToFPS()
    {
        isFPS = true;
        foreach (Renderer renderer in previousObstacles)
        {
            SetTransparency(renderer, 1f);
        }
    }

    public void SwitchToTPS()
    {
        isFPS = false;
    }


}
