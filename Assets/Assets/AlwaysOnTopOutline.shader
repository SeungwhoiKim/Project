Shader "Custom/AlwaysOnTopOutline"
{
    Properties{
        _OutlineColor("Outline Color", Color) = (1,1,0,1)
        _OutlineThickness("Outline Thickness", Range(0.0, 0.1)) = 0.02
    }
        SubShader{
            Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }
            Cull Front
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);
                v.vertex.xyz += norm * _OutlineThickness;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}
