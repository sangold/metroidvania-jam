using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Camera MirrorCamera;
    public RenderTexture RenderTexture;
    public SpriteRenderer Sr;
    public enum MirrorType { FRONT, SIDE, WORLD }
    [SerializeField]
    private MirrorType _mirrorType;

    private void Start()
    {
        MirrorCamera.targetTexture = Instantiate(RenderTexture);
        Sr.material.SetTexture("_BaseMap", MirrorCamera.targetTexture);
    }
}
