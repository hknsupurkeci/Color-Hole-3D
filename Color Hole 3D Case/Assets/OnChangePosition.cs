using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider GeneretedMeshCollider;
    public float initialScale = 0.5f;
    public float speed = 0.1f;
    public float minX;
    public float maxX;
    public static float minZ = -37.78f;
    public static float maxZ = -13.95f;
    public static bool gameOver = false;
    private Touch touch;
    Mesh GeneretedMesh;

    private void Start()
    {
        minZ = -37.78f;
        maxZ = -13.95f;
        gameObject.transform.position = new Vector3(0, -0.49f, -36.4f);

        MakeHole2D();
        Make3DMeshCollider();
    }

    private void FixedUpdate()
    {
        #region Reflection
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            //Hole Parent gameobjesinin pozisyonunu karakter değiştikçe 2d pozsiyonuna göre ayarlıyor
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;
            MakeHole2D();
            Make3DMeshCollider();
        }
        #endregion
        //Debug.Log(gameOver);
        if (gameOver) CharacterControl();
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);

        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    private void Make3DMeshCollider()
    {
        if (GeneretedMesh != null) Destroy(GeneretedMesh);
        GeneretedMesh = ground2DCollider.CreateMesh(true, true);
        GeneretedMeshCollider.sharedMesh = GeneretedMesh;
    }

    private void CharacterControl()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                //Eğer hareket var ise miknatis kodu calissin.
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * 0.02f,
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.y * 0.02f);
            }
        }
        //karakter sinirlandirma
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            -0.49f,
            Mathf.Clamp(transform.position.z, minZ, maxZ)
        );
    }
}
