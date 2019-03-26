// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:34438,y:32655,varname:node_3138,prsc:2|emission-3664-OUT,clip-1137-OUT;n:type:ShaderForge.SFN_Tex2d,id:5891,x:32740,y:32891,ptovrint:False,ptlb:NoiseTexture,ptin:_NoiseTexture,varname:node_5891,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d1f55578916dd4b48b02182dbbad68b5,ntxv:0,isnm:False|UVIN-8058-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8638,x:32337,y:33093,varname:node_8638,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:7467,x:32896,y:33092,varname:node_7467,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-8638-UVOUT;n:type:ShaderForge.SFN_OneMinus,id:9290,x:33063,y:33092,varname:node_9290,prsc:2|IN-7715-OUT;n:type:ShaderForge.SFN_TexCoord,id:8058,x:32511,y:32699,varname:node_8058,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:2754,x:33183,y:32893,varname:node_2754,prsc:2|A-8053-OUT,B-9290-OUT;n:type:ShaderForge.SFN_ComponentMask,id:7966,x:33408,y:32998,varname:node_7966,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-2754-OUT;n:type:ShaderForge.SFN_Step,id:7003,x:33591,y:32998,varname:node_7003,prsc:2|A-7966-OUT,B-8295-OUT;n:type:ShaderForge.SFN_Vector1,id:8295,x:33591,y:33127,varname:node_8295,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Add,id:318,x:33408,y:32760,varname:node_318,prsc:2|A-2754-OUT,B-9290-OUT;n:type:ShaderForge.SFN_Color,id:4265,x:33224,y:32250,ptovrint:False,ptlb:LateFlameColor,ptin:_LateFlameColor,varname:node_4265,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.462069,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:7919,x:33408,y:32250,varname:node_7919,prsc:2|A-4265-RGB,B-5831-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5831,x:33224,y:32422,ptovrint:False,ptlb:MultiplyEmissiveLateFlames,ptin:_MultiplyEmissiveLateFlames,varname:node_5831,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.1;n:type:ShaderForge.SFN_Color,id:7017,x:33224,y:32506,ptovrint:False,ptlb:EarlyFlameColor,ptin:_EarlyFlameColor,varname:node_7017,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:609,x:33408,y:32506,varname:node_609,prsc:2|A-7017-RGB,B-4364-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4364,x:33224,y:32667,ptovrint:False,ptlb:MultiplyEmissiveEarlyFlames,ptin:_MultiplyEmissiveEarlyFlames,varname:node_4364,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.5;n:type:ShaderForge.SFN_Lerp,id:3664,x:33759,y:32476,varname:node_3664,prsc:2|A-7919-OUT,B-609-OUT,T-6005-OUT;n:type:ShaderForge.SFN_Subtract,id:6005,x:33616,y:32760,varname:node_6005,prsc:2|A-318-OUT,B-2937-OUT;n:type:ShaderForge.SFN_Vector1,id:2937,x:33616,y:32879,varname:node_2937,prsc:2,v1:0.4;n:type:ShaderForge.SFN_OneMinus,id:3850,x:33792,y:32998,varname:node_3850,prsc:2|IN-7003-OUT;n:type:ShaderForge.SFN_Tex2d,id:2002,x:32740,y:32699,ptovrint:False,ptlb:TwoChannelTex,ptin:_TwoChannelTex,varname:node_2002,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8058-UVOUT;n:type:ShaderForge.SFN_SwitchProperty,id:8053,x:32965,y:32893,ptovrint:False,ptlb:SwitchTwoChanTex,ptin:_SwitchTwoChanTex,varname:node_8053,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5891-R,B-2002-R;n:type:ShaderForge.SFN_Length,id:9482,x:32720,y:33244,varname:node_9482,prsc:2|IN-6462-OUT;n:type:ShaderForge.SFN_RemapRange,id:6462,x:32536,y:33244,varname:node_6462,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-8638-UVOUT;n:type:ShaderForge.SFN_Time,id:3953,x:32381,y:33760,varname:node_3953,prsc:2;n:type:ShaderForge.SFN_Sin,id:1661,x:32549,y:33780,varname:node_1661,prsc:2|IN-3953-T;n:type:ShaderForge.SFN_Add,id:7715,x:32896,y:33244,varname:node_7715,prsc:2|A-9482-OUT,B-1521-OUT;n:type:ShaderForge.SFN_Slider,id:7600,x:32254,y:33522,ptovrint:False,ptlb:Progression,ptin:_Progression,varname:node_7600,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Ceil,id:8946,x:33936,y:33208,varname:node_8946,prsc:2|IN-1605-OUT;n:type:ShaderForge.SFN_Multiply,id:1137,x:34111,y:33185,varname:node_1137,prsc:2|A-3850-OUT,B-8946-OUT;n:type:ShaderForge.SFN_TexCoord,id:2217,x:33268,y:33208,varname:node_2217,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:8652,x:33429,y:33208,varname:node_8652,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-2217-UVOUT;n:type:ShaderForge.SFN_Length,id:534,x:33591,y:33208,varname:node_534,prsc:2|IN-8652-OUT;n:type:ShaderForge.SFN_OneMinus,id:1605,x:33769,y:33208,varname:node_1605,prsc:2|IN-534-OUT;n:type:ShaderForge.SFN_RemapRange,id:7449,x:32579,y:33522,varname:node_7449,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7600-OUT;n:type:ShaderForge.SFN_Multiply,id:1521,x:32739,y:33522,varname:node_1521,prsc:2|A-7449-OUT,B-224-OUT;n:type:ShaderForge.SFN_Vector1,id:224,x:32579,y:33688,varname:node_224,prsc:2,v1:-1;proporder:5891-4265-5831-7017-4364-2002-8053-7600;pass:END;sub:END;*/

Shader "Shader Forge/StylizedFire" {
    Properties {
        _NoiseTexture ("NoiseTexture", 2D) = "white" {}
        _LateFlameColor ("LateFlameColor", Color) = (0,0.462069,1,1)
        _MultiplyEmissiveLateFlames ("MultiplyEmissiveLateFlames", Float ) = 1.1
        _EarlyFlameColor ("EarlyFlameColor", Color) = (1,0,0,1)
        _MultiplyEmissiveEarlyFlames ("MultiplyEmissiveEarlyFlames", Float ) = 1.5
        _TwoChannelTex ("TwoChannelTex", 2D) = "white" {}
        [MaterialToggle] _SwitchTwoChanTex ("SwitchTwoChanTex", Float ) = 0
        _Progression ("Progression", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _NoiseTexture; uniform float4 _NoiseTexture_ST;
            uniform float4 _LateFlameColor;
            uniform float _MultiplyEmissiveLateFlames;
            uniform float4 _EarlyFlameColor;
            uniform float _MultiplyEmissiveEarlyFlames;
            uniform sampler2D _TwoChannelTex; uniform float4 _TwoChannelTex_ST;
            uniform fixed _SwitchTwoChanTex;
            uniform float _Progression;
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
                float4 _NoiseTexture_var = tex2D(_NoiseTexture,TRANSFORM_TEX(i.uv0, _NoiseTexture));
                float4 _TwoChannelTex_var = tex2D(_TwoChannelTex,TRANSFORM_TEX(i.uv0, _TwoChannelTex));
                float node_9290 = (1.0 - (length((i.uv0*2.0+-1.0))+((_Progression*2.0+-1.0)*(-1.0))));
                float node_2754 = (lerp( _NoiseTexture_var.r, _TwoChannelTex_var.r, _SwitchTwoChanTex )*node_9290);
                clip(((1.0 - step(node_2754.r,0.2))*ceil((1.0 - length((i.uv0*2.0+-1.0))))) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = lerp((_LateFlameColor.rgb*_MultiplyEmissiveLateFlames),(_EarlyFlameColor.rgb*_MultiplyEmissiveEarlyFlames),((node_2754+node_9290)-0.4));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _NoiseTexture; uniform float4 _NoiseTexture_ST;
            uniform sampler2D _TwoChannelTex; uniform float4 _TwoChannelTex_ST;
            uniform fixed _SwitchTwoChanTex;
            uniform float _Progression;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _NoiseTexture_var = tex2D(_NoiseTexture,TRANSFORM_TEX(i.uv0, _NoiseTexture));
                float4 _TwoChannelTex_var = tex2D(_TwoChannelTex,TRANSFORM_TEX(i.uv0, _TwoChannelTex));
                float node_9290 = (1.0 - (length((i.uv0*2.0+-1.0))+((_Progression*2.0+-1.0)*(-1.0))));
                float node_2754 = (lerp( _NoiseTexture_var.r, _TwoChannelTex_var.r, _SwitchTwoChanTex )*node_9290);
                clip(((1.0 - step(node_2754.r,0.2))*ceil((1.0 - length((i.uv0*2.0+-1.0))))) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}