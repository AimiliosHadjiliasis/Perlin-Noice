
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    //We have already define the size of our scene in Unity so this 2 variables 
    //are used to define the resolution of our texture (how many pixels are going to be inside that area)
    public int width = 256;
    public int height = 256;

    //Since we are too close to the camera we need a scale variable
    //so we create a variable to scale what we see 
    //and use that variable in the CalculateColour funciton
    //to multiply the float x,y coordinates
    public float scale = 20f;

    //Add these variables to be able to pan around our noise map
    //and add it in the x/y cordinates in our CalculateColor function
    public float offsetX = 100f;
    public float offsetY = 100f;

    public float speed = 15f;

    void Start()
    {
        //Give random value to our offsets so each time we run the game we get a new map
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }

    void Update()
    {
        //Get reference to our current renderer
        //Thats because in order to change the texture on a material 
        //1) We need to access the mesh renderer component
        //2) Then access the material
        //3) And then change the texture
        Renderer renderer =  GetComponent<Renderer>(); //store the renderer in a variable
        renderer.material.mainTexture = GenerateTexture(); //change our texture

        //Control zoom in and zoom out in the map
        if (Input.GetKey("x"))
            scale = scale + 2;
        if (Input.GetKey("z"))
            scale = scale - 2;

        //Control movement in the map
        if (Input.GetKey("a"))
            offsetX = offsetX - 0.2f;
        if (Input.GetKey("d"))
            offsetX = offsetX + 0.2f;
        if (Input.GetKey("w"))
            offsetY = offsetY + 0.2f;
        if (Input.GetKey("s"))
            offsetY = offsetY - 0.2f;
    }

    //Since we need to return the texture we set the type of the function to Texture2D
    Texture2D GenerateTexture()
    {
        //Since we need to generate a texture, first we create a variable to hold that texture
        Texture2D texture = new Texture2D(width, height);

        //Generate Perlin noise map for the texture:
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x,y);
                //Set each pixel that we are currently on, equal to what perlin noise defines
                texture.SetPixel(x, y, color); // set color to the current pixel
            }
        }

        //When we change the data of texture we need to apply that color to the data
        texture.Apply();

        //Return the color of the texture:
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        //Since we get whole numbers we create 2 floats to store x and y
        //(1)at first we divide them hy height and width so the smaller the x and y
        //so the closer we get to 0, the higer the closer we get to 1
        //(2)Then we multiply by scale
        //meaning that its going to scale our coordinade number up or down 
        //and since we are multiplying by x20 means that our coordinates will be bigger
        //therefore it will cramp more of our perlin noise in our texture which will give the 
        //effect of zooming out
        //(Further explanation in the initialisation of scale variable)
        //(3) Finally add the offsets so we can pan around our noise map
        //And then we pass them to the perlin noise function
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        //Get the value of our PerlinFunction at a certain point x,y coordinate
        //then save it a variable
        float sample =  Mathf.PerlinNoise(xCoord, yCoord);

        //Create and return colour where RGB is equal to sample
        //Means if is equal to 0 we get black color, if its equal to 1 we get white color
        //and if its someware between we get a geay color
        return new Color(sample, sample, sample);
    }
}
