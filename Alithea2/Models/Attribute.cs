using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alithea2.Models
{
    public class Attribute
    {

        public ColorProduct Color;
        public enum ColorProduct
        {
            color_Black = 0,
            color_Red = 1,
            color_Blue = 2,
            color_Yellow = 3,
            color_Green = 4,
            color_Pink = 5,
            color_Violet = 6,
            color_White = 7,
        }

        public SizeProduct Size;
        public enum SizeProduct
        {
            size_M = 0,
            size_L = 1,
            size_XL = 3,
        }
    }
}