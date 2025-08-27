using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

//FOKOzuynen

public class ScreenCapture : MonoBehaviour
{
	
    [SerializeField] private string fileName = "screenshoot";
    private Texture2D texture;
	[SerializeField] private Camera cameraCapture;
	
	
	[SerializeField] private int m_CaptureWidth = 512;
	[SerializeField] private int m_CaptureHeight = 512;
	[SerializeField] private bool shoot = false;
	
	void Update()
	{
		if(shoot)
		{
			CameraCapture();
			shoot = false;
		}
	}
	
	
    public void CameraCapture() 
	{
		 Texture2D captureWhite = new Texture2D(m_CaptureWidth,  m_CaptureHeight, TextureFormat.RGB24, false);
		Texture2D captureBlack = new Texture2D(m_CaptureWidth,  m_CaptureHeight, TextureFormat.RGB24, false);
		
		texture = new Texture2D(m_CaptureWidth, m_CaptureHeight, TextureFormat.ARGB32, false);
			for (int x = 0; x < m_CaptureWidth; ++x) 
			{
				for (int y = 0; y < m_CaptureHeight; ++y) 
				{
					Color colorBlack = captureBlack.GetPixel(x, y);
					Color colorWhite = captureWhite.GetPixel(x, y);
					Color colorMix = (colorBlack + colorWhite) * 0.5f;
					float alphaR = 1 + colorBlack.r - colorWhite.r;
					float alphaG = 1 + colorBlack.g - colorWhite.g;
					float alphaB = 1 + colorBlack.b - colorWhite.b;
					float alpha = (alphaR + alphaG + alphaB) / 3f;
					Color color = new Color(colorMix.r, colorMix.g, colorMix.b, alpha);
					texture.SetPixel(x, y, color);
				}
			}
	    var scrRenderTexture = new RenderTexture(texture.width, texture.height, 32,RenderTextureFormat.ARGB32 );
		
		cameraCapture.targetTexture = scrRenderTexture;
		RenderTexture.active = scrRenderTexture;
        
         
		 
		 CameraClearFlags preClearFlags = cameraCapture.clearFlags;
		 cameraCapture.clearFlags = CameraClearFlags.Depth ;
		 cameraCapture.Render();
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();
		
        byte[] bytes = texture.EncodeToPNG();
        
		string timestamp = System.DateTime.Now.ToString ("dd_MM_yyyy_HH_mm_ss");

		
		File.WriteAllBytes(Application.dataPath + "/../Assets/CreateIconSprite/ScreenCapture/"+ fileName +"_" + timestamp + ".png", bytes);
		       
        Debug.Log("Capture SHOOT");
	#if UNITY_EDITOR
		AssetDatabase.Refresh();
	#endif	
	}

   }
   
	
