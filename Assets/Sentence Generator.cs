using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SentenceGenerator : MonoBehaviour
{

    [System.Serializable]
    public struct AtlasParams
    {
        public Vector2 fontSize;
        public bool isColor;
        public int columns;
        public int rows;
        public int characterCount;
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

    private Dictionary<int, string> atlasDict = new Dictionary<int, string>()
    {
        {0, " "}, {1, "!"}, {2, "\""}, {3, "#"}, {4, "$"}, {5, "%"}, {6, "&"}, {7, "'"}, {8, "("}, {9, ")"},
        {10, "*"}, {11, "+"}, {12, ","}, {13, "-"}, {14, "."}, {15, "/"}, {16, "0"}, {17, "1"}, {18, "2"}, {19, "3"},
        {20, "4"}, {21, "5"}, {22, "6"}, {23, "7"}, {24, "8"}, {25, "9"}, {26, ":"}, {27, ";"}, {28, "<"}, {29, "="},
        {30, ">"}, {31, "?"}, {32, "@"}, {33, "A"}, {34, "B"}, {35, "C"}, {36, "D"}, {37, "E"}, {38, "F"}, {39, "G"},
        {40, "H"}, {41, "I"}, {42, "J"}, {43, "K"}, {44, "L"}, {45, "M"}, {46, "N"}, {47, "O"}, {48, "P"}, {49, "Q"},
        {50, "R"}, {51, "S"}, {52, "T"}, {53, "U"}, {54, "V"}, {55, "W"}, {56, "X"}, {57, "Y"}, {58, "Z"}, {59, "["},
        {60, "\\"}, {61, "]"}, {62, "^"}, {63, "_"}, {64, "`"}, {65, "a"}, {66, "b"}, {67, "c"}, {68, "d"}, {69, "e"}, 
        {70, "f"}, {71, "g"}, {72, "h"}, {73, "i"}, {74, "j"}, {75, "k"}, {76, "l"}, {77, "m"}, {78, "n"}, {79, "o"}, 
        {80, "p"}, {81, "q"}, {82, "r"}, {83, "s"}, {84, "t"}, {85, "u"}, {86, "v"}, {87, "w"}, {88, "x"}, {89, "y"}, 
        {90, "z"}, {91, "{"}, {92, "|"}, {93, "}"}, {94, "~"}, {95, "  "}
    };
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
            var key = atlasDict.FirstOrDefault(b => b.Value == sentence[a].ToString()).Key;
            keyArr[a].id = key; //need to actually replace this with the id of the letter key
            
            Debug.Log(sentence[a] + ": " + key);
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

    void PrimeAtlas()
    {
        if (!FontAtlas)
            return;

        atlasConfig.characterCount = atlasConfig.rows * atlasConfig.columns;
        var w = FontAtlas.width;
        var h = FontAtlas.height;

        atlasConfig.fontSize = new Vector2(w / atlasConfig.columns, h / atlasConfig.rows);
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
        PrimeAtlas();
        ParseSentence();
    }

    
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        clearPersistants();
    }
    
    
}
