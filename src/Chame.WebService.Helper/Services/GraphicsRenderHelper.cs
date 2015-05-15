using ChameHOT.WebService.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chame.WebService.Helper.Services
{
    public sealed class GraphicsRenderHelper
    {
        public static SizeF MeasureText(Graphics g, RenderItemModel renderItem, float ratio, SizeF size, RectangleF rect)
        {
            var rectR = new RectangleF(rect.Left, rect.Top + size.Height, rect.Width, rect.Height);
            var option = renderItem.TextOption;
            var margin = option.Margin;
            var fontStyle = option.FontWeight.Weight > 500 ? (option.FontSytle.ToDrawingType() | System.Drawing.FontStyle.Bold) : option.FontSytle.ToDrawingType();
            var font = new Font(option.FontFamilyName, option.FontSize / ratio, fontStyle, GraphicsUnit.Pixel);
            var format = new StringFormat()
            {
                Alignment = option.ParagraphAlignment.ToDrawingType(),
                LineAlignment = option.TextAlignment.ToDrawingType()
            };

            var sizeR = g.MeasureString(renderItem.Content, font, new SizeF(rectR.Width, rectR.Height), format);

            return new SizeF((float)(sizeR.Width + margin.Left + margin.Right), (float)(sizeR.Height + margin.Top + margin.Bottom));
        }

        public static void RenderText(Graphics g, RenderItemModel renderItem, float ratio, ref SizeF size, ref RectangleF rect)
        {
            rect = new RectangleF(rect.Left, rect.Top + size.Height, rect.Width, rect.Height);
            var option = renderItem.TextOption;
            var margin = option.Margin;
            var fontStyle = option.FontWeight.Weight > 500 ? (option.FontSytle.ToDrawingType() | System.Drawing.FontStyle.Bold) : option.FontSytle.ToDrawingType();
            var font = new Font(option.FontFamilyName, option.FontSize / ratio, fontStyle, GraphicsUnit.Pixel);
            var brush = new SolidBrush(System.Drawing.Color.FromArgb(option.Foreground.A, option.Foreground.R, option.Foreground.G, option.Foreground.B));
            var format = new StringFormat()
            {
                Alignment = option.ParagraphAlignment.ToDrawingType(),
                LineAlignment = option.TextAlignment.ToDrawingType()
            };

            size = g.MeasureString(renderItem.Content, font, new SizeF(rect.Width, rect.Height), format);

            // Check and change the text alignment
            var renderRect = new RectangleF((float)(rect.Left + margin.Left),
                                  (float)(rect.Top + margin.Top),
                                  (float)(rect.Width + margin.Left + margin.Right),
                                  (float)(rect.Height + margin.Top + margin.Bottom));
            switch (option.HorizontalAlignment)
            {
                case TextHorizontalAlignment.Right:
                    renderRect = new RectangleF((float)(rect.Left + margin.Left + rect.Width - size.Width),
                                                (float)(rect.Top + margin.Top),
                                                (float)(rect.Width + margin.Left + margin.Right),
                                                (float)(rect.Height + margin.Top + margin.Bottom));
                    break;
                case TextHorizontalAlignment.Center:
                    renderRect = new RectangleF((float)(rect.Left + margin.Left + (rect.Width - size.Width) / 2.0),
                                                (float)(rect.Top + margin.Top),
                                                (float)(rect.Width + margin.Left + margin.Right),
                                                (float)(rect.Height + margin.Top + margin.Bottom));
                    break;
            }

            g.DrawString(renderItem.Content, font, brush, renderRect, format);

            size = new SizeF((float)(size.Width + margin.Left + margin.Right), (float)(size.Height + margin.Top + margin.Bottom));
        }
    }
}
