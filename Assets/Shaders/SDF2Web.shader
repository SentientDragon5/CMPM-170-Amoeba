Shader "Custom/SDF2_WebGL"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0,0,0,0)
        _CircleColor ("Circle Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define MAX_POINTS 128
            uniform float4 _Points[MAX_POINTS];
            uniform int _PointCount;

            fixed4 _BackgroundColor;
            fixed4 _CircleColor;
            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float smin(float a, float b, float k)
            {
                float h = max(0.0, min(1.0, (b - a + k) / (2.0 * k)));
                return lerp(b, a, h) - k * h * (1.0 - h);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float minDist = 1e20; 
                for (int j = 0; j < MAX_POINTS; j++)
                {
                    if (j >= _PointCount)
                    {
                        break;
                    }

                    float2 pos = _Points[j].xy;
                    float radius = _Points[j].z;
                    float pointBlendStrength = _Points[j].w;
                    
                    float dist = distance(i.uv, pos) - radius;
                    
                    minDist = smin(minDist, dist, pointBlendStrength);
                }
                
                // anti-aliasing
                float crispEdge = 0.001;
                float fill = smoothstep(crispEdge, -crispEdge, minDist);

                float outline = smoothstep(_OutlineThickness + crispEdge, _OutlineThickness - crispEdge, minDist);
                float outlineAmount = saturate(outline - fill);

                fixed4 col = _BackgroundColor;
                col = lerp(col, _OutlineColor, outlineAmount);
                col = lerp(col, _CircleColor, fill);
                
                clip(col.a - 0.1);
                
                return col;
            }
            ENDCG
        }
    }
}