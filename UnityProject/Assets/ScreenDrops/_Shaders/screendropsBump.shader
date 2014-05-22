Shader "ScreenDrops/Bump" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {} 
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _Alpha ("Alpha", float) = 0.25
        _Influence ("Influence", float) = 15.0
        _Bumpiness ("Bumpiness", float ) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200 
		
		Cull Front
		
		ZWrite Off
		ZTest Always
		
		Blend One One
		
		CGPROGRAM
		#pragma surface surf CustomLambert noambient

		sampler2D _MainTex; 
		sampler2D _BumpMap; 
		float _Alpha; 
		float _Bumpiness;
		float _Influence;
		 
		struct EditorSurfaceOutput {
		    half3 Albedo;
		    half3 Normal;
		    half3 Emission;
		    half Specular;
		    half Gloss;
		    half Alpha; 
		}; 

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		}; 
		
		
		half4 LightingCustomLambert (EditorSurfaceOutput s, half3 lightDir, half atten) { 
	        half NdotL = pow(max(0, dot (s.Normal, lightDir)),_Influence); 
	        half4 c;
	        c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
	        c.a = s.Alpha;
	        return c;
	    }

		void surf (Input IN, inout EditorSurfaceOutput o) {
			half4 texColor = tex2D (_MainTex, IN.uv_MainTex); 			
			
			o.Albedo = texColor.rgb*_Alpha;
			
			//UnpackNormal
			float4 encodedNormal = float4(1,1,1,1) - tex2D(_BumpMap, IN.uv_BumpMap);
            float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0), 0.0);
            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords))/_Bumpiness;
            
            o.Normal = normalize(localCoords);
		}
		ENDCG
	} 
	Fallback "VertexLit"
}
