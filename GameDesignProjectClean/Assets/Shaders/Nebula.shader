// Shader created with Shader Forge v1.16 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.16;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:33129,y:32666,varname:node_3138,prsc:2|emission-2155-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:6306,x:32083,y:32706,ptovrint:False,ptlb:NebulaClouds1,ptin:_NebulaClouds1,varname:node_6306,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a518279e23b41ee4485d02e1932f0d18,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:9310,x:32121,y:33092,ptovrint:False,ptlb:NebulaClouds2,ptin:_NebulaClouds2,varname:_NebulaClouds_copy,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a518279e23b41ee4485d02e1932f0d18,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6784,x:32250,y:32540,varname:node_6784,prsc:2,tex:a518279e23b41ee4485d02e1932f0d18,ntxv:0,isnm:False|UVIN-4135-OUT,TEX-6306-TEX;n:type:ShaderForge.SFN_Tex2d,id:5815,x:32310,y:32920,varname:node_5815,prsc:2,tex:a518279e23b41ee4485d02e1932f0d18,ntxv:0,isnm:False|UVIN-1392-OUT,TEX-9310-TEX;n:type:ShaderForge.SFN_Color,id:7428,x:32258,y:32682,ptovrint:False,ptlb:NebColor1,ptin:_NebColor1,varname:node_7428,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4411765,c2:0.178417,c3:0.178417,c4:1;n:type:ShaderForge.SFN_Color,id:9252,x:32323,y:33077,ptovrint:False,ptlb:NebColor2,ptin:_NebColor2,varname:node_9252,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2975778,c2:0.4799726,c3:0.6323529,c4:1;n:type:ShaderForge.SFN_Multiply,id:5638,x:32510,y:32641,varname:node_5638,prsc:2|A-6784-RGB,B-7428-RGB;n:type:ShaderForge.SFN_Multiply,id:5841,x:32556,y:32890,varname:node_5841,prsc:2|A-5815-RGB,B-9252-RGB;n:type:ShaderForge.SFN_Add,id:2155,x:32834,y:32794,varname:node_2155,prsc:2|A-5638-OUT,B-5841-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:50,x:31687,y:32467,varname:node_50,prsc:2;n:type:ShaderForge.SFN_Append,id:7710,x:31852,y:32467,varname:node_7710,prsc:2|A-50-X,B-50-Y;n:type:ShaderForge.SFN_Append,id:1392,x:31979,y:32876,varname:node_1392,prsc:2|A-9194-OUT,B-4118-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:3318,x:31446,y:32790,varname:node_3318,prsc:2;n:type:ShaderForge.SFN_Add,id:4118,x:31759,y:32926,varname:node_4118,prsc:2|A-3318-Y,B-7847-T;n:type:ShaderForge.SFN_Time,id:7847,x:31557,y:32589,varname:node_7847,prsc:2;n:type:ShaderForge.SFN_Add,id:4135,x:32057,y:32508,varname:node_4135,prsc:2|A-7710-OUT,B-7847-T;n:type:ShaderForge.SFN_Add,id:9194,x:31759,y:32747,varname:node_9194,prsc:2|A-7847-TSL,B-3318-X;proporder:6306-7428-9310-9252;pass:END;sub:END;*/

Shader "Shader Forge/Nebula" {
    Properties {
        _NebulaClouds1 ("NebulaClouds1", 2D) = "white" {}
        _NebColor1 ("NebColor1", Color) = (0.4411765,0.178417,0.178417,1)
        _NebulaClouds2 ("NebulaClouds2", 2D) = "white" {}
        _NebColor2 ("NebColor2", Color) = (0.2975778,0.4799726,0.6323529,1)
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
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _NebulaClouds1; uniform float4 _NebulaClouds1_ST;
            uniform sampler2D _NebulaClouds2; uniform float4 _NebulaClouds2_ST;
            uniform float4 _NebColor1;
            uniform float4 _NebColor2;
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
                float4 node_7847 = _Time + _TimeEditor;
                float2 node_4135 = (float2(i.posWorld.r,i.posWorld.g)+node_7847.g);
                float4 node_6784 = tex2D(_NebulaClouds1,TRANSFORM_TEX(node_4135, _NebulaClouds1));
                float2 node_1392 = float2((node_7847.r+i.posWorld.r),(i.posWorld.g+node_7847.g));
                float4 node_5815 = tex2D(_NebulaClouds2,TRANSFORM_TEX(node_1392, _NebulaClouds2));
                float3 emissive = ((node_6784.rgb*_NebColor1.rgb)+(node_5815.rgb*_NebColor2.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
