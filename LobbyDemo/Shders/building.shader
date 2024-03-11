Shader "Unlit/building"
{
    Properties
    {
        _MainCol("MainColor",color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _EmissIntensity("EmissTexIntensity",float) = 1
        _EmissTex("_EmissTex", 2D) = "black" {}
        _BakeTexIntensity("BakeTexIntensity",float) = 1
        _BakeTex("BakeTex",2D) = "black"{}
        _SmoothnessTex("SmoothnessTex",2D)="white"{}
        _SmoothnessIntensity("SmoothnessIntensity",float)=0.5
        _CubeTex("CubeTex",cube)=""{}
        _fogStart("fogStart", Float) = 0
		_fogEnd("fogEnd", Float) = 700
        _HighFogStart("HighFogStart", Float) = 0
		_HighFogEnd("HighFogEnd", Float) = 700
        _HighFogColor("FogColor",color)=(1,1,1,1)
        _FogPartTex("FogPartTex",2D)="white" {}
        _SunRange("SunRange", Float) = 0
		_sunPow("sunPow", Float) = 0
        //_FogNoiesTex("FogNoiesTex",2d)="white" {}
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

            #include"UnityCG.cginc"
            #include"Lighting.cginc"
			//#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normal:NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 world_pos:TEXCOORD2;
                float3 word_normal:TEXCOORD3;
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

            uniform sampler2D _SmoothnessTex;
            uniform float _SmoothnessIntensity;
            uniform samplerCUBE _CubeTex;
            uniform float4 _CubeTex_HDR;

            uniform float _HighFogEnd;
		    uniform float _HighFogStart;
            uniform float4 _HighFogColor;
            uniform float _fogStart;
            uniform float _fogEnd;
            uniform sampler2D _FogPartTex;
            uniform float4 _FogPartTex_ST;

            uniform float _SunRange;
            uniform float _sunPow;
            //uniform sampler2D _FogNoiesTex;
            //uniform float4 _FogNoiesTex_ST;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world_pos=mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2,_BakeTex);
                o.word_normal=normalize(UnityObjectToWorldNormal(v.normal));
                return o; 
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 world_normal=normalize(i.word_normal);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.world_pos);
                float3 worldSpaceLightDir = UnityWorldSpaceLightDir(i.world_pos);
            //距离雾
                float CamDistance=1-clamp(((_fogEnd-distance(i.world_pos,_WorldSpaceCameraPos))/(_fogEnd-_fogStart)),0.0,1.0);
                fixed4 DistanceFog=CamDistance*_HighFogColor;
                //高度雾
                float Highfog=clamp( ( ( _HighFogEnd - i.world_pos.y ) / ( _HighFogEnd - _HighFogStart ) ) , 0.0 , 1.0 );

                //太阳雾
                float dotResult5 = dot( -viewDir , worldSpaceLightDir );
				float clampResult24 = clamp( pow( (dotResult5*0.5 + 0.5) , _SunRange ) , 0.0 , 1.0 );
				float SunFog38 = ( clampResult24 * _sunPow );
				

                 //FogNoies
                //fixed FogNoies=saturate(tex2D(_FogNoiesTex,i.uv2).r+0.3);
                //区域雾色
                fixed4 FogPartColor=tex2D(_FogPartTex,i.uv2)*_HighFogColor;
                //final fog color
                float4 FinalFogCol = lerp(FogPartColor,_LightColor0 , SunFog38);

                //ambient
                fixed4 col = tex2D(_MainTex, i.uv) * _MainCol;
                fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT * col;
                //自发光
                float4 emiss = tex2D(_EmissTex, i.uv) * _EmissIntensity;
                //烘焙贴图
                fixed4 bakeCol = tex2D(_BakeTex,i.uv2)*_BakeTexIntensity;
                //反射

                float smoothness=tex2D(_SmoothnessTex, i.uv).r*_SmoothnessIntensity;
                float3 view_reflect=normalize(reflect(-viewDir, world_normal));
                float4 Cube=texCUBE(_CubeTex, view_reflect)*smoothness;
                fixed3 CubeColor=DecodeHDR(Cube, _CubeTex_HDR);
                //物体色总和
                fixed4 ambientColor= ambient + (bakeCol * col) + emiss+fixed4(CubeColor,1);
                //fog总和
                fixed4 finalColor=lerp(ambientColor,FinalFogCol,Highfog*DistanceFog);
                //最终
                return finalColor;
            }
            ENDCG
        }
    }
}