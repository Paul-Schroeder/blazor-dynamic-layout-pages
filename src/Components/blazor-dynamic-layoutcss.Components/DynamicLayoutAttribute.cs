using System;
using System.ComponentModel;

namespace blazor_dynamic_layoutcss.Components
{
    /// <summary>
    /// Indicates that the associated component type uses a specified layout.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DynamicLayoutAttribute : Attribute
    {
        /// <summary>
        /// The type of the layout. The type must implement <see cref="IComponent"/>
        /// and must accept a parameter with the name 'Body'.
        /// </summary>
        public Type LayoutType { get; private set; }
    }
}