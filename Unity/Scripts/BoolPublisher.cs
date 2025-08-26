using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

/// Publishes a std_msgs/Bool message once when triggered.
public class BoolPublisher : MonoBehaviour
{
    [Header("ROS Topic Settings")]
    [SerializeField] private ROSConnection ros;
    [SerializeField] private string captureStateTopic = "/droid/droid_user_gui/capture_button";
    [SerializeField] private string finishStateTopic = "/droid/droid_user_gui/finish_button";

    [Header("Task Info (set from GUI)")]
    public int currentIndex = 0;
    public int lastIndex = 0; // Will be set by GUI when task list is known

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<BoolMsg>(captureStateTopic);
        ros.RegisterPublisher<BoolMsg>(finishStateTopic);
    }

    /// Send a Bool message with value 'true'.
    public void PublishTrue()
    {
        string boolTopic = (currentIndex == lastIndex) ? finishStateTopic : captureStateTopic;
        ros.Publish(boolTopic, new BoolMsg(true));
        Debug.Log($"Published TRUE to {boolTopic}");
    }

}
