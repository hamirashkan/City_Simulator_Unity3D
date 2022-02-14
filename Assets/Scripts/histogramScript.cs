using UnityEngine;

public class histogramScript : MonoBehaviour
{

    public ComputeShader shader;
    public Texture2D inputTexture;
    public uint[] histogramData;

    ComputeBuffer histogramBuffer;
    int handleMain;
    int handleInitialize;

    void Start()
    {
        if (null == shader || null == inputTexture)
        {
            Debug.Log("Shader or input texture missing.");
            return;
        }

        handleInitialize = shader.FindKernel("HistogramInitialize");
        handleMain = shader.FindKernel("HistogramMain");
        histogramBuffer = new ComputeBuffer(256, sizeof(uint) * 4);
        histogramData = new uint[256 * 4];

        if (handleInitialize < 0 || handleMain < 0 ||
           null == histogramBuffer || null == histogramData)
        {
            Debug.Log("Initialization failed.");
            return;
        }

        shader.SetTexture(handleMain, "InputTexture", inputTexture);
        shader.SetBuffer(handleMain, "HistogramBuffer", histogramBuffer);
        shader.SetBuffer(handleInitialize, "HistogramBuffer", histogramBuffer);
    }

    void OnDestroy()
    {
        if (null != histogramBuffer)
        {
            histogramBuffer.Release();
            histogramBuffer = null;
        }
    }

    void Update()
    {
        if (null == shader || null == inputTexture ||
           0 > handleInitialize || 0 > handleMain ||
           null == histogramBuffer || null == histogramData)
        {
            Debug.Log("Cannot compute histogram");
            return;
        }

        shader.Dispatch(handleInitialize, 256 / 64, 1, 1);
        // divided by 64 in x because of [numthreads(64,1,1)] in the compute shader code
        shader.Dispatch(handleMain, (inputTexture.width + 7) / 8, (inputTexture.height + 7) / 8, 1);
        // divided by 8 in x and y because of [numthreads(8,8,1)] in the compute shader code

        histogramBuffer.GetData(histogramData);
    }
}