using Cave.Text;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Cave
{
    /// <summary>
    /// Provides public access to the Cave System Media Assembly instance
    /// </summary>
    public static class CaveSystemMedia
    {
        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public static Type Type { get { return typeof(CaveSystemMedia); } }


        /// <summary>
        /// Obtains the assembly
        /// </summary>
        public static Assembly Assembly { get { return Type.Assembly; } }

        /// <summary>
        /// Obtains the <see cref="AssemblyVersionInfo"/> for the <see cref="Assembly"/>
        /// </summary>
        public static AssemblyVersionInfo VersionInfo { get { return AssemblyVersionInfo.FromAssembly(Assembly); } }
    }
}
