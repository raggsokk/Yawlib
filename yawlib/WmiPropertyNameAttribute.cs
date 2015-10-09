#region License
//
// WmiPropertyNameAttribute.cs
// 
// The MIT License (MIT)
//
// Copyright (c) 2015 Jarle Hansen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE. 
//
#endregion

using System;
using System.Reflection;
using System.Diagnostics;

namespace yawlib
{
    /// <summary>
    /// Enables mapping .net friendly names to WMI property names.
    /// </summary>
    [DebuggerDisplay("{WmiPropertyName}")]
    public class WmiPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// The name to use during wmi mapping.
        /// </summary>
        public string WmiPropertyName { get; set; }

        /// <summary>
        /// Enables mapping .net friendly names to WMI property names.
        /// </summary>
        /// <param name="WmiPropertyName">Override the name to use during wmi property mapping.</param>
        [DebuggerNonUserCode()]
        public WmiPropertyNameAttribute(string WmiPropertyName)
        {
            this.WmiPropertyName = WmiPropertyName;
        }
    }
}
