// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32837,y:32708,varname:node_3138,prsc:2|emission-4386-OUT,alpha-6181-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31932,y:32489,ptovrint:False,ptlb:CentralColor,ptin:_CentralColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8620689,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:2583,x:31560,y:32800,varname:node_2583,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Subtract,id:5455,x:31792,y:32810,varname:node_5455,prsc:2|A-2583-U,B-1988-OUT;n:type:ShaderForge.SFN_Add,id:816,x:31792,y:32990,varname:node_816,prsc:2|A-1988-OUT,B-2583-U;n:type:ShaderForge.SFN_Ceil,id:8028,x:31954,y:32810,varname:node_8028,prsc:2|IN-5455-OUT;n:type:ShaderForge.SFN_Floor,id:6292,x:31954,y:32990,varname:node_6292,prsc:2|IN-816-OUT;n:type:ShaderForge.SFN_OneMinus,id:3512,x:32108,y:32990,varname:node_3512,prsc:2|IN-6292-OUT;n:type:ShaderForge.SFN_Add,id:5006,x:32281,y:32814,varname:node_5006,prsc:2|A-8028-OUT,B-3512-OUT;n:type:ShaderForge.SFN_Lerp,id:4386,x:32469,y:32814,varname:node_4386,prsc:2|A-9753-OUT,B-4967-RGB,T-5006-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7914,x:31928,y:32651,ptovrint:False,ptlb:EmissiveMultiplier,ptin:_EmissiveMultiplier,varname:node_7914,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:9753,x:32131,y:32489,varname:node_9753,prsc:2|A-7241-RGB,B-7914-OUT;n:type:ShaderForge.SFN_Slider,id:8124,x:32215,y:33174,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_8124,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:1402,x:32516,y:33118,varname:node_1402,prsc:2|A-5006-OUT,B-8124-OUT;n:type:ShaderForge.SFN_Clamp01,id:6181,x:32680,y:33118,varname:node_6181,prsc:2|IN-1402-OUT;n:type:ShaderForge.SFN_OneMinus,id:1988,x:31539,y:32973,varname:node_1988,prsc:2|IN-5145-OUT;n:type:ShaderForge.SFN_Slider,id:5145,x:31227,y:32973,ptovrint:False,ptlb:BorderWidth,ptin:_BorderWidth,varname:node_5145,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.5;n:type:ShaderForge.SFN_Color,id:4967,x:32281,y:32980,ptovrint:False,ptlb:BorderColor,ptin:_BorderColor,varname:node_4967,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;proporder:7241-7914-8124-5145-4967;pass:END;sub:END;*/

Shader "Shader Forge/QuadBetweenPlayers" {
    Properties {
        _CentralColor ("CentralColor", Color) = (0.8620689,1,0,1)
        _EmissiveMultiplier ("EmissiveMultiplier", Float ) = 3
        _Opacity ("Opacity", Range(0, 1)) = 0
        _BorderWidth ("BorderWidth", Range(0, 0.5)) = 0
        _BorderColor ("BorderColor", Color) = (0,0,0,1)
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _CentralColor;
            uniform float _EmissiveMultiplier;
            uniform float _Opacity;
            uniform float _BorderWidth;
            uniform float4 _BorderColor;
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
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_1988 = (1.0 - _BorderWidth);
                float node_5455 = (i.uv0.r-node_1988);
                float node_816 = (node_1988+i.uv0.r);
                float node_5006 = (ceil(node_5455)+(1.0 - floor(node_816)));
                float3 emissive = lerp((_CentralColor.rgb*_EmissiveMultiplier),_BorderColor.rgb,node_5006);
                float3 finalColor = emissive;
                return fixed4(finalColor,saturate((node_5006+_Opacity)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
