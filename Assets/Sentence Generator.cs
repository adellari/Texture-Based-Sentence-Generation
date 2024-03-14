using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    public struct ComputeParams
    {
        public uint width;
        public uint height;
        public uint depth;
    }

    public struct FontKey
    {
        public int placement;
        public int id;
    }

    public Texture FontAtlas;
    [SerializeField]
    public AtlasParams atlasConfig;
    private ComputeParams cParams;
    
    public ComputeShader SentenceProcessor;
    public RenderTexture Result;
    public string sentence;
    
    //This will contain an integer pair [i, o]
    //where i is the index of the letter in the atlas
    //and o is its order in the sentence
    private ComputeBuffer indexBuffer;

    
    //for this it might be helpful to create a dictionary for lookup
    // to understand where letters map on the atlas

    
    //
    void ParseSentence()
    {
        var len = sentence.Length;
        FontKey[] keyArr = new FontKey[sentence.Length];
        indexBuffer = new ComputeBuffer(len, Marshal.SizeOf(typeof(FontKey)));
        
        for (int a = 0; a < sentence.Length; a++)
        {
            keyArr[a].placement = a;
            keyArr[a].id = a >> 13; //need to actually replace this with the id of the letter key
        }
        
        indexBuffer.SetData(keyArr);
    }
    
    void PrimeData()
    {
        var dim = atlasConfig.fontSize;
        var len = sentence.Length;
        
        cParams = new ComputeParams();
        cParams.width = (uint)Mathf.CeilToInt(dim.x);
        cParams.height = (uint)Mathf.CeilToInt(dim.y);
        cParams.depth = (uint)len;
        
        Result = new RenderTexture(Mathf.CeilToInt(dim.x * len), Mathf.CeilToInt(dim.y), 0,
            GraphicsFormat.R8_UInt);
        
        
    }

    void clearPersistants()
    {
        if (Result)
            Result.Release();

        if (indexBuffer != null)
            indexBuffer.Dispose();

    }
    
    void Start()
    {
        clearPersistants();
    }

    
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        clearPersistants();
    }
    
    
}
