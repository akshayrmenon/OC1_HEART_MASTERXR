using System.Collections;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using DataFormat = RosMessageTypes.Sensor.CompressedImageMsg;

 /// <summary> Responsible for subscribing to the Head Camera: RealSense RGBB (640(w)x480(h)) from ROS-TCP
 /// in sensor_msgs/CompressedImage message type. Exposes a Texture2D via the GetTexture function.</summary>

public class CameraImageSubscriberTCP : MonoBehaviour
{
    [Tooltip("Specify Render Source")]
    public MeshRenderer mainViewRenderer;   // Big view plane
    public MeshRenderer sideViewRenderer;   // Small view plane

    private Texture2D texture2D;
    private byte[] imageData;         

    /// <summary> ROS Connection and Topic</summary>
    [SerializeField] private ROSConnection ros;
    [SerializeField] private string rosTopic = "/rs_camera/color/image_raw/compressed";

    public bool renderToMain;

    void Start()
    {
       
        texture2D = new Texture2D(1, 1);
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<DataFormat>(rosTopic, callback);
        
    }
    private void callback(DataFormat receivedImage)
    {
        texture2D.LoadImage(receivedImage.data);
        texture2D.Apply();

        if (renderToMain && mainViewRenderer != null)
        {
            mainViewRenderer.material.SetTexture("_MainTex", texture2D);
        }
        else if (!renderToMain && sideViewRenderer != null)
        {
            sideViewRenderer.material.SetTexture("_MainTex", texture2D);
        }
    }

    // GUI button to flip where the feed goes
    public void ToggleRenderTarget()
    {
        renderToMain = !renderToMain;
    }


}


