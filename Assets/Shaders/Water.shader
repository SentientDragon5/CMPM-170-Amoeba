Shader "Custom/VoronoiFoamWater"
{
    Properties
    {
        _BlueColor ("Water Color", Color) = (0.1, 0.2, 0.8, 1.0)
        _FoamColor ("Foam Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Scale ("Scale", Float) = 10.0
        _FoamWidth ("Foam Width", Range(0.01, 0.5)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            fixed4 _BlueColor;
            fixed4 _FoamColor;
            float _Scale;
            float _FoamWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; 
                return o;
            }

            float2 rand2(float2 p)
            {
                return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
            }

            float voronoi_edge(float2 uv)
            {
                float2 cell = floor(uv);
                float2 frac_uv = frac(uv);

                float minDist1 = 1.0;
                float minDist2 = 1.0;

                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 neighborCell = float2(x, y);
                        float2 featurePoint = neighborCell + rand2(cell + neighborCell);
                        float dist = length(featurePoint - frac_uv);

                        if (dist < minDist1)
                        {
                            minDist2 = minDist1;
                            minDist1 = dist;
                        }
                        else if (dist < minDist2)
                        {
                            minDist2 = dist;
                        }
                    }
                }
                
                return minDist2 - minDist1;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * _Scale;
                
                float v_edge = voronoi_edge(uv);
                
                float foam = 1.0 - smoothstep(0.0, _FoamWidth, v_edge);
                
                fixed4 col = lerp(_BlueColor, _FoamColor, foam);
                return col;
            }
            ENDCG
        }
    }
}

