using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Timeline;

public class ShadowMode : MonoBehaviour
{
    
    public enum Mode { Mode_SunShadow = 1, Mode_LandMarkVis, Mode_HeatmapShadow, Mode_CFH, Mode_EditMode, Mode_SkyExposure };
    public Mode SelectedMode = Mode.Mode_SunShadow;
    public enum SkyView { Original, Calculated, Top };
    public SkyView ViewMode = SkyView.Original;
    public bool AutoRefresh = true;
    public bool TestMode = false;
    public float SunHeight = 10000f;
    public Camera shotCam;// viewpoint camera
    private Texture2D tex = null; // viewpoint texture for storage
    private GameObject Landmark;
    private GameObject Park;
    private GameObject SunDirLight;
    private GameObject SunSphere;
    private GameObject SelectedBuilding;

    public float LandmarkVisTargetedLevel = 1f;
    public float ShadowsPerMinutes = 60f;
    public string FeatureString = "010";

    //private float TimeValue = 60f; // previous value
    private float SliderStartTimeValue = 0f; // current start value
    private float SliderStopTimeValue = 60f; // current stop value
    private float SliderSkyOffsetX = 0;
    private float SliderSkyOffsetZ = 0;
    private int SkyViewWidth = 256;
    private int SkyViewHeight = 256;
    private int LastVisViewCount = 0;

    private Vector3 LastPos = Vector3.zero;
    private const string strT_Landmark = "T_Landmark";
    private const string strUntagged = "Untagged";
    private const string strLandmark = "Landmark";
    private const string strDefault = "Default";
    private const string strSelected = "Selected";
    private const float SunEastUpTime = 6.0f;
    private const float SunWestDownTime = 18.0f;

    private float Sun2OriginalDistance = 10000f;
    private float SunZenithAngle = 30.0f;

    private Vector3 ParkPos = Vector3.zero;
    private float ParkWidth = 6; // localscale.x
    private float ParkHeight = 3; // localscale.z
    private float ws = 1; // x
    private float hs = 1; // z
    private float mu = 10;
    private List<Ray> RaysPark = new List<Ray>();
    private List<int> ParkShadow = new List<int>();
    private List<int> ParkShadows = new List<int>();
    private int patternMax = 1;
    private List<int> Histogram = new List<int>();
    private float HistogramStartTimeValue = 0;
    private float HistogramStopTimeValue = 0;
    private float HeatmapStartTimeValue = 0;
    private float HeatmapStopTimeValue = 0;
    private float SunShadowTimeValue = 0;
    private string[] materialName_House1;
    private string[] materialName_House2;
    private RaycastHit hitInfoSky;
    private bool hitboolSky;
    private string StrGraphCaption;

    private Mode LastMode = Mode.Mode_CFH;

    // double click time stamp
    private double t1;
    private double t2;
    private bool IsMoving = false;
    private bool ShowContextMenu = false;
    private Vector3 lastMousePosition;
    private Vector3 LastSkyPos = Vector3.zero;

    private BinarSave BS;
    private bool CreateByPredefined = false;
    private GameObject Predefined = null;
    private Texture2D ParkTexture = null;

    private bool windowShow = true;
    private bool windowHistogramShow = true;
    private Rect winRect = new Rect(30, 30, 300, 250);
    private Rect winRectCreateByPredefined = new Rect(30, 230, 150, 200);
    private Rect winRectHistogram = new Rect(400, 30, 450, 280);

    [System.Serializable]
    public class BinarSave
    {
        public List<string> name = new List<string>();
        public List<string> tag = new List<string>();
        public List<SerializableVector3> position = new List<SerializableVector3>();
        public List<SerializableQuaternion> rotation = new List<SerializableQuaternion>();
        public List<SerializableVector3> scale = new List<SerializableVector3>();
    }

    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }

        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3(rValue.x, rValue.y, rValue.z);
        }
    }

    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }

        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }
    public Vector3 GetPosition(float t)
    {
        float AzimuthAngle = (t - SunEastUpTime) / (SunWestDownTime - SunEastUpTime) * 180;// + 90;
        float x = Sun2OriginalDistance * Mathf.Cos(Mathf.Deg2Rad * AzimuthAngle);
        float y = Sun2OriginalDistance * Mathf.Sin(Mathf.Deg2Rad * AzimuthAngle) * Mathf.Cos(Mathf.Deg2Rad * SunZenithAngle);
        float z = Sun2OriginalDistance * Mathf.Sin(Mathf.Deg2Rad * AzimuthAngle) * (-Mathf.Sin(Mathf.Deg2Rad * SunZenithAngle));
        //float x = Sun2OriginalDistance * Mathf.Cos(Mathf.Deg2Rad * YAngle) * Mathf.Cos(Mathf.Deg2Rad * SunBiasAngle);
        //float y = Sun2OriginalDistance * Mathf.Sin(Mathf.Deg2Rad * YAngle);
        //float z = Sun2OriginalDistance * Mathf.Cos(Mathf.Deg2Rad * YAngle) * (-Mathf.Sin(Mathf.Deg2Rad * SunBiasAngle));
        x = (int)(x * Mathf.Pow(10, 2)) / Mathf.Pow(10, 2);
        y = (int)(y * Mathf.Pow(10, 2)) / Mathf.Pow(10, 2);
        z = (int)(z * Mathf.Pow(10, 2)) / Mathf.Pow(10, 2);
        LastPos = new Vector3(x, y, z);
        //Debug.Log(t.ToString() + " " + LastPos.ToString());
        return LastPos;
    }

    public void SplitPark(Vector3 pos, float minUnit = 1f)
    {
        mu = minUnit;
        ws = ParkWidth * 10 / minUnit;
        hs = ParkHeight * 10 / minUnit;
        ParkPos = pos;
    }

    // Function GetRaysPark(p, su) . return is a set of rays
    public List<Ray> GetRaysPark()
    {
        float hs_i = 0f;
        float ws_j;
        RaysPark = new List<Ray>();
        while (hs_i < hs) // left-top is the start point for the array of rays
        {
            ws_j = 0f;
            while (ws_j < ws)
            {
                Vector3 RayPos = ParkPos + new Vector3(-mu * ws / 2, 0, mu * hs / 2) + new Vector3(mu * ws_j, 0, -mu * hs_i) + new Vector3(mu / 2, 0, -mu / 2);
                Vector3 RayDir = LastPos - RayPos;
                RaysPark.Add(new Ray(RayPos, RayDir));
                ws_j += 1;
            }
            hs_i += 1;
        }
        return RaysPark;
    }

    /// <summary>
    /// Function GetHistogram(), count the number from 0 to max and return 
    /// </summary>
    /// <param name="input"></param>
    /// <returns>return is a set of value</returns>
    public List<int> GetHistogram()
    {
        if ((ShadowsPerMinutes <= 0) || (ShadowsPerMinutes > 12 * 60 / 2))
            ShadowsPerMinutes = 60f;
        if ((Mathf.Abs(HistogramStartTimeValue - SliderStartTimeValue) > 0.1f) || (Mathf.Abs(HistogramStopTimeValue - SliderStopTimeValue) > 0.1f))
        {
            List<int> resultCFM = ComputeShadows(ShadowsPerMinutes / 60f, TimeValueMinsToHourFloat(SliderStartTimeValue), TimeValueMinsToHourFloat(SliderStopTimeValue)); // 1f/60 = 1 minute, //0.1f
            HistogramStartTimeValue = SliderStartTimeValue;
            HistogramStopTimeValue = SliderStopTimeValue;
        }

        Histogram = new List<int>();
        for (int i = 0; i <= patternMax; i++)
            Histogram.Add(0);

        for (int i = 0; i < ParkShadows.Count; i++)
        {
            if (ParkShadows[i] <= patternMax)
                Histogram[ParkShadows[i]]++;
        }
            
        return Histogram;
    }

    private void DrawRect(Texture2D texture, Vector2 position, Vector2 size, Color newcolor, bool useSetPixels = true)
    {
        if (useSetPixels)
        {
            var colors = new Color[3];
            colors[0] = Color.red;
            colors[1] = Color.green;
            colors[2] = Color.blue;
            var mipCount = 1;
            for (var mip = 0; mip < mipCount; ++mip)
            {
                var cols = new Color[mipCount * (int)size.x * (int)size.y];
                for (var i = 0; i < cols.Length; ++i)
                {
                    cols[i] = newcolor;
                }
                texture.SetPixels((int)position.x, (int)position.y, (int)size.x, (int)size.y, cols, mip);
                cols = null;
            }
            colors = null;
        }
        else
        {
            // setpixel solution
            int x, y;
            for (y = (int)position.y; y < position.y + size.y; y++)
            {
                for (x = (int)position.x; x < position.x + size.x; x++)
                {
                    texture.SetPixel((int)x, (int)y, newcolor);
                }
            }
        }
    }

    Vector2 scrollPosition = Vector2.zero;
    private void DrawHistogram(List<int> HistogramValue)
    {
        float width = 380f;
        float height = 200f;
        float hmargin = 20f;
        float width_offset = 30f;
        float height_offset = 30f;
        int maximum = Mathf.Clamp(patternMax, 5, patternMax);
        float r = 0.8f;// ratio for bar width
        float w_unit = Mathf.Clamp(Mathf.Round(width / (maximum + 1)), 30, 100);
        float maxValue = 0;
        float label_offset = 15f;
        float drawThreshold = 3;
        int yAccount = 10;

        if (HistogramValue.Count > 0)
        {

            Rect area = new Rect(width_offset, height_offset, Mathf.RoundToInt(width + 20), Mathf.RoundToInt(height + 40));
            width = w_unit * (maximum + 1);
            Rect scrollarea = new Rect(width_offset, height_offset, Mathf.RoundToInt(width), Mathf.RoundToInt(height + 20));
            scrollPosition = GUI.BeginScrollView(area, scrollPosition, scrollarea);

            for (int i = 0; i < HistogramValue.Count; i++)
                if (maxValue < HistogramValue[i])
                    maxValue = HistogramValue[i];

            float h_unit = (height - hmargin) / (maxValue + 1);

            // left-top is x=0,y=0        
            Texture2D texture = new Texture2D(Mathf.RoundToInt(width), Mathf.RoundToInt(height));

            // Draw auxiliary horizon lines
            for (int i = 0; i <= yAccount; i++)
            {
                //height + height_offset - i*h_unit
                Rect bar = new Rect(0, i * (height - hmargin) / yAccount, width, 1);
                DrawRect(texture, new Vector2(bar.x, bar.y), new Vector2(bar.width, bar.height), Color.gray);
            }

            if (patternMax >= drawThreshold)
            {
                // Draw the histogram figure
                for (int i = 0; i < HistogramValue.Count; i++)
                {
                    Rect bar = new Rect((i + (1 - r) / 2) * w_unit, 0, w_unit * r, HistogramValue[i] * h_unit);
                    Color color = new Color(i / (float)patternMax, 0,
                            1 - i / (float)patternMax);
                    DrawRect(texture, new Vector2(bar.x, bar.y), new Vector2(bar.width, bar.height), color);// Color.red);
                }

                texture.Apply();
                GUI.DrawTexture(new Rect(width_offset, height_offset, texture.width, texture.height), texture);
                Destroy(texture);
            }
            else
            { 
                for (int i = 0; i < HistogramValue.Count; i++)
                {
                    Rect bar = new Rect((i + (1 - r) / 2) * w_unit + width_offset, height_offset + height - HistogramValue[i] * h_unit, w_unit * r, HistogramValue[i] * h_unit);
                    GUI.Box(bar,"");
                }
            }


            // Horizon line labels
            for (int i = 0; i < HistogramValue.Count; i++)
            {
                Rect bar = new Rect((i + (1 - r) / 2) * w_unit, 0, w_unit * r, HistogramValue[i] * h_unit);
                bar.x += width_offset;
                bar.y = height - bar.height + label_offset;
                GUIStyle lblStyle = new GUIStyle();
                lblStyle.alignment = TextAnchor.UpperCenter;
                if (patternMax >= drawThreshold)
                    lblStyle.normal.textColor = Color.black;
                else
                    lblStyle.normal.textColor = Color.white;
                GUI.Label(bar, HistogramValue[i].ToString(), lblStyle);
                lblStyle.normal.textColor = Color.white;
                bar.y = height + height_offset;
                GUI.Label(bar, i.ToString(), lblStyle);
            }

            GUI.EndScrollView();

            // Vertical line labels
            for (int i = 1; i <= yAccount; i++)
            {
                //height + height_offset - i*h_unit
                Rect bar = new Rect(0, (height + hmargin) - i * (height - hmargin) / yAccount, 28, 30);
                GUIStyle lblStyle = new GUIStyle();
                lblStyle.alignment = TextAnchor.UpperRight;
                lblStyle.normal.textColor = Color.white;
                GUI.Label(bar, Mathf.RoundToInt((float)i / yAccount * maxValue).ToString(), lblStyle);
            }
        }    
    }

    //Function computeShadow
    //Data: Scene S, Sun su, Park p
    //Result: shadow map M
    public List<int> ComputeShadow(Vector3 Su)
    {
        RaycastHit hit;
        GetRaysPark();
        ParkShadow = new List<int>();
        for (int i = 0; i < RaysPark.Count; i++)
        {
            if (Physics.Raycast(RaysPark[i].origin, RaysPark[i].direction, out hit, 10000, 1 << 0))//, QueryTriggerInteraction.Collide
            {
                ParkShadow.Add(1);
                // Debug.Log("Did Hit");
            }
            else
                ParkShadow.Add(0);
        }
        return ParkShadow;
    }

    public List<int> ComputeShadows(float stept, float start_time = SunEastUpTime, float stop_time = SunWestDownTime)
    {
        ParkShadows = new List<int>();
        if(start_time>=stop_time)
            ParkShadows.AddRange(ComputeShadow(GetPosition(start_time)));
        else
            for (float i = start_time; i <stop_time - Mathf.Pow(10, -4); i = i + stept)
            {
                List<int> tParkShadow = ComputeShadow(GetPosition(i));
                if (ParkShadows.Count == 0)
                    ParkShadows.AddRange(tParkShadow);
                else
                    for (int j = 0; j < ParkShadows.Count; j++)
                        ParkShadows[j] += ParkShadow[j];
            }
        
        int i_count = (int)hs;
        int j_count = (int)ws;
        int max = 1;
        for (int i = 0; i < i_count; i++)
            for (int j = 0; j < j_count; j++)
            {
                if (ParkShadows[i * j_count + j] > max)
                    max = ParkShadows[i * j_count + j];
            }

        patternMax = max;
        if (TestMode)
            Debug.Log("E003: max value per minimum unit is " + patternMax.ToString() + ", marked as Red color in heatmap."); 

        return ParkShadows;
    }

    public List<int> ComputeCFH(float stept)
    {
        ParkShadows = new List<int>();
        int t_count = 0;
        for (float i = SunEastUpTime; i < SunWestDownTime - Mathf.Pow(10, -4); i = i + stept)
        {
            t_count++;
            List<int> tParkShadow = ComputeShadow(GetPosition(i));
            ParkShadows.AddRange(tParkShadow);
        }

        
        // t count = frame size of t
        // ws = account size of width
        // hs = account size of height
        List<int> CountParkShadowsCFH = new List<int>();
        int[] sub;// = new int[] { 0, 1 };//{ 1, 1, 1 }
        sub = Fun_FeatureStrToInt(FeatureString);
        if (sub == null)
        {
            FeatureString = "010";
            sub = Fun_FeatureStrToInt(FeatureString);
        }
        int i_count = (int)hs;
        int j_count = (int)ws;
        int[] data = new int[t_count - 1];
        int max = 1;
        if(t_count >= 2)
            for (int i = 0; i < i_count; i++)
                for (int j = 0; j < j_count; j++)
                {
                    for (int t = 0; t < t_count - 1; t++)
                    {
                        int binaryMapData = ParkShadows[t * (j_count * i_count) + i * j_count + j] !=
                            ParkShadows[(t + 1) * (j_count * i_count) + i * j_count + j] ? 1 : 0;
                        data[t] = binaryMapData;
                    }
                    List<int> a = Fun_SubFeatureForData(data, sub);
                    if (a.Count > max)
                        max = a.Count;
                    CountParkShadowsCFH.Add(a.Count);
                }

        patternMax = max;
        if (TestMode)
            Debug.Log("E004: feature (" + FeatureString + ") max value per minimum unit is " + patternMax.ToString() + ", marked as Red color in heatmap.");
        return CountParkShadowsCFH;
    }

    public int[] Fun_FeatureStrToInt(string feature)
    {
        char[] c_feature;
        int[] result;
        if (feature.Length > 0)
        {
            c_feature = feature.ToCharArray();
            result = new int[c_feature.Length];
            for (int i = 0; i < c_feature.Length; i++)
            {
                if (c_feature[i] == '0')
                    result[i] = 0;
                else if (c_feature[i] == '1')
                    result[i] = 1;
                else
                    return null;
            }
            return result;
        }
        return null;
    }

    public List<int> Fun_SubFeatureForData(int[] data, int[] sub)
    {
        //int[] data = new int[] { 1, 1, 1, 0, 1, 0, 1 };
        //int[] sub = new int[] { 1, 0, 1 };
        List<int> result = new List<int>();
        for (int i = 0; i < data.Length - sub.Length + 1; i++)
            for (int j = 0; j < sub.Length; j++)
            {
                if (data[i + j] == sub[j])
                {
                    if (j + 1 == sub.Length)
                    {
                        result.Add(i);
                    }
                }
                else
                    break;
            }
        // return [2,4] for List
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        Landmark = GameObject.FindGameObjectWithTag(strT_Landmark);
        Park = GameObject.FindGameObjectWithTag("T_Park");
        SunSphere = GameObject.FindGameObjectWithTag("T_Sun");
        SunDirLight = GameObject.Find("Sun");

        Predefined = GameObject.Find("Predefined");
        Predefined.SetActive(false);

        materialName_House1 = new string[] { "A3", "K7", "Windows no light", "A3", "A1", "Windows no light", "A2", "A3", "A2" };
        materialName_House2 = new string[] { "A", "A2", "A", "Windows no light", "A1", "A1", "Windows no light", "A3", "A2", "A3", "K10", "K9", "A3", "K7", "A3", "A2" };
        hitInfoSky = new RaycastHit();
        hitboolSky = false;

        SelectedMode = Mode.Mode_SunShadow;
        BS = new BinarSave();
    }
    // Update is called once per frame
    void OnGUI()
    {
        windowShow = GUI.Toggle(new Rect(30, 10, 200, 20), windowShow, "Information");
        if (windowShow)
        {
            winRect = GUI.Window(0, winRect, WindowFun, SelectedMode.ToString() + "   F" + ((int)SelectedMode).ToString());
        }
        if (CreateByPredefined)
            winRectCreateByPredefined = GUI.Window(1, winRectCreateByPredefined, WindowFunCreateByPredefined, "Insert building as");

        windowHistogramShow = GUI.Toggle(new Rect(400, 10, 200, 20), windowHistogramShow, "Graph");
        if (windowHistogramShow)
        {
            if ((SelectedMode == Mode.Mode_LandMarkVis) || (SelectedMode == Mode.Mode_EditMode) || (SelectedMode == Mode.Mode_CFH))
            { }
            else
            {
                if (SelectedMode == Mode.Mode_SunShadow)
                    StrGraphCaption = "(6:00) - (" + DTFloat2Str(SunShadowTimeValue) + ")";
                else if (SelectedMode == Mode.Mode_HeatmapShadow)
                    StrGraphCaption = "(" + DTFloat2Str(HeatmapStartTimeValue) + ") - (" + DTFloat2Str(HeatmapStopTimeValue) + ")";
                else
                    StrGraphCaption = "Unknown";
                winRectHistogram = GUI.Window(2, winRectHistogram, WindowFunHistogram, SelectedMode == Mode.Mode_SkyExposure ? "Sky graph" : "Histogram Graph " + StrGraphCaption);
            }
        }

        if (ShowContextMenu)
        {
            if (GUI.Button(new Rect(lastMousePosition.x, Screen.height - lastMousePosition.y, 100, 30), "Load Scene"))
            {
                DestroyChildren("Buildings");
                Invoke("Fun_LoadSceneNone", 0.5f);
            }
            if (GUI.Button(new Rect(lastMousePosition.x, Screen.height - lastMousePosition.y + 35, 100, 30), "Save Scene"))
            {
                Fun_SaveScene();
            }
        }
    }

    void WindowFun(int windowID)
    {
       //float LeftX = 10f;

        GUILayout.BeginVertical();
        #region status box
        GUILayout.BeginHorizontal();
        if (AutoRefresh)
            GUILayout.Box("AutoUpdate");
        if (TestMode)
            GUILayout.Box("TestMode");
        GUILayout.EndHorizontal();
        #endregion
        GUILayout.Label("SunHeight: " + SunHeight);
        switch (SelectedMode)
        {
            case Mode.Mode_SunShadow:
                GUILayout.Label("StopTime     " + DTFloat2Str(SunShadowTimeValue));
                SliderStopTimeValue = Mathf.RoundToInt(GUILayout.HorizontalSlider(SunShadowTimeValue, 0f, 720f));

                break;
            case Mode.Mode_LandMarkVis:
                if (Landmark != null)
                    GUILayout.Label(strLandmark + ": " + Landmark.name);
                GUILayout.Label( "Altitude: " + LandmarkVisTargetedLevel);
                MeshRenderer[] buildingSet = GameObject.Find("Buildings").GetComponentsInChildren<MeshRenderer>();
                int iCount = 0;
                for (int i = 0; i < buildingSet.Length; i++)
                {
                    if (buildingSet[i].material.GetColor("_Color") == Color.green)
                        iCount++;
                }
                GUILayout.Label("Visual ratio = " + iCount.ToString() + "/" + (buildingSet.Length - 1));
                GUILayout.Label("Visual ratio = " + (float)iCount * 100 / (buildingSet.Length - 1) + "%");
                break;
            case Mode.Mode_HeatmapShadow:
                GUILayout.Label("PerMinutes: " + ShadowsPerMinutes);
                GUILayout.Label("Maximum: " + patternMax);
                GUILayout.Label("StartTime     " + DTFloat2Str(HeatmapStartTimeValue));
                SliderStartTimeValue = Mathf.RoundToInt(GUILayout.HorizontalSlider(HeatmapStartTimeValue, 0f, 720f));
                GUILayout.Label("StopTime     " + DTFloat2Str(HeatmapStopTimeValue));
                SliderStopTimeValue = Mathf.RoundToInt(GUILayout.HorizontalSlider(HeatmapStopTimeValue, 0f, 720f));
                if (SliderStartTimeValue >= SliderStopTimeValue)
                    SliderStopTimeValue = SliderStartTimeValue;
                break;
            case Mode.Mode_CFH:
                GUILayout.Label("PerMinutes: " + ShadowsPerMinutes);
                GUILayout.Label("Maximum: " + patternMax);
                GUILayout.Label("Pattern: " + FeatureString);
                FeatureString = GUILayout.TextField(FeatureString);
                break;
            case Mode.Mode_EditMode:
                GUILayout.Label("Selected: " + (SelectedBuilding == null ? "" : SelectedBuilding.name));
                if (IsMoving)
                {
                    GUILayout.Label("Move");
                    GUILayout.Label((SelectedBuilding.transform.position).ToString());//Camera.main.ScreenToWorldPoint(Input.mousePosition)
                }
                GUILayout.Label(Input.mousePosition.ToString());
                break;
            case Mode.Mode_SkyExposure:
                GUILayout.Label("Sky view point position:" + shotCam.transform.position.ToString());
                GUILayout.Label("Offset X = " + SliderSkyOffsetX+ "; Offset Z = " + SliderSkyOffsetZ);
                int range = 30;
                SliderSkyOffsetX = Mathf.RoundToInt(GUILayout.HorizontalSlider(SliderSkyOffsetX, -range, range));
                SliderSkyOffsetZ = Mathf.RoundToInt(GUILayout.HorizontalSlider(SliderSkyOffsetZ, -range, range));
                #region buttons
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Set as new"))
                {
                    LastSkyPos = LastSkyPos + new Vector3(SliderSkyOffsetX, 0, SliderSkyOffsetZ);
                    SliderSkyOffsetX = 0;
                    SliderSkyOffsetZ = 0;
                }
                if (GUILayout.Button("Reset offset"))
                {
                    SliderSkyOffsetX = 0;
                    SliderSkyOffsetZ = 0;
                }
                GUILayout.EndHorizontal();
                #endregion
                break;
        }
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    string DTFloat2Str(float fdatatime)
    {
        return (6 + Mathf.RoundToInt(fdatatime / 60)).ToString().PadLeft(2, '0') + ":"
                    + Mathf.RoundToInt(fdatatime % 60).ToString().PadLeft(2, '0');
    }

    void WindowFunCreateByPredefined(int windowID)
    {
        if (CreateByPredefined)
        {
            Predefined.SetActive(true);
            Transform[] PredefinedSet = Predefined.GetComponentsInChildren<Transform>();
            float PosX = 25f;
            float PosY = 30f;
            float Margin = 40f;
            winRectCreateByPredefined.height = PredefinedSet.Length * Margin;
            for (int i = 1; i < PredefinedSet.Length; i++)
            {
                if (GUI.Button(new Rect(PosX, PosY + Margin * (i - 1), 100, 30), PredefinedSet[i].name))
                {
                    CreateByPredefined = false;
                    GameObject cloneObj;
                    //GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Cube"), Vector3.zero, Quaternion.identity, transform
                    cloneObj = Instantiate(GameObject.Find(PredefinedSet[i].name)) as GameObject;
                    cloneObj.name = PredefinedSet[i].name + "_" + DTtoGameObjectName(DateTime.Now);
                    cloneObj.tag = strUntagged;
                    cloneObj.transform.parent = GameObject.Find("Buildings").transform;
                    ClearSelectedBuilding();
                    SelectedBuilding = cloneObj;
                    SetMaterial(SelectedBuilding, strSelected);
                    if (TestMode)
                        Debug.Log("E006: selected building is " + SelectedBuilding.name);
                    IsMoving = true;
                }
            }
            Predefined.SetActive(false);
        }

        GUI.DragWindow();
    }
    void OnPostRender()
    {
        if(SelectedMode == Mode.Mode_SkyExposure)
            Fun_CalculateSky();
    }

    void Fun_CalculateSky()//OnPostRender()
    {
        RenderTexture rt;
        if (tex != null)
        {
            Destroy(tex);
        }

        // follow the main camera for test
        //Transform camerapos = GameObject.Find("Top Camera").GetComponent<Transform>();
        //shotCam.transform.position = camerapos.position;// Camera.main.transform.position;
        //shotCam.transform.rotation = camerapos.rotation;// Camera.main.transform.rotation;
        //shotCam.transform.localScale = camerapos.localScale;//Camera.main.transform.localScale;
        //
        var pos = LastSkyPos + new Vector3(SliderSkyOffsetX, 0, SliderSkyOffsetZ);
        // Draw line to the up direction
        hitboolSky = Physics.Raycast(pos - new Vector3(0, 1, 0), Vector3.up, out hitInfoSky);
        if (hitboolSky)
        {
            Debug.DrawLine(pos, Vector3.up * 100 + pos, Color.red);
        }
        else
            Debug.DrawLine(pos, Vector3.up * 100 + pos, Color.blue);
        // Only display graph if not hit any building
        if (!hitboolSky)
        {
            switch (ViewMode)
            {
                case SkyView.Original:

                    pos.y = 0;
                    shotCam.transform.position = pos;
                    shotCam.transform.eulerAngles = new Vector3(270, 0, 0);
                    rt = shotCam.targetTexture;
                    RenderTexture.active = rt;
                    tex = new Texture2D(rt.width, rt.height);
                    tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                    tex.Apply();
                    break;

                case SkyView.Calculated:
                    pos.y = 0;
                    shotCam.transform.position = pos;
                    shotCam.transform.eulerAngles = new Vector3(270, 0, 0);
                    rt = shotCam.targetTexture;
                    RenderTexture.active = rt;
                    tex = new Texture2D(rt.width, rt.height);
                    tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

                    var cols = tex.GetPixels();// new Color[tex.width * tex.height];
                    LastVisViewCount = 0;
                    for (var i = 0; i < cols.Length; ++i)
                    {
                        if ((cols[i].r + cols[i].g + cols[i].b) / 3 * 255 >= 40)
                        {
                            cols[i] = Color.white;
                            LastVisViewCount++;
                        }
                        else
                            cols[i] = Color.red;
                    }
                    LastVisViewCount = LastVisViewCount / 12;
                    tex.SetPixels(cols, 0);
                    cols = null;
                    tex.Apply();
                    break;
                case SkyView.Top:
                    pos.y = 50f;
                    shotCam.transform.position = pos;
                    shotCam.transform.eulerAngles = new Vector3(90, 0, 0);
                    rt = shotCam.targetTexture;
                    RenderTexture.active = rt;
                    tex = new Texture2D(rt.width, rt.height);
                    tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                    tex.Apply();
                    break;
            }
        }
    }

    void WindowFunHistogram(int windowID)
    {
        if(SelectedMode == Mode.Mode_SkyExposure)
        {
           GUILayout.BeginArea(new Rect(20, 20, 100, 200));
            if (GUILayout.Button(SkyView.Original.ToString()))
                ViewMode = SkyView.Original;
            if (GUILayout.Button(SkyView.Calculated.ToString()))
                ViewMode = SkyView.Calculated;
            if (GUILayout.Button(SkyView.Top.ToString()))
                ViewMode = SkyView.Top;
            if (hitboolSky)
            {
                GUILayout.Label("The view point ");
                GUILayout.Label("in the building:");
                GUILayout.Label(hitInfoSky.collider.name);
            }
            else if (ViewMode == SkyView.Calculated)
            {
                GUILayout.Label("Visual ratio = " + LastVisViewCount + "/" + SkyViewWidth * SkyViewHeight);
                GUILayout.Label("Visual ratio = " + (float)LastVisViewCount * 100 / (SkyViewWidth * SkyViewHeight) + "%");
            }
                
            GUILayout.EndArea();

            if (!hitboolSky)
            {
                int marginx = 140;
                int marginy = 20;

                GUI.DrawTexture(new Rect(marginx, marginy, SkyViewWidth, SkyViewHeight), tex);

                if (ViewMode == SkyView.Top)
                {
                    float bw = 80;
                    float bh = 80;
                    float point_w = 5;
                    Texture2D texturebox = new Texture2D((int)bw, (int)bh);
                    DrawRect(texturebox, new Vector2(bw / 2 - point_w / 2, bh / 2 - point_w / 2), new Vector2(point_w, point_w), Color.blue, false);
                    texturebox.filterMode = FilterMode.Point;
                    texturebox.Apply();
                    GUI.Box(new Rect(marginx - bw / 2 + SkyViewWidth / 2, marginy - bh / 2 + SkyViewHeight / 2, bw, bh), texturebox);
                    Destroy(texturebox);
                }
            }
        }
        else
            DrawHistogram(GetHistogram());

        GUI.DragWindow();
    }

    void ResetBuildingsColor()
    {
        MeshRenderer[] buildingSet = GameObject.Find("Buildings").GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < buildingSet.Length; i++)
            if (buildingSet[i].tag != strT_Landmark)
                SetMaterial(buildingSet[i].gameObject, "Default");
    }

    float TimeValueMinsToHourFloat(float Mins)
    {
        float result;
        if (Mins >= 0 || Mins <= (SunWestDownTime - SunEastUpTime) * 60)
            result = Mins / 60f + SunEastUpTime;
        else
            result = SunEastUpTime;
        return result;
    }

    void Mode_SunShadowExec()
    {
        SunDirLight.transform.eulerAngles = new Vector3(120, 120, 0);
        SunSphere.SetActive(true);

        HeatmapStartTimeValue = 0;
        HeatmapStopTimeValue = 720;
        float New_TimeValue = SliderStopTimeValue;
        if (Mathf.Abs(New_TimeValue - SunShadowTimeValue) > 0.1f)
        {
            SunShadowTimeValue = New_TimeValue;
            Sun2OriginalDistance = SunHeight;
            ComputeShadow(GetPosition(TimeValueMinsToHourFloat(New_TimeValue)));

            SunSphere.transform.position = LastPos;
            Destroy(ParkTexture);
            ParkTexture = new Texture2D(Mathf.RoundToInt(ws), Mathf.RoundToInt(hs));
            Park.GetComponent<MeshRenderer>().materials = new Material[0];
            Park.GetComponent<Renderer>().material.mainTexture = ParkTexture;
            Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

            Park.GetComponent<Renderer>().sharedMaterial.SetFloat("_SpecularHighlights", 0);
            Park.GetComponent<Renderer>().sharedMaterial.SetFloat("_GlossyReflections", 0);
            if (ParkTexture.height * ParkTexture.width <= ParkShadow.Count)
                for (int y = 0; y < ParkTexture.height; y++)
                {
                    for (int x = 0; x < ParkTexture.width; x++)
                    {
                        //Color color = ((x + y) % 2 != 0 ? Color.red : Color.blue); 
                        Color color = (ParkShadow[y * ParkTexture.width + x] != 0 ? Color.gray : Color.green);
                        ParkTexture.SetPixel(ParkTexture.width - 1 - x, y, color);
                    }
                }
            ParkTexture.Apply();
        }
        if (TestMode)
        {
            for (int i = 0; i < RaysPark.Count; i++)
            {
                // Enable the Gizmos if the line can not be displayed
                Debug.DrawLine(RaysPark[i].origin, (RaysPark[i].direction * Sun2OriginalDistance + RaysPark[i].origin),
                    (ParkShadow[i] != 0 ? Color.red : Color.blue));
            }
        }
    }

    void Mode_LandMarkVisExec()
    {
        SunDirLight.transform.eulerAngles = new Vector3(120, 120, 0);
        SunSphere.SetActive(true);
        if (Input.GetMouseButtonDown(0))
        {
            t2 = Time.realtimeSinceStartup;
            if (t2 - t1 < 0.5f) //<0.5s, considered double click
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    Landmark.tag = strUntagged;
                    SetMaterial(Landmark, strDefault);
                    Landmark = GameObject.Find(hitInfo.collider.name); //hitInfo.transform.gameObject.name
                    Landmark.tag = strT_Landmark;
                    SetMaterial(Landmark, strLandmark);
                    if (TestMode)
                        Debug.Log("E005: new Landmark is selected as " + Landmark.name);
                }
            }
            t1 = t2;
        }

        ResetBuildingsColor();
        MeshRenderer[] buildingSet = GameObject.Find("Buildings").GetComponentsInChildren<MeshRenderer>();

        if(Landmark != null)
            Fun_LandmarkVis(LandmarkVisTargetedLevel);

        int iCount = 0;
        for (int i = 0; i < buildingSet.Length; i++)
        {
            if (buildingSet[i].material.GetColor("_Color") == Color.green)
                iCount++;
        }

        if (TestMode)
            Debug.Log("E002: There are " + iCount.ToString() + " / " + (buildingSet.Length - 1).ToString() +
                " buildings could see the landmark on altitude = " + LandmarkVisTargetedLevel.ToString() + ", marked as green");
    }

    void Mode_HeatmapShadow()
    {
        SunDirLight.transform.eulerAngles = new Vector3(120, 120, 0);
        SunSphere.SetActive(true);
        if ((ShadowsPerMinutes <= 0) || (ShadowsPerMinutes > 12 * 60 / 2))
            ShadowsPerMinutes = 60f;

        SunShadowTimeValue = 0;
        float New_StartTimeValue = SliderStartTimeValue;
        float New_StopTimeValue = SliderStopTimeValue;
        if ((Mathf.Abs(New_StartTimeValue - HeatmapStartTimeValue) > 0.1f) || (Mathf.Abs(New_StopTimeValue - HeatmapStopTimeValue) > 0.1f))
        {
            HeatmapStartTimeValue = New_StartTimeValue;
            HeatmapStopTimeValue = New_StopTimeValue;
            List<int> resultCFM = ComputeShadows(ShadowsPerMinutes / 60f, TimeValueMinsToHourFloat(New_StartTimeValue), TimeValueMinsToHourFloat(New_StopTimeValue)); // 1f/60 = 1 minute, //0.1f
            //List<int> resultCFM = ComputeShadows(ShadowsPerMinutes / 60f); // 1f/60 = 1 minute, //0.1f
            Destroy(ParkTexture);
            ParkTexture = new Texture2D(Mathf.RoundToInt(ws), Mathf.RoundToInt(hs));
            Park.GetComponent<MeshRenderer>().materials = new Material[0];
            Park.GetComponent<Renderer>().material.mainTexture = ParkTexture;
            Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

            if (ParkTexture.height * ParkTexture.width <= ParkShadow.Count)
                for (int y = 0; y < ParkTexture.height; y++)
                {
                    for (int x = 0; x < ParkTexture.width; x++)
                    {
                        // maxium value is color red, and zero value
                        Color color = new Color(resultCFM[y * ParkTexture.width + x] / (float)patternMax, 0,
                            1 - resultCFM[y * ParkTexture.width + x] / (float)patternMax);
                        ParkTexture.SetPixel(ParkTexture.width - 1 - x, y, color);
                    }
                }
            ParkTexture.Apply();
        }
    }

    void Mode_CFH()
    {
        SunDirLight.transform.eulerAngles = new Vector3(120, 120, 0);
        SunSphere.SetActive(true);
        if ((ShadowsPerMinutes <= 0) || (ShadowsPerMinutes > 12 * 60 / 2))
            ShadowsPerMinutes = 60f;
        List<int> resultCFH = ComputeCFH(ShadowsPerMinutes / 60f); // 1f/60 = 1 minute, //0.1f
        Destroy(ParkTexture);
        ParkTexture = new Texture2D(Mathf.RoundToInt(ws), Mathf.RoundToInt(hs));
        Park.GetComponent<MeshRenderer>().materials = new Material[0];
        Park.GetComponent<Renderer>().material.mainTexture = ParkTexture;
        Park.GetComponent<Renderer>().material.mainTexture.filterMode = FilterMode.Point;

        if (ParkTexture.height * ParkTexture.width <= ParkShadow.Count)
            for (int y = 0; y < ParkTexture.height; y++)
            {
                for (int x = 0; x < ParkTexture.width; x++)
                {
                    // maxium value is color red, and zero value
                    Color color = new Color(resultCFH[y * ParkTexture.width + x] / (float)patternMax, 0,
                        1 - resultCFH[y * ParkTexture.width + x] / (float)patternMax);
                    ParkTexture.SetPixel(ParkTexture.width - 1 - x, y, color);
                }
            }
        ParkTexture.Apply();
    }

    void Mode_Edit()
    {
        SunDirLight.transform.eulerAngles = new Vector3(120, 120, 0);
        SunSphere.SetActive(true);
        if (Input.GetMouseButtonDown(0))
        {
            t2 = Time.realtimeSinceStartup;
            if (t2 - t1 < 0.5f) //<0.5s, considered double click
            {
                if (SelectedBuilding != null)
                {
                    IsMoving = true;
                }
            }
            else
            {
                // single mouse click
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                ClearSelectedBuilding();
                if (hit)
                {
                    SelectedBuilding = GameObject.Find(hitInfo.collider.name);
                    SetMaterial(SelectedBuilding, strSelected);
                    if (TestMode)
                        Debug.Log("E006: selected building is " + SelectedBuilding.name);
                }
                else
                {
                }

                if (IsMoving)
                {
                    ClearSelectedBuilding();
                    SelectedBuilding = null;
                    IsMoving = false;
                }
            }
            t1 = t2;
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    //Debug.Log(keyCode.ToString());
                    switch (keyCode)
                    {
                        case KeyCode.Delete:
                            if (SelectedBuilding != null)
                            {
                                Destroy(SelectedBuilding);
                                if (TestMode)
                                    Debug.Log("E007: selected building " + SelectedBuilding.name + " is deleted");
                                SelectedBuilding = null;
                            }
                            break;
                        case KeyCode.M:
                            if (SelectedBuilding != null)
                            {
                                if (IsMoving)
                                {
                                    ClearSelectedBuilding();
                                    SelectedBuilding = null;
                                }
                                IsMoving = !IsMoving;
                            }
                            else
                            {
                                IsMoving = false;
                            }
                            break;
                        case KeyCode.Insert:
                            GameObject cloneObj;
                            if (SelectedBuilding != null)
                            {
                                Predefined.SetActive(true);
                                Transform[] PredefinedSet = Predefined.GetComponentsInChildren<Transform>();
                                for (int j = 1; j < PredefinedSet.Length; j++)
                                    if (SelectedBuilding.name.Contains(PredefinedSet[j].name))
                                    {
                                        cloneObj = Instantiate(SelectedBuilding) as GameObject;
                                        cloneObj.name = PredefinedSet[j].name + "_" + DTtoGameObjectName(DateTime.Now);
                                        cloneObj.tag = "Untagged";
                                        cloneObj.transform.parent = GameObject.Find("Buildings").transform;
                                        ClearSelectedBuilding();
                                        SelectedBuilding = cloneObj;
                                        IsMoving = true;
                                    }
                                Predefined.SetActive(false);  
                            }
                            else
                            {
                                CreateByPredefined = true;
                            }
                            break;
                    }
                }
            }
        }
        if ((SelectedBuilding != null) && (IsMoving))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2 = new RaycastHit();

            if (Physics.Raycast(ray, out hit2, 10000, 1 << 2))
            {
                SelectedBuilding.transform.position = new Vector3(hit2.point.x, SelectedBuilding.transform.position.y, hit2.point.z);
            }
        }
    }

    public void Mode_SkyExposure() 
    {
        SunDirLight.transform.eulerAngles = new Vector3(90, 120, 0);
        SunSphere.SetActive(false);
        if (Input.GetMouseButtonDown(0))
        {
            t2 = Time.realtimeSinceStartup;
            if (t2 - t1 < 0.5f) //<0.5s, considered double click
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 10000, 1 << 2);
                if (hit)
                {
                    LastSkyPos = hitInfo.point;
                }
            }
            t1 = t2;
        }
    }

    void ClearSelectedBuilding()
    {
        if (SelectedBuilding != null)
        {
            if (SelectedBuilding.tag == strT_Landmark)
                SetMaterial(SelectedBuilding, strLandmark);
            else
                SetMaterial(SelectedBuilding, strDefault);
            SelectedBuilding = null;
        }
    }

    void ModeExecution(Mode executedMode)
    {
        switch (executedMode)
        {
            case Mode.Mode_SunShadow:
                Mode_SunShadowExec();
                break;
            case Mode.Mode_LandMarkVis:
                Mode_LandMarkVisExec();
                break;
            case Mode.Mode_HeatmapShadow:
                Mode_HeatmapShadow();
                break;
            case Mode.Mode_CFH:
                Mode_CFH();
                break;
            case Mode.Mode_EditMode:
                Mode_Edit();
                break;
            case Mode.Mode_SkyExposure:
                Mode_SkyExposure();
                break;
        }
    }
    void ModeUpdate()
    {
        if ((LastMode != SelectedMode) || (AutoRefresh))
        {
            ModeExecution(SelectedMode);
            if (TestMode)
                Debug.Log("E001: Mode executed as " + SelectedMode.ToString());
            LastMode = SelectedMode;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    //Debug.Log(keyCode.ToString());
                    switch (keyCode)
                    {
                        case KeyCode.F1:
                            ResetBuildingsColor();
                            SelectedMode = Mode.Mode_SunShadow;
                            break;
                        case KeyCode.F2:
                            SelectedMode = Mode.Mode_LandMarkVis;
                            break;
                        case KeyCode.F3:
                            ResetBuildingsColor();
                            SelectedMode = Mode.Mode_HeatmapShadow;
                            break;
                        case KeyCode.F4:
                            ResetBuildingsColor();
                            SelectedMode = Mode.Mode_CFH;
                            break;
                        case KeyCode.F5:
                            ResetBuildingsColor();
                            SelectedBuilding = null;
                            SelectedMode = Mode.Mode_EditMode;
                            break;
                        case KeyCode.F6:
                            ResetBuildingsColor();
                            SelectedBuilding = null;
                            SelectedMode = Mode.Mode_SkyExposure;
                            break;
                        case KeyCode.Home:
                            Fun_LoadScene();
                            break;
                        case KeyCode.End:
                            Fun_SaveScene();
                            break;
                    }
                }
            }
        }
        SplitPark(Park.transform.position, 1f); // min accuracy is 0.1f
        ModeUpdate();
        if (Input.GetMouseButtonDown(1))
        {
            if (CreateByPredefined)
                CreateByPredefined = false;
            else
            {
                ShowContextMenu = true;
                lastMousePosition = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Invoke("CancelContextMenu", 0.5f);
        }
    }

    public void CancelContextMenu()
    {
        ShowContextMenu = false;
        lastMousePosition = Vector3.zero;
    }

    /// <summary>
    /// Calculate the landmark visibility
    /// </summary>
    /// <param name="heightPos">the height of targeted level, the y value </param>
    /// <param name="stepAng">the step angle for raycast loop</param>
    /// <returns></returns>
    public void Fun_LandmarkVis(float heightPos, float stepAng = 1f, float startAng = 0f, float endAng = 360f)
    {
        RaycastHit hit;
        Landmark.layer = 2;
        for (float i = startAng; i < endAng; i+= stepAng)
        {
            Vector3 levelPos = Landmark.transform.position -
                new Vector3(Landmark.transform.position.y == 0 ? -5:0, Landmark.transform.position.y == 0 ? 0 : Landmark.transform.localScale.y / 2, Landmark.transform.position.y == 0 ? 3 : 0) +
                new Vector3(0, heightPos, 0); 
            Vector3 levelDir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i), 0, Mathf.Sin(Mathf.Deg2Rad * i));
            if (Physics.Raycast(levelPos, levelDir, out hit, 10000, 1 << 0, QueryTriggerInteraction.Collide))
            {
                if (GameObject.Find(hit.transform.name).tag == strLandmark)
                {
                    ;// GameObject.Find(hit.transform.name).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                }
                else
                {
                    MeshRenderer ma = GameObject.Find(hit.transform.name).GetComponent<MeshRenderer>();
                    for (int j = 0; j < ma.materials.Length; j++)
                        ma.materials[j].SetColor("_Color", Color.green);
                }
                // Debug.Log(i.ToString() + "_" + hit.transform.name);
                if (TestMode)
                    Debug.DrawLine(levelPos, hit.point, Color.red);
            }
            else
                if(TestMode)
                    Debug.DrawLine(levelPos, levelDir * 1000 + levelPos, Color.blue);
        }
        Landmark.layer = 0;
    }

    public void Fun_SaveScene(string fileName = "Save.txt")
    {
        BinaryFormatter BF = new BinaryFormatter();
        FileStream Fs = File.Create(Application.dataPath + fileName);
        Transform[] buildingSet = GameObject.Find("Buildings").GetComponentsInChildren<Transform>();
        for (int i = 0; i < buildingSet.Length; i++)
        {
            BS.name.Add(buildingSet[i].name);
            BS.tag.Add(buildingSet[i].tag);
            BS.position.Add(buildingSet[i].position);
            BS.rotation.Add(buildingSet[i].rotation);
            BS.scale.Add(buildingSet[i].localScale);
        }
        BF.Serialize(Fs, BS);
        Fs.Close();
    }

    public void Fun_LoadSceneNone()
    {
        Fun_LoadScene();
    }

    public void Fun_LoadScene(string fileName = "Save.txt")
    {
        BinaryFormatter Bf = new BinaryFormatter();
        if (File.Exists(Application.dataPath + "/Save.txt"))
        {
            FileStream FS = File.Open(Application.dataPath + fileName, FileMode.Open);
            BinarSave bs = (BinarSave)Bf.Deserialize(FS);

            for (int i = 0; i < bs.name.Count; i++)
            {
                GameObject obj = GameObject.Find(bs.name[i]);

                if (obj == null)
                {
                    Predefined.SetActive(true);
                    Transform[] PredefinedSet = Predefined.GetComponentsInChildren<Transform>();
                    for (int j = 1; j < PredefinedSet.Length; j++)
                        if (bs.name[i].Contains(PredefinedSet[j].name))
                        {
                            obj = Instantiate(GameObject.Find(PredefinedSet[j].name)) as GameObject;
                            obj.name = bs.name[i];
                            obj.tag = "Untagged";
                            obj.transform.parent = GameObject.Find("Buildings").transform;
                        }
                    Predefined.SetActive(false);
                }
                
                if (obj != null)
                {
                    obj.transform.tag = bs.tag[i];
                    obj.transform.position = bs.position[i];
                    obj.transform.rotation = bs.rotation[i];
                    obj.transform.localScale = bs.scale[i];
                    //if (i == 22)
                    //{
                    //    obj.transform.tag = "T_Landmark";
                    //}
                    if (obj.transform.tag == strT_Landmark)
                    {
                        if (Landmark != null)
                        {
                            Landmark.tag = strUntagged;
                            SetMaterial(Landmark, strDefault);
                        }
                        Landmark = obj;
                        SetMaterial(Landmark, strLandmark);
                    }
                }
            }
            FS.Close();
        }
    }

    public void DestroyChildren(string parentName)
    {
        Transform[] buildingSet = GameObject.Find(parentName).GetComponentsInChildren<Transform>();
        for (int i = 1; i < buildingSet.Length; i++)
        {
            Destroy(GameObject.Find(buildingSet[i].name) as GameObject);
        }
    }

    public void SetMaterialArray(GameObject target, string materialName, string[] materialArrayName)
    {
        // Change material to Landmark mode
        if (materialName == strLandmark)
        {
            Material[] materials = target.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = Resources.Load(materialName) as Material;
            target.GetComponent<MeshRenderer>().materials = materials;
        }
        // Change material to selected mode
        else if (materialName == strSelected)
        {
            Material[] materials = target.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = Resources.Load(materialName) as Material;
            target.GetComponent<MeshRenderer>().materials = materials;
        }
        // Change material to default mode
        else if (materialName == strDefault)
        {
            Material[] materials = target.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = Resources.Load(materialArrayName[i]) as Material;
            target.GetComponent<MeshRenderer>().materials = materials;
        }
    }

    public void SetMaterial(GameObject target, string materialName)
    {
        if (target.name.Contains("House"))
        {
            if (target.name.Contains("House1"))
            {
                SetMaterialArray(target, materialName, materialName_House1);
            }
            else if (target.name.Contains("House2"))
            {
                SetMaterialArray(target, materialName, materialName_House2);
            }
        }
        else
        { 
            Material[] materials = new Material[]
            {
                        Resources.Load(materialName) as Material,//Landmark
            };
            target.GetComponent<MeshRenderer>().materials = materials;
        }
    }

    public string DTtoGameObjectName(DateTime dt)
    {
        string sp = "-";
        string mp = "_";
        return dt.Year.ToString() + sp + dt.Month.ToString().PadLeft(2, '0') + sp + dt.Day.ToString().PadLeft(2, '0') + mp + dt.Hour.ToString().PadLeft(2, '0') + sp + dt.Minute.ToString().PadLeft(2, '0') + sp + dt.Second.ToString().PadLeft(2, '0');
    }
}