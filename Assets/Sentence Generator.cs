using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SentenceGenerator : MonoBehaviour
{

    [System.Serializable]
    public struct AtlasParams
    {
        public Vector2 fontSize;
        public bool isColor;
    }

    public Texture FontAtlas;
    [SerializeField]
    public AtlasParams atlasConfig;

    public ComputeShader SentenceProcessor;
    public RenderTexture Result;
    public string sentence;
    private ComputeBuffer indexBuffer;


    void PrimeData()
    {
        var dim = atlasConfig.fontSize;
        Result = new RenderTexture(Mathf.CeilToInt(dim.x * sentence.Length), Mathf.CeilToInt(dim.y), 0,
            GraphicsFormat.R8_UInt);
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
