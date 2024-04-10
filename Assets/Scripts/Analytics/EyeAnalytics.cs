using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class EyeAnalytics : MonoBehaviour
{
    [System.Serializable]
    public class ObjectOfInterest
    {
        public string name;
        public float time;
        public string majorityColor;
    }

    private string filePath = "";
    private Dictionary<string, ObjectOfInterest> pointsOfInterest = new Dictionary<string, ObjectOfInterest>();
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + "/eye_tracking.csv";
        Camera cam = Camera.main;
        if (cam.GetComponent<EyeTracking>())
        {
            cam.GetComponent<EyeTracking>().onGazeEnter.AddListener(onGazeEnter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            WriteToCSV();
        }
    }

    public void WriteToCSV()
    {
        if (pointsOfInterest.Count > 0)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))

            {
                // Write the header
                sw.WriteLine("Name;Time;Majority Color;");
                Debug.Log("Saving Eye data...");

                // Write each object's data
                foreach (var point in pointsOfInterest)
                {
                    ObjectOfInterest current = point.Value;
                    string name = current.name.Replace(";", "");
                    sw.WriteLine("{0};{1};{2}", name, current.time, current.majorityColor);

                }
                Debug.Log("File saved: " + filePath);
                sw.Close();
            }
        }
    }

    private void onGazeEnter(GameObject go)
    {
        if (!pointsOfInterest.ContainsKey(go.name))
        {
            string color = "";
            if (go.GetComponent<ObjectMajorityColor>()) { color = go.GetComponent<ObjectMajorityColor>().GetMajorityColorHex(); }
            pointsOfInterest.Add(go.name, new ObjectOfInterest() { name = go.name, time = 0.0f, majorityColor = color });
        } else
        {
            pointsOfInterest[go.name].time += Time.deltaTime;
        }
    }
}
