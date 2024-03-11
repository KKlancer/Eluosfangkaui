Shader "Unlit/box"
{
    Properties
    {
        _MainCol("MainColor",color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _EmissIntensity("EmissTexIntensity",float) = 1
        _EmissTex("_EmissTex", 2D) = "black" {}
        _BakeTexIntensity("BakeTexIntensity",float) = 1
        _BakeTex("BakeTex",2D) = "black"{}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
                
            };

            fixed4 _MainCol;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _EmissIntensity;
            sampler2D _EmissTex;
            float4 _EmissTex_ST;
            sampler2D _BakeTex;
            float4 _BakeTex_ST;
            float _BakeTexIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2,_BakeTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2D(_MainTex, i.uv) * _MainCol;
                fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT * col;
                float4 emiss = tex2D(_EmissTex, i.uv) * _EmissIntensity;
                fixed4 bakeCol = tex2D(_BakeTex,i.uv2)*_BakeTexIntensity;
                fixed4 finalCol= ambient + (bakeCol * col) + emiss;
                
                UNITY_APPLY_FOG(i.fogCoord, finalCol);
                return finalCol;
            }
            ENDCG
        }
    }
}
