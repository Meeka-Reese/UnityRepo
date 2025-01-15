#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

static float2 sobelSamplePoints[9] =
{
    float2(-1, 1), float2(0, 1), float2(1, 1),
    float2(-1, 0), float2(0, 0), float2(1, 0),
    float2(-1, -1), float2(0, -1), float2(1, -1)
}; // Removed trailing comma here

static float sobelMatrix[9] =
{
    1, 0, -1,
    2, 0, -2,
    1, 0, -1
};

static float sobelYMatrix[9] =
{
    1, 2, 1,
    0, 0, 0,
    -1, -2, -1
};

void DepthSobel_float(float2 UV, float Thickness, out float Out)
{
    float2 sobel = 0;
    [unroll] for (int i = 0; i < 9; i++)
    {
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
        sobel += depth * float2(sobelMatrix[i], sobelYMatrix[i]); // Changed to float2 for gradient
    }
    Out = length(sobel);
}
#endif
