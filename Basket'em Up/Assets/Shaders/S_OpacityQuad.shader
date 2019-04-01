// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33526,y:32714,varname:node_3138,prsc:2|emission-4386-OUT,alpha-9157-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31932,y:32489,ptovrint:False,ptlb:BorderColor,ptin:_BorderColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8620689,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:2583,x:31073,y:32802,varname:node_2583,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Subtract,id:5455,x:31305,y:32812,varname:node_5455,prsc:2|A-2583-U,B-1988-OUT;n:type:ShaderForge.SFN_Add,id:816,x:31305,y:32992,varname:node_816,prsc:2|A-1988-OUT,B-2583-U;n:type:ShaderForge.SFN_Ceil,id:8028,x:31467,y:32812,varname:node_8028,prsc:2|IN-5455-OUT;n:type:ShaderForge.SFN_Floor,id:6292,x:31467,y:32992,varname:node_6292,prsc:2|IN-816-OUT;n:type:ShaderForge.SFN_OneMinus,id:3512,x:31621,y:32992,varname:node_3512,prsc:2|IN-6292-OUT;n:type:ShaderForge.SFN_Add,id:5006,x:31794,y:32816,varname:node_5006,prsc:2|A-8028-OUT,B-3512-OUT;n:type:ShaderForge.SFN_Lerp,id:4386,x:32483,y:32814,varname:node_4386,prsc:2|A-9753-OUT,B-4967-RGB,T-9162-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7914,x:31928,y:32651,ptovrint:False,ptlb:EmissiveMultiplier,ptin:_EmissiveMultiplier,varname:node_7914,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:9753,x:32131,y:32489,varname:node_9753,prsc:2|A-7241-RGB,B-7914-OUT;n:type:ShaderForge.SFN_Slider,id:8124,x:32646,y:33158,ptovrint:False,ptlb:BorderOpacity,ptin:_BorderOpacity,varname:node_8124,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_OneMinus,id:1988,x:31052,y:32975,varname:node_1988,prsc:2|IN-5145-OUT;n:type:ShaderForge.SFN_Slider,id:5145,x:30740,y:32975,ptovrint:False,ptlb:BorderWidth,ptin:_BorderWidth,varname:node_5145,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.5;n:type:ShaderForge.SFN_Color,id:4967,x:32265,y:32987,ptovrint:False,ptlb:CentralColor,ptin:_CentralColor,varname:node_4967,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_OneMinus,id:9162,x:31967,y:32816,varname:node_9162,prsc:2|IN-5006-OUT;n:type:ShaderForge.SFN_ComponentMask,id:5345,x:32651,y:32978,varname:node_5345,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-4386-OUT;n:type:ShaderForge.SFN_Clamp01,id:4730,x:32803,y:32978,varname:node_4730,prsc:2|IN-5345-OUT;n:type:ShaderForge.SFN_Multiply,id:9355,x:33013,y:32978,varname:node_9355,prsc:2|A-4730-OUT,B-8124-OUT;n:type:ShaderForge.SFN_OneMinus,id:9515,x:33013,y:33132,varname:node_9515,prsc:2|IN-4730-OUT;n:type:ShaderForge.SFN_Multiply,id:9936,x:33189,y:33132,varname:node_9936,prsc:2|A-9515-OUT,B-5681-OUT;n:type:ShaderForge.SFN_Slider,id:5681,x:32646,y:33314,ptovrint:False,ptlb:CenterOpacity,ptin:_CenterOpacity,varname:_BorderOpacity_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:9157,x:33345,y:32976,varname:node_9157,prsc:2|A-9355-OUT,B-9936-OUT;proporder:7241-7914-8124-5145-4967-5681;pass:END;sub:END;*/

Shader "Shader Forge/QuadBetweenPlayers" {
    Properties {
        _BorderColor ("BorderColor", Color) = (0.8620689,1,0,1)
        _EmissiveMultiplier ("EmissiveMultiplier", Float ) = 3
        _BorderOpacity ("BorderOpacity", Range(0, 1)) = 0
        _BorderWidth ("BorderWidth", Range(0, 0.5)) = 0
        _CentralColor ("CentralColor", Color) = (0,0,0,1)
        _CenterOpacity ("CenterOpacity", Range(0, 1)) = 0
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
            uniform float4 _BorderColor;
            uniform float _EmissiveMultiplier;
            uniform float _BorderOpacity;
            uniform float _BorderWidth;
            uniform float4 _CentralColor;
            uniform float _CenterOpacity;
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
                float node_5006 = (ceil((i.uv0.r-node_1988))+(1.0 - floor((node_1988+i.uv0.r))));
                float node_9162 = (1.0 - node_5006);
                float3 node_4386 = lerp((_BorderColor.rgb*_EmissiveMultiplier),_CentralColor.rgb,node_9162);
                float3 emissive = node_4386;
                float3 finalColor = emissive;
                float node_5345 = node_4386.g;
                float node_4730 = saturate(node_5345);
                return fixed4(finalColor,((node_4730*_BorderOpacity)+((1.0 - node_4730)*_CenterOpacity)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
