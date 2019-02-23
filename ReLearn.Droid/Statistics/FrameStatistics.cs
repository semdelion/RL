﻿using Android.Graphics;
using ReLearn.Droid.Helpers;
using System;
using static Android.Graphics.Shader;

namespace ReLearn.Droid
{
    class FrameStatistics
    {
        public float Left { get; }
        public float Right { get; }
        public float Top { get; }
        public float Bottom { get; }
        public float Width { get => Right - Left; }
        public float Height { get => Bottom - Top; }
        public static Typeface Plain { get; set; }
        Paint PaintBackground;

        public FrameStatistics(float left, float top, float right, float bottom, Color color)
        {
            Left = left; Bottom = bottom; Right = right; Top = top;
            PaintBackground = new Paint { Color = color, AntiAlias = true };
        }

        public FrameStatistics(float left, float top, float right, float bottom, Color color1, Color color2)
        {
            Left = left; Bottom = bottom; Right = right; Top = top;
            PaintBackground = new Paint { AntiAlias = true };
            Shader shader = new LinearGradient(0, Top, 0, Bottom, color1, color2, TileMode.Clamp);
            PaintBackground.SetShader(shader);
        }

        public void Draw(Canvas canvas)
        {
            LinearGradient backlg = new LinearGradient((canvas.Width + canvas.Height) / 200, 0, 0, (canvas.Width + canvas.Height) / 200,
                Color.Argb(40, 60, 90, 125), Color.Transparent, TileMode.Repeat);
            Paint paint1 = new Paint { AntiAlias = true };
            paint1.SetShader(backlg);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint1);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, PaintBackground);
        }

        public void DrawBorder(Canvas canvas, Paint paint)
        {
            LinearGradient backlg = new LinearGradient((canvas.Width + canvas.Height) / 200, 0, 0, (canvas.Width + canvas.Height) / 200,
                Color.Argb(40, 60, 90, 125), Color.Transparent, TileMode.Repeat);
            Paint paint1 = new Paint { AntiAlias = true };
            paint1.SetShader(backlg);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint1);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, PaintBackground);
            canvas.DrawRoundRect(new RectF(Left, Top, Right, Bottom), 6, 6, paint);
        }

        public void ProgressLine(Canvas canvas, float True, float False, Color Color_Diagram_1, Color Color_Diagram_2)
        {
            float padding_left_right = 7f * Width / 100f;
            float padding_bottom = 14f * Height / 100f;
            float height = 15f * ((Bottom - Top) / 100f);
            float step_width = (Width - padding_left_right * 2) / (True + False);

            Shader shader = new LinearGradient(0, Left + padding_left_right, Right - padding_left_right, 0, Color_Diagram_2, Color_Diagram_1, TileMode.Clamp);
            Paint Pen_F = new Paint { Color = Color.Rgb(29, 43, 59), AntiAlias = true };
            Paint Pen_T = new Paint { AntiAlias = true };
            Pen_T.SetShader(shader);

            canvas.DrawRect(new RectF(Left + padding_left_right, Bottom - padding_bottom - height, Left + padding_left_right + step_width * True, Bottom - padding_bottom), Pen_T);
            canvas.DrawRect(new RectF(Left + padding_left_right + step_width * True, Bottom - padding_bottom - height, Right - padding_left_right, Bottom - padding_bottom), Pen_F);
        }

        public void DrawPieChartWithText(Canvas canvas, float average, float sum, Color Color_Diagram_1, Color Color_Diagram_2, PointF Center, float Radius)
        {
            Shader shader1 = new SweepGradient(Center.X, Center.Y, Color_Diagram_2, Color_Diagram_1);
            Paint paint1 = new Paint { Color = Color_Diagram_1, StrokeWidth = 10f * Width / 100f, AntiAlias = true };
            Paint paint2 = new Paint { Color = Color.Rgb(29, 43, 59), StrokeWidth = 10f * Width / 100f, AntiAlias = true };
            paint1.SetStyle(Paint.Style.Stroke);
            paint1.SetShader(shader1);
            paint2.SetStyle(Paint.Style.Stroke);

            canvas.DrawArc(new RectF(Center.X - Radius, Center.Y - Radius, Center.X + Radius, Center.Y + Radius), 0f, 360f, false, paint2);
            canvas.Rotate(-90f, Center.X, Center.Y);
            canvas.DrawArc(new RectF(Center.X - Radius, Center.Y - Radius, Center.X + Radius, Center.Y + Radius), 0.5f, 360f - average * (360f / sum), false, paint1);
            canvas.Rotate(90f, Center.X, Center.Y);
            DrawText(canvas, Width * 22f / 100f, RoundOfNumber(100 - average * 100f / sum) + "%", Left + 2f * Width / 10f, Center.Y - 33f * Radius / 100);
        }

        public void DrawPieChart(Canvas canvas, float average, float sum, Color Color_Diagram_1, Color Color_Diagram_2, float Radius, float width)
        {
            float X = Left + Width / 2,
                  Y = Top + Height / 2;
            Shader shader1 = new SweepGradient(X, Y, Color_Diagram_2, Color_Diagram_1);
            Paint paint1 = new Paint { Color = Color_Diagram_1, StrokeWidth = width, AntiAlias = true };
            Paint paint2 = new Paint { Color = Color.Rgb(29, 43, 59), StrokeWidth = width, AntiAlias = true };
            paint1.SetStyle(Paint.Style.Stroke);
            paint1.SetShader(shader1);
            paint2.SetStyle(Paint.Style.Stroke);
            canvas.DrawArc(new RectF(X - Radius, Y - Radius, X + Radius, Y + Radius), 0f, 360f, false, paint2);
            canvas.Rotate(-90f, X, Y);
            canvas.DrawArc(new RectF(X - Radius, Y - Radius, X + Radius, Y + Radius), 0.5f, 360f - average * (360f / sum), false, paint1);
            canvas.Rotate(90f, X, Y);
        }

        public void DrawPieChartWithText(Canvas canvas, float average, float sum, Color Color_Diagram_1, Color Color_Diagram_2)
        {
            float rate = (Width + Height) / 200f;
            Shader shader1 = new SweepGradient(Left + Width / 2, Top + Height / 2, Color_Diagram_2, Color_Diagram_1);
            Paint paint1 = new Paint { Color = Color_Diagram_1, StrokeWidth = 10f * Width / 100f, AntiAlias = true };
            Paint paint2 = new Paint { Color = Color.Rgb(29, 43, 59), StrokeWidth = 10f * Width / 100f, AntiAlias = true };
            paint1.SetStyle(Paint.Style.Stroke);
            paint1.SetShader(shader1);
            paint2.SetStyle(Paint.Style.Stroke);
            canvas.DrawArc(new RectF(Left + rate * 10f, Top + rate * 10f, Right - rate * 10f, Bottom - rate * 10f), 0f, 360f, false, paint2);
            canvas.Rotate(-90f, Left + Width / 2, Top + Height / 2);
            canvas.DrawArc(new RectF(Left + rate * 10f, Top + rate * 10f, Right - rate * 10f, Bottom - rate * 10f), 0.5f, 360f - average * (360f / sum), false, paint1);
            canvas.Rotate(90f, Left + Width / 2f, Top + Height / 2);
            DrawText(canvas, Width * 22f / 100f, RoundOfNumber(100 - average * 100f / sum) + "%", Left + 2f * Width / 10f, Top + 3.2f * Width / 10f);
        }

        public void DrawText(Canvas canvas, float font_size, string text, float left, float top, Color? c = null)
        {
            Typeface bold = Typeface.Create(Plain, TypefaceStyle.Normal);
            Paint paint = new Paint
            {
                AntiAlias = true,
                TextSize = font_size,
                Color = c ?? Colors.White
            };
            paint.SetTypeface(bold);
            canvas.DrawText(Convert.ToString(text), left, top + font_size, paint);
        }

        private static string RoundOfNumber(float number)
        {
            var numberChar = Convert.ToString(number);
            if (numberChar.Length > 4)
                numberChar = numberChar.Remove(4);
            else if (numberChar.Contains(","))
                numberChar += "0";
            else
            {
                if (numberChar.Length == 2)
                    numberChar += ".0";
                else if (numberChar.Length == 1)
                    numberChar += ".00";
            }
            return numberChar;
        }
    }
}