Shader "Simple Toon/SToon Animate"
{
	Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _AnimMap ("AnimMap", 2D) ="white" {}
		_AnimLen("Anim Length", Float) = 0
    	[PerRendererData] _OffsetAnim("Anim Offset", Float) = 0

        [Header(Colorize)][Space(5)]  //colorize
		_Color ("Color", COLOR) = (1,1,1,1)

		[HideInInspector] _ColIntense ("Intensity", Range(0,3)) = 1
        [HideInInspector] _ColBright ("Brightness", Range(-1,1)) = 0
		_AmbientCol ("Ambient", Range(0,1)) = 0

        [Header(Detail)][Space(5)]  //detail
        [Toggle] _Segmented ("Segmented", Float) = 1
        _Steps ("Steps", Range(1,25)) = 3
        _StpSmooth ("Smoothness", Range(0,1)) = 0
        _Offset ("Lit Offset", Range(-1,1.1)) = 0

        [Header(Light)][Space(5)]  //light
        [Toggle] _Clipped ("Clipped", Float) = 0
        _MinLight ("Min Light", Range(0,1)) = 0
        _MaxLight ("Max Light", Range(0,1)) = 1
        _Lumin ("Luminocity", Range(0,2)) = 0

        [Header(Shine)][Space(5)]  //shine
		[HDR] _ShnColor ("Color", COLOR) = (1,1,0,1)
        [Toggle] _ShnOverlap ("Overlap", Float) = 0

        _ShnIntense ("Intensity", Range(0,1)) = 0
        _ShnRange ("Range", Range(0,1)) = 0.15
        _ShnSmooth ("Smoothness", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
        Pass
        {
            Name "DirectLight"
            LOD 80

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"
            #include "STCore.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                LIGHTING_COORDS(0,1)
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                half3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD2;
            	float4 worldPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(MyProperties)
            UNITY_DEFINE_INSTANCED_PROP (float, _OffsetAnim)
            UNITY_INSTANCING_BUFFER_END(MyProperties)
            
            sampler2D _AnimMap;
            float4 _AnimMap_TexelSize;//x == 1/width

            float4 ColorBack;
            float3 TargetPos;
            float FallofRadius;
            float TargetRadius;

            float GlobalSpeed;

            float _AnimLen;

            v2f vert (appdata v, uint vid : SV_VertexID)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                float f = _Time.y;

            	f -= UNITY_ACCESS_INSTANCED_PROP (MyProperties, _OffsetAnim);
                f /= _AnimLen;
            	f *= max(GlobalSpeed, 1.0);

                fmod(f, 1.0);

                float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;
                float animMap_y = f;

                float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y,0,0));
                
                v2f o;
                o.pos = UnityObjectToClipPos(pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
            	o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

			fixed4 frag (v2f i) : SV_Target
            {
                _MaxLight = max(_MinLight, _MaxLight);
                _Steps = _Segmented ? _Steps : 1;
                _StpSmooth = _Segmented ? _StpSmooth : 1;

				_DarkColor = fixed4(0,0,0,1);
				_MaxAtten = 1.0;

				float3 normal = normalize(i.worldNormal);
				float3 light_dir = normalize(_WorldSpaceLightPos0.xyz);
				float3 view_dir = normalize(i.viewDir);
				float3 halfVec = normalize(light_dir + view_dir);
				float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));

                float NdotL = dot(normal, light_dir);
				float NdotH = dot(normal, halfVec);
				float VdotN = dot(view_dir, normal);
				float FdotV = dot(forward, -view_dir);

                fixed atten = SHADOW_ATTENUATION(i);
                float toon = Toon(NdotL, atten);

				fixed4 shadecol = _DarkColor;
				fixed4 litcol = ColorBlend(_Color, _LightColor0, _AmbientCol);
				fixed4 texcol = tex2D(_MainTex, i.uv) * litcol * _ColIntense + _ColBright;

				float4 blendCol = ColorBlend(shadecol, texcol, toon);
				float4 postCol = PostEffects(blendCol, toon, atten, NdotL, NdotH, VdotN, FdotV);

				postCol.a = 1.;

            	float distance = length(i.worldPos - TargetPos);
            	float dist = clamp01(exp(-FallofRadius*(distance-TargetRadius)));

            	float4 fog = lerp(ColorBack, postCol, dist);
            	
				return fog;
            }

            ENDCG
        }

    	Pass
    	{
    		Tags {"LightMode" = "ShadowCaster"}
    		
    		CGPROGRAM
			#pragma vertex vert
    		#pragma fragment frag
    		#pragma multi_compile_shadowcaster
    		#pragma multi_compile_instancing
    		
    		#include "UnityCG.cginc"

    		struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

    		struct v2f { 
                V2F_SHADOW_CASTER;
            };

    		sampler2D _AnimMap;
            float4 _AnimMap_TexelSize;

            float _AnimLen;
    		float GlobalSpeed;

    		UNITY_INSTANCING_BUFFER_START(MyProperties)
            UNITY_DEFINE_INSTANCED_PROP (float, _OffsetAnim)
            UNITY_INSTANCING_BUFFER_END(MyProperties)

            v2f vert(appdata v, uint vid : SV_VertexID)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                float f = _Time.y;

            	f -= UNITY_ACCESS_INSTANCED_PROP (MyProperties, _OffsetAnim);
                f /= _AnimLen;
    			f *= max(GlobalSpeed, 1.0);

                fmod(f, 1.0);

                float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;
                float animMap_y = f;

                float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));
    			
                v2f o;
            	o.pos = UnityObjectToClipPos(pos);
				o.pos = UnityApplyLinearShadowBias(o.pos);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
    		ENDCG
    	}
    }
}