// Shader created with Shader Forge v1.16 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.16;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:33129,y:32666,varname:node_3138,prsc:2|emission-2155-OUT,alpha-4836-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:6306,x:32016,y:32654,ptovrint:False,ptlb:NebulaClouds1,ptin:_NebulaClouds1,varname:node_6306,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6784,x:32250,y:32540,varname:node_6784,prsc:2,ntxv:0,isnm:False|UVIN-7710-OUT,TEX-6306-TEX;n:type:ShaderForge.SFN_Color,id:7428,x:32258,y:32682,ptovrint:False,ptlb:NebColor1,ptin:_NebColor1,varname:node_7428,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4411765,c2:0.178417,c3:0.178417,c4:1;n:type:ShaderForge.SFN_Color,id:9252,x:32242,y:33091,ptovrint:False,ptlb:NebColor2,ptin:_NebColor2,varname:node_9252,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2975778,c2:0.4799726,c3:0.6323529,c4:1;n:type:ShaderForge.SFN_Multiply,id:5638,x:32510,y:32641,varname:node_5638,prsc:2|A-6784-RGB,B-7428-RGB;n:type:ShaderForge.SFN_Multiply,id:5841,x:32556,y:32890,varname:node_5841,prsc:2|A-1020-RGB,B-9252-RGB;n:type:ShaderForge.SFN_Add,id:2155,x:32834,y:32794,varname:node_2155,prsc:2|A-5638-OUT,B-5841-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:50,x:31153,y:32313,varname:node_50,prsc:2;n:type:ShaderForge.SFN_Append,id:7710,x:32002,y:32480,varname:node_7710,prsc:2|A-9990-OUT,B-2317-OUT;n:type:ShaderForge.SFN_Append,id:1392,x:32027,y:32881,varname:node_1392,prsc:2|A-9194-OUT,B-4118-OUT;n:type:ShaderForge.SFN_Add,id:4118,x:31812,y:32943,varname:node_4118,prsc:2|A-50-Y,B-6779-OUT;n:type:ShaderForge.SFN_Time,id:7847,x:31074,y:32600,varname:node_7847,prsc:2;n:type:ShaderForge.SFN_Add,id:9194,x:31792,y:32750,varname:node_9194,prsc:2|A-8542-OUT,B-50-X,C-1495-OUT;n:type:ShaderForge.SFN_Add,id:1191,x:32591,y:33028,varname:node_1191,prsc:2|A-6784-RGB,B-1020-RGB;n:type:ShaderForge.SFN_ComponentMask,id:7131,x:32797,y:33028,varname:node_7131,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-1191-OUT;n:type:ShaderForge.SFN_Slider,id:1073,x:32659,y:33229,ptovrint:False,ptlb:Transparency Modifier,ptin:_TransparencyModifier,varname:node_1073,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:0.2;n:type:ShaderForge.SFN_Multiply,id:4276,x:31465,y:32299,varname:node_4276,prsc:2|A-412-OUT,B-7847-T;n:type:ShaderForge.SFN_Add,id:9990,x:31786,y:32375,varname:node_9990,prsc:2|A-50-X,B-4276-OUT;n:type:ShaderForge.SFN_Add,id:2317,x:31800,y:32588,varname:node_2317,prsc:2|A-50-Y,B-8073-OUT;n:type:ShaderForge.SFN_Multiply,id:8073,x:31465,y:32490,varname:node_8073,prsc:2|A-6110-OUT,B-7847-T;n:type:ShaderForge.SFN_Multiply,id:8542,x:31465,y:32642,varname:node_8542,prsc:2|A-3803-OUT,B-7847-T;n:type:ShaderForge.SFN_Multiply,id:6779,x:31465,y:32879,varname:node_6779,prsc:2|A-4773-OUT,B-7847-T;n:type:ShaderForge.SFN_ValueProperty,id:412,x:31153,y:32231,ptovrint:False,ptlb:Nebula1 X Offset Modifier,ptin:_Nebula1XOffsetModifier,varname:node_412,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_ValueProperty,id:6110,x:31153,y:32476,ptovrint:False,ptlb:Nebula1 Y Offset Modifier,ptin:_Nebula1YOffsetModifier,varname:_node_412_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:3803,x:31225,y:32745,ptovrint:False,ptlb:Nebula2 X Offset Modifier,ptin:_Nebula2XOffsetModifier,varname:_Nebula1XOffsetModifier_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_ValueProperty,id:4773,x:31225,y:32990,ptovrint:False,ptlb:Nebula2 Y Offset Modifier,ptin:_Nebula2YOffsetModifier,varname:_Nebula1YOffsetModifier_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.4;n:type:ShaderForge.SFN_Subtract,id:4836,x:32948,y:32953,varname:node_4836,prsc:2|A-7131-OUT,B-1073-OUT;n:type:ShaderForge.SFN_Tex2d,id:1020,x:32258,y:32870,varname:node_1020,prsc:2,ntxv:0,isnm:False|UVIN-1392-OUT,TEX-6306-TEX;n:type:ShaderForge.SFN_Vector1,id:1495,x:31632,y:32851,varname:node_1495,prsc:2,v1:0.3;proporder:6306-7428-412-6110-9252-3803-4773-1073;pass:END;sub:END;*/

Shader "Shader Forge/Nebula" {
    Properties {
        _NebulaClouds1 ("NebulaClouds1", 2D) = "white" {}
        _NebColor1 ("NebColor1", Color) = (0.4411765,0.178417,0.178417,1)
        _Nebula1XOffsetModifier ("Nebula1 X Offset Modifier", Float ) = 0.1
        _Nebula1YOffsetModifier ("Nebula1 Y Offset Modifier", Float ) = 0.2
        _NebColor2 ("NebColor2", Color) = (0.2975778,0.4799726,0.6323529,1)
        _Nebula2XOffsetModifier ("Nebula2 X Offset Modifier", Float ) = 0.3
        _Nebula2YOffsetModifier ("Nebula2 Y Offset Modifier", Float ) = 0.4
        _TransparencyModifier ("Transparency Modifier", Range(0, 0.2)) = 0.2
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
            uniform float4 _TimeEditor;
            uniform sampler2D _NebulaClouds1; uniform float4 _NebulaClouds1_ST;
            uniform float4 _NebColor1;
            uniform float4 _NebColor2;
            uniform float _TransparencyModifier;
            uniform float _Nebula1XOffsetModifier;
            uniform float _Nebula1YOffsetModifier;
            uniform float _Nebula2XOffsetModifier;
            uniform float _Nebula2YOffsetModifier;
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
                float2 node_7710 = float2((i.posWorld.r+(_Nebula1XOffsetModifier*node_7847.g)),(i.posWorld.g+(_Nebula1YOffsetModifier*node_7847.g)));
                float4 node_6784 = tex2D(_NebulaClouds1,TRANSFORM_TEX(node_7710, _NebulaClouds1));
                float2 node_1392 = float2(((_Nebula2XOffsetModifier*node_7847.g)+i.posWorld.r+0.3),(i.posWorld.g+(_Nebula2YOffsetModifier*node_7847.g)));
                float4 node_1020 = tex2D(_NebulaClouds1,TRANSFORM_TEX(node_1392, _NebulaClouds1));
                float3 emissive = ((node_6784.rgb*_NebColor1.rgb)+(node_1020.rgb*_NebColor2.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,((node_6784.rgb+node_1020.rgb).r-_TransparencyModifier));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
