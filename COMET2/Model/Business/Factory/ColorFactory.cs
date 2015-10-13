using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Business.Factory {
    public class ColorFactory {
        private static string[] colors = {
                "#B71C1C",
                "#1B5E20",
                "#880E4F",
                "#33691E",
                "#4A148C",
                "#827717",
                "#311B92",
                "#F57F17",
                "#1A237E",
                "#FF6F00",
                "#0D47A1",
                "#E65100",
                "#01579B",
                "#BF360C",
                "#006064",
                "#3E2723",
                "#004D40",
                "#212121"
                                  };
        private static string[] lightColors = {
                "#FFCDD2",
                "#C8E6C9",
                "#FB88D0",
                "#DCEDC8",
                "#E1BEE7",
                "#F0F4C3",
                "#D1C4E9",
                "#FFF9C4",
                "#C5CAE9",
                "#FFECB3",
                "#BBDEFB",
                "#FFE0B2",
                "#B3E5FC",
                "#FFCCBC",
                "#B2EBF2",
                "#D7CCC8",
                "#B2DFDB",
                "#F5F5F5"
                                       };

        public static string[] getColors() {
            return colors;
        }

        public static string[] getLightColors() {
            return lightColors;
        }

        public static string getColor(int index) {
            return colors[index];
        }

        public static string getLightColor(int index) {
            return lightColors[index];
        }

        public static string createColor(int rotation, double frequency, int offset = 2) {
            byte red = (byte)(Math.Sin(rotation * frequency + 0) * 127 + 128);
            byte green = (byte)(Math.Sin(rotation * frequency + offset) * 127 + 128);
            byte blue = (byte)(Math.Sin(rotation * frequency + offset * 2) * 127 + 128);
            return rgb2Color(red, green, blue);
        }

        public static string rgb2Color(byte r, byte g, byte b) {
            string nybHexString = "0123456789ABCDEF";
            string red = nybHexString.Substring((r >> 4) & 0x0F, 1) + nybHexString.Substring(r & 0x0F, 1);
            string green = nybHexString.Substring((g >> 4) & 0x0F, 1) + nybHexString.Substring(g & 0x0F, 1);
            string blue = nybHexString.Substring((b >> 4) & 0x0F, 1) + nybHexString.Substring(b & 0x0F, 1);
            return "#" + red + green + blue;
        }

        public static int[] hex2RGB(string hex) {
            var color = System.Drawing.ColorTranslator.FromHtml(hex);
            return new int[] { color.R, color.G, color.B };
        }

        public static int[] lighten(int[] color, double factor) {
            if (factor < 0 || factor > 1)
                return color;
            int r = (int)(factor * color[0] + (1 - factor) * 255);
            int g = (int)(factor * color[1] + (1 - factor) * 255);
            int b = (int)(factor * color[2] + (1 - factor) * 255);
            return new int[] { r, g, b };
        }
    }
}