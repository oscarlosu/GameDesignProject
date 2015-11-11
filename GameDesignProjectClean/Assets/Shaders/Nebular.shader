// Shader created with Shader Forge v1.16 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.16;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:True;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-4446-OUT;n:type:ShaderForge.SFN_Tex2d,id:8671,x:31703,y:32503,varname:sdsds,prsc:2,tex:faf68c6dfbb1522428bfe4b225e9e8e2,ntxv:0,isnm:False|TEX-984-TEX;n:type:ShaderForge.SFN_Color,id:7383,x:31789,y:32684,ptovrint:False,ptlb:NebularColor,ptin:_NebularColor,varname:_NebularColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3542388,c2:0.4080241,c3:0.7647059,c4:1;n:type:ShaderForge.SFN_Multiply,id:3940,x:31945,y:32538,varname:node_3940,prsc:2|A-8671-RGB,B-7383-RGB;n:type:ShaderForge.SFN_Tex2dAsset,id:984,x:31405,y:32631,ptovrint:False,ptlb:Nebula,ptin:_Nebula,varname:_Nebula,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:faf68c6dfbb1522428bfe4b225e9e8e2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9497,x:31628,y:32859,varname:_node_9497,prsc:2,tex:faf68c6dfbb1522428bfe4b225e9e8e2,ntxv:0,isnm:False|UVIN-9952-OUT,TEX-984-TEX;n:type:ShaderForge.SFN_TexCoord,id:1320,x:30668,y:32829,varname:node_1320,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:8290,x:30881,y:32829,varname:node_8290,prsc:2|A-3190-OUT,B-1320-U;n:type:ShaderForge.SFN_Vector1,id:3190,x:30819,y:32762,varname:node_3190,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Vector1,id:4333,x:30601,y:33073,varname:node_4333,prsc:2,v1:0.3;n:type:ShaderForge.SFN_Add,id:1851,x:30796,y:33007,varname:node_1851,prsc:2|A-1320-V,B-4333-OUT;n:type:ShaderForge.SFN_Multiply,id:3161,x:31016,y:32607,varname:node_3161,prsc:2|A-1805-OUT,B-8290-OUT;n:type:ShaderForge.SFN_Vector1,id:1805,x:30767,y:32577,varname:node_1805,prsc:2,v1:0.6;n:type:ShaderForge.SFN_Append,id:9952,x:31156,y:32824,varname:node_9952,prsc:2|A-3161-OUT,B-1851-OUT;n:type:ShaderForge.SFN_Color,id:4589,x:31720,y:33105,ptovrint:False,ptlb:NebularColor2,ptin:_NebularColor2,varname:_NebularColor2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.7058823,c2:0.2491349,c3:0.2491349,c4:1;n:type:ShaderForge.SFN_Multiply,id:3144,x:31937,y:32951,varname:node_3144,prsc:2|A-9497-RGB,B-4589-RGB;n:type:ShaderForge.SFN_Lerp,id:4446,x:32212,y:32783,varname:node_4446,prsc:2|A-3940-OUT,B-3144-OUT,T-3255-OUT;n:type:ShaderForge.SFN_Vector1,id:3255,x:32086,y:32978,varname:node_3255,prsc:2,v1:0.5;proporder:7383-984-4589;pass:END;sub:END;*/

Shader "Shader Forge/Nebular" {
    Properties {
        _NebularColor ("NebularColor", Color) = (0.3542388,0.4080241,0.7647059,1)
        _Nebula ("Nebula", 2D) = "white" {}
        _NebularColor2 ("NebularColor2", Color) = (0.7058823,0.2491349,0.2491349,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform float4 _NebularColor;
            uniform sampler2D _Nebula; uniform float4 _Nebula_ST;
            uniform float4 _NebularColor2;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 sdsds = tex2D(_Nebula,TRANSFORM_TEX(i.uv0, _Nebula));
                float2 node_9952 = float2((0.6*(0.1+i.uv0.r)),(i.uv0.g+0.3));
                float4 _node_9497 = tex2D(_Nebula,TRANSFORM_TEX(node_9952, _Nebula));
                float3 emissive = lerp((sdsds.rgb*_NebularColor.rgb),(_node_9497.rgb*_NebularColor2.rgb),0.5);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
