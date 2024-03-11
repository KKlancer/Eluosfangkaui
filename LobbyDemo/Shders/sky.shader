// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "sky"
{
	Properties
	{
		_HighFogStart("HighFogStart", Float) = 0
		_HighFogEnd("HighFogEnd", Float) = 700
		_FogColor("FogColor", Color) = (0.5566038,0.5566038,0.5566038,0)
		_SunRange("SunRange", Float) = 0
		_sunPow("sunPow", Float) = 0
		_Color1("Color 1", Color) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Front
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _FogColor;
			uniform float4 _Color1;
			uniform float _SunRange;
			uniform float _sunPow;
			uniform float _HighFogEnd;
			uniform float _HighFogStart;
			float3 ACESToneMap55( float3 LinerColor )
			{
				float a = 2.51f;
				float b = 0.03f;
				float c = 2.43f;   
				float d = 0.59f;
				float e = 0.15f;
				return saturate((LinerColor*(a*LinerColor + b)) / (LinerColor*(c*LinerColor + d) + e));
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float3 WorldPosition = i.worldPos;
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(WorldPosition);
				float dotResult5 = dot( -ase_worldViewDir , worldSpaceLightDir );
				float clampResult24 = clamp( pow( (dotResult5*0.5 + 0.5) , _SunRange ) , 0.0 , 1.0 );
				float SunFog38 = ( clampResult24 * _sunPow );
				float4 lerpResult50 = lerp( ( _FogColor + _FogColor) , _Color1 , SunFog38);
				float temp_output_8_0_g5 = _HighFogEnd;
				float clampResult4_g5 = clamp( ( ( temp_output_8_0_g5 - WorldPosition.y ) / ( temp_output_8_0_g5 - _HighFogStart ) ) , 0.0 , 1.0 );
				float4 lerpResult51 = lerp( tex2D( _MainTex, uv_MainTex ) , lerpResult50 , saturate( ( 1.0 - ( 1.0 - clampResult4_g5 ) ) ));
				float3 LinerColor55 = ( lerpResult51 * lerpResult51 ).rgb;
				float3 localACESToneMap55 = ACESToneMap55( LinerColor55 );
				
				
				finalColor = float4( sqrt( localACESToneMap55 ) , 0.0 );
				return finalColor;
			}
			ENDCG
		}
	}
	
}