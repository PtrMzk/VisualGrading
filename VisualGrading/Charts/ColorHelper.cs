#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// ColorHelper.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System.Collections.Generic;
using System.Windows;
using OxyPlot;

#endregion

namespace VisualGrading.Charts
{
    //Color Palettes from http://www.colourlovers.com/

    public static class ColorHelper
    {
        #region Fields

        private static readonly ResourceDictionary _resources = Application.Current.Resources;

        #endregion

        #region Constructors

        static ColorHelper()
        {
            MutedColorPalette = new Dictionary<string, string>();

            OxyColorPalete = new List<OxyColor>();

            //TODO: same as AlternatingBackgroundColor. See if you can parse it from _resources. 
            OxyBackgroundColor = OxyColor.Parse("#ebebeb");

            MutedColorPalette.Add("Medium Navigation Green", "#2F7177");

            //i demand a pancake
            //MutedColorPalette.Add("Dark Purple (borrowed from forever lost)", "#5D4157");
            //MutedColorPalette.Add("Rock my world", "#547980");
            //MutedColorPalette.Add("med green blue", "#45ADA8");
            // MutedColorPalette.Add("fetch me some", "#9DE0AD");
            //MutedColorPalette.Add("pancakes", "#E5FCC2");

            //mellon ball surprise
            //MutedColorPalette.Add("splash of lime", "#D1F2A5");
            //MutedColorPalette.Add("honey-do", "#EFFAB4");
            //MutedColorPalette.Add("orange sherbert", "#FFC48C");
            //MutedColorPalette.Add("feeling orange", "#FF9F80");
            //MutedColorPalette.Add("watermillion", "#F56991");

            //lovers in japan
            ////MutedColorPalette.Add("LIJ Heartbeat", "#E94E77");
            //MutedColorPalette.Add("LIJ Dusty Rose", "#D68189");
            //MutedColorPalette.Add("LIJ Cafe", "#C6A49A");
            ////MutedColorPalette.Add("LIJ Tidal Pool", "#C6E5D9");
            //MutedColorPalette.Add("LIJ Cozy Blanket", "#F4EAD5");

            //adrift in dreams
            //MutedColorPalette.Add("Sunlit Sea", "#CFF09E");
            //MutedColorPalette.Add("Sea Foaming", "#A8DBA8");
            //MutedColorPalette.Add("Sea Showing Green", "#79BD9A");
            //MutedColorPalette.Add("There We Could Sail", "#3B8686");
            //MutedColorPalette.Add("Adrift in Dreams", "#0B486B");

            foreach (var color in MutedColorPalette.Values)
                OxyColorPalete.Add(OxyColor.Parse(color));
        }

        #endregion

        #region Properties

        public static Dictionary<string, string> MutedColorPalette { get; }

        public static IList<OxyColor> OxyColorPalete { get; }

        public static OxyColor OxyBackgroundColor { get; }

        #endregion
    }
}