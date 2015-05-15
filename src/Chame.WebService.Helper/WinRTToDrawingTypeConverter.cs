using This = ChameHOT.WebService.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chame.WebService.Helper
{
    public static class WinRTToDrawingTypeConverter
    {
        #region FontStyle extension method

        public static System.Drawing.FontStyle ToDrawingType(this This.FontStyle value)
        {
            switch (value)
            {
                case This.FontStyle.Italic:
                    return System.Drawing.FontStyle.Italic;
                case This.FontStyle.Normal:
                    return System.Drawing.FontStyle.Regular;
                case This.FontStyle.Oblique:
                    return System.Drawing.FontStyle.Regular;
            }
            return System.Drawing.FontStyle.Regular;
        }

        #endregion

        #region ParagraphAlignment extension method

        public static System.Drawing.StringAlignment ToDrawingType(this This.ParagraphAlignment value)
        {
            switch (value)
            {
                case This.ParagraphAlignment.Far:
                    return System.Drawing.StringAlignment.Far;
                case This.ParagraphAlignment.Near:
                    return System.Drawing.StringAlignment.Near;
                case This.ParagraphAlignment.Center:
                    return System.Drawing.StringAlignment.Center;
            }
            return System.Drawing.StringAlignment.Near;
        }

        #endregion

        #region TextAlignment extension method

        public static System.Drawing.StringAlignment ToDrawingType(this This.TextAlignment value)
        {
            switch (value)
            {
                case This.TextAlignment.Center:
                    return System.Drawing.StringAlignment.Center;
                case This.TextAlignment.Justified:
                    return System.Drawing.StringAlignment.Center;
                case This.TextAlignment.Leading:
                    return System.Drawing.StringAlignment.Near;
                case This.TextAlignment.Trailing:
                    return System.Drawing.StringAlignment.Far;
            }
            return System.Drawing.StringAlignment.Near;
        }

        #endregion
    }
}
