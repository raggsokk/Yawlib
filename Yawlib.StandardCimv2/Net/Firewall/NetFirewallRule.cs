#region License
//
// NetFirewallRule.cs
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
using System.Collections.Generic;
using System.Diagnostics;

namespace Yawlib.StandardCimv2
{
    /// <summary>
    /// Represens a single firewall rule. Unfortunately it doesn't show ports opened or anything. Just their names. So kinda useless for now.
    /// </summary>
    [DebuggerDisplay("{DisplayName}")]
    [WmiClassName("MSFT_NetFirewallRule")]
    public class NetFirewallRule
    {
        public string CreationClassName { get; set; }
        public string PolicyRuleName { get; set; }
        public UInt16 Action { get; set; }
        public UInt16 ConditionListType { get; set; }
        public string Description { get; set; }
        public UInt16 Direction { get; set; }
        public string DisplayGroup { get; set; }
        public string DisplayName { get; set; }
        public UInt16 EdgeTraversalPolicy { get; set; }
        public string ElementName { get; set; }
        public UInt16 Enabled { get; set; }
        public UInt16[] EnforcementStatus { get; set; }
        public UInt16 ExecutionStrategy { get; set; }
        //public Guid InstanceID { get; set; }
        public string InstanceID { get; set; }
        public bool LocalOnlyMapping { get; set; }
        public bool LooseSourceMapping { get; set; }
        public string Owner { get; set; }
        public List<string> Platforms { get; set; }
        public UInt16 PolicyDecisionStrategy { get; set; }
        public string PolicyStoreSource { get; set; }
        public UInt16 PolicyStoreSourceType { get; set; }
        public UInt16 PrimaryStatus { get; set; }
        public UInt16 Profiles { get; set; }
        public string RuleGroup { get; set; }
        public UInt16 SequencedActions { get; set; }
        public string Status { get; set; }
        public UInt32 StatusCode { get; set; }
    }
}
