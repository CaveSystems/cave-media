﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cave.Media.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Cave.Media.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die #version 120
        ///
        ///uniform float shaderAlpha;
        ///uniform vec4 shaderTint;
        ///uniform sampler2D shaderTextureData;
        ///
        ///varying vec2 TextureCoordinates;
        ///
        ///void main(void)
        ///{
        ///    // mix color from texture with flat color, use alpha of flat color for interpolation
        ///    vec4 color = mix(texture2D(shaderTextureData, TextureCoordinates), vec4(shaderTint.rgb, 1), shaderTint.a);
        ///    // calculate transparency
        ///    gl_FragColor = vec4(color.rgb, color.a * shaderAlpha);
        ///} ähnelt.
        /// </summary>
        internal static string Glfw3FragmentShader {
            get {
                return ResourceManager.GetString("Glfw3FragmentShader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die #version 120
        ///
        ///attribute vec2 shaderVertexPosition;
        ///attribute vec2 shaderTextureCoordinates;
        ///uniform vec3 shaderRotation;
        ///uniform vec3 shaderTranslation;
        ///uniform vec3 shaderScale;
        ///
        ///varying vec2 TextureCoordinates;
        ///
        ///mat3 angleVec (vec3 angles)
        ///{
        ///    vec3 s = sin(radians(angles*360));
        ///    vec3 c = cos(radians(angles*360));
        ///    return mat3(c.z, s.z, 0, -s.z, c.z, 0, 0, 0, 1) * mat3(c.y, 0, -s.y, 0, 1, 0, s.y, 0, c.y) * mat3(1, 0, 0, 0, c.x, s.x, 0, -s.x, c.x);
        ///}
        ///
        ///void main(void)
        ///{
        ///    //calcu [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string Glfw3VertexShader {
            get {
                return ResourceManager.GetString("Glfw3VertexShader", resourceCulture);
            }
        }
    }
}