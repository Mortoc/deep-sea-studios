Shader "Custom/Hologram/Grid" {

	Properties {
		_color ("Color", Color) = (0,1,0,1)
        _gridSpacing ("Grid Spacing", Vector) = (1,1,1,0) 
        _gridThickness ("Grid Thickness", Float) = 0.1
    }
    
    SubShader {
        Pass {
        	Offset -1, -1
		    Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		    Blend SrcAlpha OneMinusSrcAlpha
		    Cull Off
		    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 _gridSpacing;
            float4 _color;
            float _gridThickness;

            struct vertOut {
                float4 pos:SV_POSITION;
             	float4 wpos:TEXCOORD1;
            };

            vertOut vert(appdata_base v) {
                vertOut o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.wpos = mul(_Object2World, v.vertex);
                return o;
            }

            float4 frag(vertOut i) : SV_Target {
            	float4 result = _color;
            	
            	float xGrid = 1 - step(_gridThickness, abs(sin(i.wpos.x / _gridSpacing.x)));
            	float yGrid = 1 - step(_gridThickness, abs(sin(i.wpos.y / _gridSpacing.y)));
            	float zGrid = 1 - step(_gridThickness, abs(sin(i.wpos.z / _gridSpacing.z)));
            	
            	result.a = saturate(xGrid + yGrid + zGrid);
            		
            	return result;
            }

            ENDCG
        }
    }
}