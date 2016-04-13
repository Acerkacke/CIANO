using UnityEngine;
using System.Collections;

public class RandomCameraColor : MonoBehaviour,ISlowestUpdate{

	public int maxColor = 600;
	public int minColor = 100;

	private Camera cam;

	void Start(){
		cam = GetComponent<Camera>();
		if(maxColor > 255*3){
			maxColor = 255*3;
		}
		if(minColor < 15){
			minColor = 15;
		}
		if(minColor > maxColor || maxColor < minColor){
			int temp = maxColor;
			maxColor = minColor;
			minColor = temp;
		}
	}

	int tries = 0;

	public void SlowestUpdate(){
		int r = 0,g=0,b=0;
		while((r+g+b < minColor || r+g+b > maxColor) && tries < 10){
			tries ++;
			r = Random.Range(5,255+1);
			g = Random.Range(5,255+1);
			b = Random.Range(5,255+1);
		}
		tries = 0;

		float fr = (float)r/255;
		float fg = (float)g/255;
		float fb = (float)b/255;
		Color bgColor = new Color(fr,fg,fb);
		cam.backgroundColor = bgColor;
	}

}
