// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;

namespace ReactiveGit.Gui.Core.Interactions
{
    /// <summary>
    /// Common interactions that can be used throughout the application.
    /// </summary>
    public static class CommonInteractions
    {
        /// <summary>
        /// A interaction which will determine if you can proceed with a action.
        /// </summary>
        public static readonly Interaction<string, bool> CheckToProceed = new Interaction<string, bool>();

        /// <summary>
        /// A interaction which will get you a text response.
        /// </summary>
        public static readonly Interaction<string, string> GetStringResponse = new Interaction<string, string>();
    }
}
