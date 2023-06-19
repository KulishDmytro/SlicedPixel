Shader "Unlit/SlicedPixel_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelsPerUnit ("PixelsPerUnit", Int) = 100
        _Color ("Color", Color) = (1,1,1,1)
        _ColorIntensity ("ColorIntensity", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float _ColorIntensity;
            float _PixelsPerUnit;
            
            StructuredBuffer<float2> _MvBuffer;
            int _MvBufferSize;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv0 : TEXCOORD0;
                float3 vcol : COLOR0;
                float4 vertex : SV_POSITION;
            };
            
            float map(float s, float a1, float a2, float b1, float b2)
            {
                return b1 + (s-a1)*(b2-b1)/(a2-a1);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                // Fetch noise from uv1 green channel
                half noise = map(v.uv1.g, 0.0f, 1.0f, 0.5f, 1.5f);
                
                // Fetch Motion Vector from Buffer and multiply by noise
                // for slightly randomizing vertex offset
                float2 motionVec = _MvBuffer[v.uv1.r * _MvBufferSize].rg * noise;
                
                // Add Motion Vector to vertex position 
                // floor() is used for pixel perfect movement
                v.vertex.rg += floor(motionVec.rg)*0.01f;
                
                // Create distance mask from Motion Vector
                half distMask = abs(motionVec.r) + abs(motionVec.y);
                
                // Slightly offset in Z coord to avoid Z-fighting
                v.vertex.b -= distMask * 0.0001f;
                
                // Sample Texture by Pixel Center stored in uv0
                o.vcol = tex2Dlod(_MainTex, float4(v.uv0 / (_MainTex_TexelSize.zw / _PixelsPerUnit), 0.0f, 0.0f)).rgb;
                
                // Change color based on distance mask
                o.vcol = lerp(o.vcol, _Color.rgb, map(distMask, 0.0f, 1.0f, 0.0f, _ColorIntensity * 10.0f));
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed3 frag (v2f i) : SV_Target
            {
                return i.vcol;
            }
            ENDCG
        }
    }
}
