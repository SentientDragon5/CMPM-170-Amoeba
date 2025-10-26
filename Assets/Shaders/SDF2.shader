Shader "Custom/SDF2"
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

            StructuredBuffer<float4> _Points;
            int _PointCount;

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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _BackgroundColor;
                bool isFilled = false;

                for (int j = 0; j < _PointCount; j++)
                {
                    float2 pos = _Points[j].xy; // Position from (x, y)
                    float radius = _Points[j].w; // Radius from (w)

                    if (distance(i.uv, pos) < radius)
                    {
                        col = _CircleColor; 
                        isFilled = true;
                        break;
                    }
                }

                if (!isFilled)
                {
                    for (int j = 0; j < _PointCount; j++)
                    {
                        float2 pos = _Points[j].xy;
                        float radius = _Points[j].w;

                        if (distance(i.uv, pos) < radius + _OutlineThickness)
                        {
                            col = _OutlineColor;
                            break;
                        }
                    }
                }
                
                clip(col.a - 0.1);
                
                return col;
            }
            ENDCG
        }
    }
}

