//Color Palettes from http://www.colourlovers.com/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using OxyPlot;

namespace VisualGrading.Charts
{
    public static class ColorHelper
    {
        private static readonly ResourceDictionary _resources = Application.Current.Resources;

        static ColorHelper()
        {
            MutedColorPalette = new Dictionary<string, string>();

            OxyColorPalete = new List<OxyColor>();

            //TODO: same as AlternatingBackgroundColor. See if you can parse it from _resources. 
            OxyBackgroundColor = OxyColor.Parse("#ECE9E8");

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
            MutedColorPalette.Add("Adrift in Dreams", "#0B486B");

            foreach (var color in MutedColorPalette.Values)
                OxyColorPalete.Add(OxyColor.Parse(color));
        }

        public static Dictionary<string, string> MutedColorPalette { get; }

        public static IList<OxyColor> OxyColorPalete { get; }

        public static OxyColor OxyBackgroundColor { get; }
    }
}