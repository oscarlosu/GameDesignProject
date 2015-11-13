// Shader created with Shader Forge v1.16 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.16;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-7783-RGB,alpha-750-OUT;n:type:ShaderForge.SFN_Tex2d,id:7783,x:32203,y:32909,varname:node_7783,prsc:2,tex:b6c96aea5b97d8546aca9cd8393bfc1b,ntxv:0,isnm:False|UVIN-8364-OUT,TEX-7736-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:7736,x:32007,y:33002,ptovrint:False,ptlb:StarTexture,ptin:_StarTexture,varname:node_7736,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b6c96aea5b97d8546aca9cd8393bfc1b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ComponentMask,id:750,x:32417,y:33046,varname:node_750,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-7783-RGB;n:type:ShaderForge.SFN_FragmentPosition,id:6457,x:31680,y:32797,varname:node_6457,prsc:2;n:type:ShaderForge.SFN_Append,id:8364,x:31949,y:32810,varname:node_8364,prsc:2|A-6457-X,B-6457-Y;proporder:7736;pass:END;sub:END;*/

Shader "Shader Forge/Stars" {
    Properties {
        _StarTexture ("StarTexture", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _StarTexture; uniform float4 _StarTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float2 node_8364 = float2(i.posWorld.r,i.posWorld.g);
                float4 node_7783 = tex2D(_StarTexture,TRANSFORM_TEX(node_8364, _StarTexture));
                float3 emissive = node_7783.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,node_7783.rgb.r);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
