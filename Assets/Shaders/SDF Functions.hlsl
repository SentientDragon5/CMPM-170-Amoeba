void CircleSDF_float(float2 UV, float Radius, out float Dist)
{
    Dist = length(UV) - Radius;
}

void OutlineSDF_float(float Distance, float Thickness, out float Dist)
{
    Dist = abs(Distance) - Thickness;
}