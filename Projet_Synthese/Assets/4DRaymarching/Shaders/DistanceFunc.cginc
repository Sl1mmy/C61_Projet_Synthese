// SPHERE
// s : radius
float sdSphere(float3 p, float s)
{
    return length(p) - s;
}
// BOX
// s : size of box 
float sdBox(float3 p, float3 s)
{
    float3 q = abs(p) - s;
    return length(max(q, 0.0f)) + min(max(q.x, max(q.y, q.z)), 0.0f);
}

// HYPERSPHERE
float sdHyperSphere(float4 p, float s)
{
    return length(p) - s;
}

// HYPERCUBE
float sdHyperCube(float4 p, float4 s)
{
    float4 q = abs(p) - s;
    return min(max(q.x, max(q.y, max(q.z, q.w))), 0.0f) + length(max(q, 0.0f));
}

// DUOCYLINDER
float sdDuoCylinder(float4 p, float h, float r)
{
            
    float2 d = abs(float2(length(p.xz), length(p.yw))) - float2(h, r);
    return min(max(d.x, d.y), 0.0f) + length(max(d, 0.0f));
}

// HYPERCONE
float sdHyperCone(float4 p, float4 h)
{
    return max(length(p.xzw) - h.x, abs(p.y) - h.y) - (h.x * p.y);
}

float sdHyperPlane(float4 p, float4 s)
{
    float plane = dot(p, normalize(float4(0, 1, 0, 0))) - (sin(p.x * s.x + p.w) + sin(p.z * s.z) + sin((p.x * 0.34 + p.z * 0.21) * s.w)) / s.y;
    return plane;

}

// CAPSULE
float sdVerticalCapsule(float3 p, float h, float r)
{
    p.y -= clamp(p.y, 0.0, h);
    return length(p) - r;
}

//OPERATEURS BOOLEENS

float4 Blend(float a, float b, float3 colA, float3 colB, float k)
{
    float h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
    float blendDst = lerp(b, a, h) - k * h * (1.0 - h);
    float3 blendCol = lerp(colB, colA, h);
    return float4(blendCol, blendDst);
}

float4 Combine(float distanceA, float distanceB, float3 colorA, float3 colorB, int operation, float k)
{
    float distance = distanceA;
    float3 color = colorA;

    //union
    if (operation == 0)
    {
        float h = clamp(0.5 + 0.5 * (distanceA - distanceB) / k, 0.0, 1.0);
        distance = lerp(distanceA, distanceB, h) - k * h * (1.0 - h);
        color = lerp(colorA, colorB, h);
    }
    // Blend
    else if (operation == 1)
    {
        float4 blend = Blend(distanceA, distanceB, colorA, colorB, k);
        distance = blend.w;
        color = blend.xyz;
    }
    // substract
    else if (operation == 2)
    {
        float h = clamp(0.5 - 0.5 * (distanceB + distanceA) / k, 0.0, 1.0);
        color = lerp(colorA, colorB, h);
        distance = lerp(distanceA, -distanceB, h) + k * h * (1.0 - h);
    }
    // intersect
    else if (operation == 3)
    {
        float h = clamp(0.5 - 0.5 * (distanceA - distanceB) / k, 0.0, 1.0);
        color = lerp(colorA, colorB, h);
        distance = lerp(distanceA, distanceB, h) + k * h * (1.0 - h);
    }

    return float4(color, distance);
}